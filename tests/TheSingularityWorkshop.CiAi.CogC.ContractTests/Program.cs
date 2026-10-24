using TheSingularityWorkshop.CiAi.CogC;
var passed = 0;
var expected = new CapabilityRequestExpectation("request-1", "TrentBest/Gui", "abcdef0123456789abcdef0123456789abcdef01", 1);
Check("matching published result correlates cleanly", () => Assert(ResultCorrelator.Validate(expected, NewResult()).Count == 0, "matching result rejected"));
Check("wrong request is rejected", () => Assert(ResultCorrelator.Validate(expected, NewResult(requestId: "request-2")).Contains("requestId.mismatch"), "request mismatch missed"));
Check("wrong provider and source revision are rejected", () => { var e = ResultCorrelator.Validate(expected, NewResult(provider: "TrentBest/Other", revision: new string('b', 40))); Assert(e.Contains("providerRepository.mismatch") && e.Contains("sourceRevision.mismatch"), "provenance mismatch missed"); });
Check("missing result is explicit", () => Assert(ResultCorrelator.Validate(expected, null).Contains("result.required"), "missing result not reported"));
Check("digest and source revision must be full hex", () => { Throws<ArgumentException>(() => NewResult(digest: "bad")); Throws<ArgumentException>(() => NewResult(revision: "main")); });
Check("undefined result status is rejected", () => Throws<ArgumentOutOfRangeException>(() => NewResult(status: (CapabilityResultStatus)999)));
Check("unknown and failed are not silently promoted to published", () => { Assert(NewResult(status: CapabilityResultStatus.Unknown).Status != CapabilityResultStatus.Published, "unknown promoted"); Assert(NewResult(status: CapabilityResultStatus.Failed).Status != CapabilityResultStatus.Published, "failure promoted"); });
Console.WriteLine($"PASS: {passed} CogC contract checks.");
void Check(string name, Action action) { action(); passed++; Console.WriteLine($"PASS {passed}: {name}"); }
static CapabilityResult NewResult(string requestId="request-1", string provider="TrentBest/Gui", string revision="abcdef0123456789abcdef0123456789abcdef01", string digest="aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", CapabilityResultStatus status=CapabilityResultStatus.Published) => new(requestId, provider, "artifact-1", "v1", digest, revision, status, 1, 42);
static void Assert(bool condition, string message) { if (!condition) throw new InvalidOperationException(message); }
static void Throws<TException>(Action action) where TException : Exception { try { action(); } catch (TException) { return; } throw new InvalidOperationException($"Expected {typeof(TException).Name}."); }