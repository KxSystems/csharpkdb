#!/bin/sh
echo "Executing MSBuild DLL begin command..."
dotnet ./tools/sonar/SonarScanner.MSBuild.dll begin /o:"cdoyle-kx" /k:"cdoyle-kx_csharpkdb" /d:sonar.cs.vstest.reportsPaths="**/TestResults/*.trx" /d:sonar.cs.opencover.reportsPaths="*/coverage.opencover.xml" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.verbose=false /d:sonar.login=${SONAR_TOKEN}
echo "Running build..."
dotnet build /p:Configuration=Release ./CSharpKdb.sln
echo "Running tests..."
dotnet test /p:Configuration=Release --no-build ./kx.Test/kx.Test.csproj --logger:trx /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
echo "Executing MSBuild DLL end command..."
dotnet ./tools/sonar/SonarScanner.MSBuild.dll end /d:sonar.login=${SONAR_TOKEN}