# CogC Readiness

**Readiness: 20% — initial result-validation contract, not integrated callbacks.**

## Implemented on development
- Typed operation result with request/correlation identity, source/target, operation, state, conclusion, run ID, head SHA, evidence URL, and UTC observation time.
- Known outcome vocabulary: pending, succeeded, failed, rejected, stale, unknown.
- Coordinator-supplied expected identity prevents callback-controlled correlation.
- Success requires a success conclusion, positive run ID, 40-hex commit SHA, and exact GitHub Actions evidence URL for the claimed target/run.
- Offline tests cover valid success, mismatched correlation/target, foreign evidence host, malformed SHA, conflicting conclusion, unknown state, and explicit unknown outcome.

## Not implemented
- Callback receiver or transport.
- Cryptographic callback authentication or trust in a target's claims.
- Authoritative run lookup from GitHub API.
- Replay persistence, ordering, stale-response arbitration, or history storage.
- Integration with CogB or CiAi.

## Run
    dotnet run --project tests/CiAi.CogC.Tests/CiAi.CogC.Tests.csproj --configuration Release

A syntactically valid evidence URL is not itself proof that a run exists or succeeded. CiAi must fetch and validate the authoritative run before treating success as established.
