# kx.Test

The kx.Test.csproj is a test library that leverages the NUnit framework 
to run unit-tests.

Developers can run the tests either
- via Visual Studio Test Explorer
- via the .\kx.Test\Scripts\GenerateLocalReport.ps1 PowerShell script

## Run via Visual Studio

Instructions for running via Visual Studio can be found at 
https://docs.microsoft.com/en-us/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2019

## Run via PowerShell

1) Running the tests through the PowerShell script not only runs the unit-tests but creates 
a local test coverage report for developers to inspect.

*i)* First ensure that the kx.Test is built
in this example we have built it in Debug

*ii)* Open PowerShell ISE and navigate to the top-level folder of the local repository
Should contain CSharpKdb.sln file and the various project sub-folders

*iii)* Run the following command in PowerShell
```powershell
.\kx.Test\Scripts\GenerateLocalReport.ps1 -TargetAssembly:kx -TargetTestProj:.\kx.Test\kx.Test.csproj -BuildConfiguration:Debug
```

*iv)* Provided the tests complete successfully you should the following output
```powershell
Starting test execution, please wait...

A total of 1 test files matched the specified pattern.

Test Run Successful.
Total tests: 363
     Passed: 363
 Total time: 7.2434 Seconds
 ................
 ```
 
*v)* Additionally if you look in top-level folder of the local repository you should see a local CodeCoverage folder has been created
 
*vi)* If you open the CodeCoverage folder you will find an index.html file. Open this file with Chrome to see the local code coverage report.

2) Also you can use the PowerShell script to run the tests multiple times in succession to confirm the tests are stable
For example to run the tests 15 times simply use a for loop in PowerShell ISE

```powershell
for($i = 0; $i -lt 15; $i++){
    .\kx.Test\Scripts\GenerateLocalReport.ps1 -TargetAssembly:kx -TargetTestProj:.\kx.Test\kx.Test.csproj -BuildConfiguration:Debug
}
```


 
 
