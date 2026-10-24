# CiAi.CogC — Result and Correlation Boundary

**Mission:** validate typed operation outcomes against coordinator-owned request identity and evidence.

## 01 — See it in a minute

Requires the .NET 8 SDK.

    dotnet build src/CiAi.CogC/CiAi.CogC.csproj --configuration Release
    dotnet run --project tests/CiAi.CogC.Tests/CiAi.CogC.Tests.csproj --configuration Release

The offline checks cover correlation, idempotency, contract-version, source, and target mismatches, foreign evidence hosts, malformed commit SHAs, conflicting conclusions, and explicitly unknown outcomes. CI runs these checks on development/master pushes and pull requests targeting development.

## 02 — Result contract

An OperationResult carries request, correlation, and idempotency IDs; contract version; source and target repository identities, operation ID, state, conclusion, optional run ID, commit SHA, evidence URL, and UTC observation time. Validation compares the result to an ExpectedOperation supplied by the coordinator, not identity values trusted from a callback.

Known states are pending, succeeded, failed, rejected, stale, and unknown. A claimed success requires a success conclusion, positive run ID, 40-character hexadecimal SHA, and a URL matching the target repository and run ID.

## 03 — Evidence boundary

A URL that has the right shape is **not** proof that the run exists or succeeded. CogC currently validates a claim's structure and correlation; CiAi must fetch the authoritative GitHub Actions run and verify repository, run ID, conclusion, and head SHA before establishing success.

No callback receiver, cryptographic callback authentication, persistent replay protection, history store, live transport, or CogB/CiAi integration is implemented yet.

## 04 — Documentation

- [Gallery](Gallery.md): short overview and recipe entry point.
- [ResultValidation class companion](src/CiAi.CogC/ResultValidation.md): purpose, contract, and trust boundary.
- [ResultValidation UML and use case](docs/UML/ResultValidationUML.md): class relationships and coordinator validation flow.
- [Readiness](docs/READINESS.md): implemented scope and remaining gaps.

## 05 — Status

**Readiness: 20%.** Initial result-validation contract, not integrated callbacks. See [READINESS.md](docs/READINESS.md).
