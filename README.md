Raport pokrycia kodu:
```
dotnet test --collect:"XPlat Code Coverage"
```
Istalacja narzędzia do konwersji raportu z XML:
```
dotnet tool install -g dotnet-reportgenerator-globaltool
```
Konwersja raportu do HTML:
```
reportgenerator -reports:"..\TestResults\572af1f0-10b4-4a1f-9400-bbeeb8802a7f\coverage.cobertura.xml" -targetdir:"coverageReport"
```
