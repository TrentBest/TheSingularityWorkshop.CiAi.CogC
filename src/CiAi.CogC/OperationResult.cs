namespace CiAi.CogC;

/// <summary>A claimed operation outcome; consumers must validate its correlation and evidence.</summary>
public sealed record OperationResult(
    string RequestId,
    string CorrelationId,
    string IdempotencyKey,
    string ContractVersion,
    string SourceRepository,
    string TargetRepository,
    string OperationId,
    string State,
    string Conclusion,
    long? RunId,
    string? HeadSha,
    string? EvidenceUrl,
    DateTimeOffset ObservedAtUtc);

public static class ResultStates
{
    public const string Pending = "pending";
    public const string Succeeded = "succeeded";
    public const string Failed = "failed";
    public const string Rejected = "rejected";
    public const string Stale = "stale";
    public const string Unknown = "unknown";
}

/// <summary>Expected request identity from the trusted coordinator, not from the callback.</summary>
public sealed record ExpectedOperation(string RequestId, string CorrelationId, string IdempotencyKey, string ContractVersion, string SourceRepository, string TargetRepository, string OperationId);
