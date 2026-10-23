using CiAi.CogC;

var expected = new ExpectedOperation("req-1", "corr-1", "idem-1", "1.0.0", "TrentBest/CiAi", "TrentBest/Target", "observe.status");
var good = new OperationResult("req-1", "corr-1", "idem-1", "1.0.0", "TrentBest/CiAi", "TrentBest/Target", "observe.status", ResultStates.Succeeded, "success", 123456, new string('a', 40), "https://github.com/TrentBest/Target/actions/runs/123456", DateTimeOffset.UtcNow);
Require(ResultValidation.Validate(good, expected).Count == 0, "valid success evidence should pass");
Require(ResultValidation.Validate(good with { CorrelationId = "wrong" }, expected).Contains("correlationId.mismatch"), "wrong correlation must be rejected");
Require(ResultValidation.Validate(good with { IdempotencyKey = "wrong" }, expected).Contains("idempotencyKey.mismatch"), "wrong idempotency key must be rejected");
Require(ResultValidation.Validate(good with { ContractVersion = "2.0.0" }, expected).Contains("contractVersion.mismatch"), "wrong contract version must be rejected");
Require(ResultValidation.Validate(good with { SourceRepository = "TrentBest/Other" }, expected).Contains("sourceRepository.mismatch"), "wrong source must be rejected");
Require(ResultValidation.Validate(good with { TargetRepository = "TrentBest/Other", EvidenceUrl = "https://github.com/TrentBest/Other/actions/runs/123456" }, expected).Contains("targetRepository.mismatch"), "wrong target must be rejected");
Require(ResultValidation.Validate(good with { EvidenceUrl = "https://example.com/actions/runs/123456" }, expected).Contains("success.evidence-invalid"), "foreign evidence host must be rejected");
Require(ResultValidation.Validate(good with { HeadSha = "not-a-sha" }, expected).Contains("success.headSha-invalid"), "malformed SHA must be rejected");
Require(ResultValidation.Validate(good with { Conclusion = "failure" }, expected).Contains("success.conclusion-required"), "success claim with failure conclusion must be rejected");
Require(ResultValidation.Validate(good with { State = "surprise" }, expected).Contains("state.unknown"), "unknown state must be rejected");
var unknown = good with { State = ResultStates.Unknown, Conclusion = "unknown", RunId = null, HeadSha = null, EvidenceUrl = null };
Require(ResultValidation.Validate(unknown, expected).Count == 0, "unknown outcome may remain explicitly unknown without success evidence");
Console.WriteLine("PASS: CogC result-correlation checks");
return;

static void Require(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}
