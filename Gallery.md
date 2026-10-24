# CogC Gallery

CogC is the result-and-correlation boundary for CiAi: it checks claimed operation outcomes against coordinator-owned request identity, constrained repository names, and evidence shape. It intentionally does not treat a well-formed evidence URL as proof of a successful run.

## Recipe: validate a claimed result

Start with the [ResultValidation class companion](src/CiAi.CogC/ResultValidation.md), then inspect the [UML class card and use case](docs/UML/ResultValidationUML.md). Run the offline contract checks with:

```sh
dotnet run --project tests/CiAi.CogC.Tests/CiAi.CogC.Tests.csproj --configuration Release
```

The current implementation is an isolated validation slice. Authoritative GitHub Actions lookup, transport, replay persistence, and end-to-end CogB/CiAi orchestration remain future integration work.

**Readiness: 20%.** See [docs/READINESS.md](docs/READINESS.md) for implemented scope and release gaps.
