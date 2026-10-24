# CogC Gallery

CogC is a small, dependency-free CiAi control-plane component. It makes one responsibility explicit and testable instead of hiding policy, identity, correlation, or lifecycle assumptions in coordinator code.

## See it in a minute

```bash
dotnet run --project tests/TheSingularityWorkshop.CiAi.CogC.ContractTests --configuration Release
```

## Recipe

- [ResultCorrelator UML and use case](docs/UML/ResultCorrelatorUML.md)
- [ResultCorrelator class companion](src/TheSingularityWorkshop.CiAi.CogC/ResultCorrelator.md)
- [CI contract verification](https://github.com/TrentBest/TheSingularityWorkshop.CiAi.CogC/actions)

This is a contract demonstration, not a claim of end-to-end CiAi orchestration.
