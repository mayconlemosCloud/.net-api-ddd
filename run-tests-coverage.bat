@echo off
REM Install tools if not present
dotnet tool install --global coverlet.console
dotnet tool install --global dotnet-reportgenerator-globaltool

REM Clean and build solution
dotnet restore 
dotnet build  

REM Remover relatório de cobertura antigo
rmdir /s /q coverage-report 2>nul

REM Run tests with coverage
dotnet test --collect:"XPlat Code Coverage" --settings coverage.runsettings

REM Gerar relatório excluindo os assemblies IOC, Infrastructure, Program e Application.Mappings.AutoMapperProfile
dotnet tool install -g dotnet-reportgenerator-globaltool >nul 2>&1
reportgenerator -reports:"**/coverage.cobertura.xml" -targetdir:coverage-report -reporttypes:Html -assemblyfilters:-IOC;-Infrastructure;-Program;-Application.Mappings.AutoMapperProfile

echo Coverage report generated in coverage-report\index.html

REM Removing temporary files
rmdir /s /q bin 2>nul
rmdir /s /q obj 2>nul
pause




