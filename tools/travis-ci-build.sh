#!/bin/sh
# Only Run SonarScanner if it is not a pull_request
if [ "$TRAVIS_PULL_REQUEST" = "false" ]; then
	echo "Executing MSBuild DLL begin command..."
	dotnet ./tools/sonar/SonarScanner.MSBuild.dll begin /o:"kxsystems" /k:"KxSystems_csharpkdb" /d:sonar.cs.vstest.reportsPaths="**/TestResults/*.trx" /d:sonar.cs.opencover.reportsPaths="*/coverage.opencover.xml" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.verbose=false /d:sonar.login=${SONAR_TOKEN}
fi

# Run Release build
echo "Running build..."
dotnet build /p:Configuration=Release ./CSharpKdb.sln

# Run Tests, Coverlet to record result and code-coverage
echo "Running tests..."
dotnet test /p:Configuration=Release --no-build ./kx.Test/kx.Test.csproj --logger:trx /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

if ["$TRAVIS_PULL_REQUEST" = "false"]; then
	echo "Executing MSBuild DLL end command..."
	dotnet ./tools/sonar/SonarScanner.MSBuild.dll end /d:sonar.login=${SONAR_TOKEN}
fi