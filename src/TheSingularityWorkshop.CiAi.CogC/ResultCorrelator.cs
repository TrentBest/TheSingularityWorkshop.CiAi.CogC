namespace TheSingularityWorkshop.CiAi.CogC;

/// <summary>Outcome metadata returned by a capability provider; payload bytes are transported separately.</summary>
public sealed class CapabilityResult
{
    public CapabilityResult(string requestId, string providerRepository, string artifactId, string artifactVersion,
        string digest, string sourceRevision, CapabilityResultStatus status, int contractVersion, long? providerRunId = null)
    {
        RequestId = Token(requestId, nameof(requestId));
        ProviderRepository = Repository(providerRepository);
        ArtifactId = Token(artifactId, nameof(artifactId));
        ArtifactVersion = Token(artifactVersion, nameof(artifactVersion));
        Digest = Hex(digest, 64, nameof(digest));
        SourceRevision = Hex(sourceRevision, 40, nameof(sourceRevision));
        if (contractVersion < 1) throw new ArgumentOutOfRangeException(nameof(contractVersion));
        if (providerRunId is <= 0) throw new ArgumentOutOfRangeException(nameof(providerRunId));
        Status = status;
        ContractVersion = contractVersion;
        ProviderRunId = providerRunId;
    }
    public string RequestId { get; }
    public string ProviderRepository { get; }
    public string ArtifactId { get; }
    public string ArtifactVersion { get; }
    public string Digest { get; }
    public string SourceRevision { get; }
    public CapabilityResultStatus Status { get; }
    public int ContractVersion { get; }
    public long? ProviderRunId { get; }

    private static string Token(string value, string parameter)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 128 || value.Any(c => !(char.IsAsciiLetterOrDigit(c) || c is '-' or '_' or '.')))
            throw new ArgumentException("Value must be a bounded identifier.", parameter);
        return value;
    }
    private static string Repository(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Repository is required.", nameof(value));
        var p = value.Split('/');
        if (p.Length != 2 || p.Any(x => x.Length is < 1 or > 100 || x.Any(c => !(char.IsAsciiLetterOrDigit(c) || c is '-' or '_' or '.'))))
            throw new ArgumentException("Repository must be owner/repository.", nameof(value));
        return value;
    }
    private static string Hex(string value, int length, string parameter)
    {
        if (value is null || value.Length != length || !value.All(Uri.IsHexDigit))
            throw new ArgumentException($"Value must be {length} hexadecimal characters.", parameter);
        return value.ToLowerInvariant();
    }
}

/// <summary>Provider-reported result state. Only Published implies an artifact reference is intended to exist.</summary>
public enum CapabilityResultStatus { Published, Failed, Partial, Cancelled, Unknown }

/// <summary>Correlates result metadata to the exact request before the coordinator considers its payload.</summary>
public static class ResultCorrelator
{
    public static IReadOnlyList<string> Validate(CapabilityRequestExpectation request, CapabilityResult? result)
    {
        ArgumentNullException.ThrowIfNull(request);
        var errors = new List<string>();
        if (result is null) { errors.Add("result.required"); return errors.AsReadOnly(); }
        if (result.RequestId != request.RequestId) errors.Add("requestId.mismatch");
        if (result.ProviderRepository != request.ProviderRepository) errors.Add("providerRepository.mismatch");
        if (result.SourceRevision != request.SourceRevision) errors.Add("sourceRevision.mismatch");
        if (result.ContractVersion != request.ContractVersion) errors.Add("contractVersion.mismatch");
        if (result.Status == CapabilityResultStatus.Published && string.IsNullOrWhiteSpace(result.ArtifactId))
            errors.Add("artifactId.required");
        return errors.AsReadOnly();
    }
}

/// <summary>The minimal request fields CogC needs for correlation, independent of a transport or storage implementation.</summary>
public sealed record CapabilityRequestExpectation(string RequestId, string ProviderRepository, string SourceRevision, int ContractVersion);
