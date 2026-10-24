# The Singularity Workshop CiAi.CogC

**CogC correlates provider results with the exact request that authorized the work.** It validates identity and provenance metadata; it does not decide whether an output is semantically correct or accepted.

## 01 — See it in a minute

Requires .NET 8:

```bash
dotnet run --project tests/TheSingularityWorkshop.CiAi.CogC.ContractTests --configuration Release
```

## 02 — Responsibility

CogC compares request ID, provider repository, pinned source SHA, and contract version. It preserves provider-reported Published, Failed, Partial, Cancelled, and Unknown as distinct states. The parent CiAi coordinator must still verify authoritative provider CI facts and the artifact bytes.

## 03 — Trust boundary

- Provider claims are untrusted until correlated and verified.
- A matching digest proves byte identity, not semantic correctness.
- Unknown, partial, failed, and cancelled results are never upgraded to success.
- No network access, persistence, or execution.

## 04 — UML and use case

See [ResultCorrelator UML and use case](docs/UML/ResultCorrelatorUML.md).

## 05 — Release status

**Implementation readiness: 25%. Package preparation: 25%.** Typed result metadata and correlation checks are committed; shared parent contracts, artifact-manifest integration, authoritative run verification, and durable result recording remain outstanding. Not a publication approval.