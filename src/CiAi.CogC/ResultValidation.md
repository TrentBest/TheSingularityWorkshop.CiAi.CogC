# ResultValidation

**User moniker:** CogC Result and Correlation Boundary

## Purpose

`ResultValidation` checks a claimed `OperationResult` against an `ExpectedOperation` supplied by the trusted coordinator. It returns stable error identifiers rather than throwing for malformed result fields.

## Contract

- Requires a non-null expected operation.
- Validates source and target repository identities as two non-empty slash-separated components, with bounded component/total lengths and ASCII alphanumeric, hyphen, underscore, or period characters; dot-only path segments are rejected.
- Compares request, correlation, idempotency, contract version, source, target, and operation identity.
- Restricts state to the known `ResultStates` vocabulary and requires a zero-offset observation timestamp.
- For `succeeded`, requires a success conclusion, positive run ID, 40-character hexadecimal head SHA, and evidence URL matching the target repository and run ID.

## Trust boundary

This is structural and correlation validation only. A matching URL is not authoritative evidence that a GitHub Actions run exists or succeeded. The parent CiAi coordinator must fetch the run independently and compare repository, run ID, conclusion, and head SHA before accepting success.

## Related types

- `OperationResult`: the claimed outcome.
- `ExpectedOperation`: coordinator-owned expected identity.
- `ResultStates`: finite outcome vocabulary.

## Verification

Run `dotnet run --project tests/CiAi.CogC.Tests/CiAi.CogC.Tests.csproj --configuration Release`. The repository workflow runs the offline contract checks on development/master pushes and pull requests targeting development.
