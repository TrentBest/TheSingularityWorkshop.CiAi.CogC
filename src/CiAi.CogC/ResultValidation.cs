namespace CiAi.CogC;

/// <summary>Validates claimed results against coordinator-owned request identity and GitHub run evidence.</summary>
public static class ResultValidation
{
    private static readonly HashSet<string> KnownStates = new(StringComparer.Ordinal)
    {
        ResultStates.Pending, ResultStates.Succeeded, ResultStates.Failed,
        ResultStates.Rejected, ResultStates.Stale, ResultStates.Unknown
    };

    public static IReadOnlyList<string> Validate(OperationResult? result, ExpectedOperation expected)
    {
        ArgumentNullException.ThrowIfNull(expected);
        var errors = new List<string>();
        if (result is null) return new[] { "result.required" };
        if (!string.Equals(result.RequestId, expected.RequestId, StringComparison.Ordinal)) errors.Add("requestId.mismatch");
        if (!string.Equals(result.CorrelationId, expected.CorrelationId, StringComparison.Ordinal)) errors.Add("correlationId.mismatch");
        if (!string.Equals(result.IdempotencyKey, expected.IdempotencyKey, StringComparison.Ordinal)) errors.Add("idempotencyKey.mismatch");
        if (!string.Equals(result.ContractVersion, expected.ContractVersion, StringComparison.Ordinal)) errors.Add("contractVersion.mismatch");
        if (!string.Equals(result.SourceRepository, expected.SourceRepository, StringComparison.OrdinalIgnoreCase)) errors.Add("sourceRepository.mismatch");
        if (!string.Equals(result.TargetRepository, expected.TargetRepository, StringComparison.OrdinalIgnoreCase)) errors.Add("targetRepository.mismatch");
        if (!string.Equals(result.OperationId, expected.OperationId, StringComparison.Ordinal)) errors.Add("operationId.mismatch");
        if (!KnownStates.Contains(result.State)) errors.Add("state.unknown");
        if (result.ObservedAtUtc == default || result.ObservedAtUtc.Offset != TimeSpan.Zero) errors.Add("observedAtUtc.invalid");

        if (result.State == ResultStates.Succeeded)
        {
            if (!string.Equals(result.Conclusion, "success", StringComparison.OrdinalIgnoreCase)) errors.Add("success.conclusion-required");
            if (result.RunId is null or <= 0) errors.Add("success.runId-required");
            if (result.HeadSha is null || result.HeadSha.Length != 40 || !result.HeadSha.All(Uri.IsHexDigit)) errors.Add("success.headSha-invalid");
            if (!HasMatchingEvidence(result)) errors.Add("success.evidence-invalid");
        }
        else if (result.State == ResultStates.Failed && string.Equals(result.Conclusion, "success", StringComparison.OrdinalIgnoreCase))
        {
            errors.Add("failure.conclusion-conflict");
        }
        return errors;
    }

    private static bool HasMatchingEvidence(OperationResult result)
    {
        if (result.RunId is null or <= 0 || string.IsNullOrWhiteSpace(result.EvidenceUrl)) return false;
        if (string.IsNullOrWhiteSpace(result.TargetRepository)) return false;
        var expected = $"https://github.com/{result.TargetRepository}/actions/runs/{result.RunId.Value}";
        return Uri.TryCreate(result.EvidenceUrl, UriKind.Absolute, out var actual)
            && string.Equals(actual.AbsoluteUri.TrimEnd('/'), expected, StringComparison.OrdinalIgnoreCase)
            && string.Equals(actual.Host, "github.com", StringComparison.OrdinalIgnoreCase)
            && actual.Query.Length == 0 && actual.Fragment.Length == 0;
    }
}
