#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
#

param(
    [string]$Configuration="Debug",
    [string]$Architecture="x64",
    # This is here just to eat away this parameter because CI still passes this in.
    [string]$Targets="Default",
    [switch]$NoPackage,
    [switch]$NoBuild,
    [switch]$Help,
    [Parameter(Position=0, ValueFromRemainingArguments=$true)]
    $ExtraParameters)

if($Help)
{
    Write-Output "Usage: .\run-build.ps1 [-Configuration <CONFIGURATION>] [-Architecture <ARCHITECTURE>] [-NoPackage] [-NoBuild] [-Help]"
    Write-Output ""
    Write-Output "Options:"
    Write-Output "  -Configuration <CONFIGURATION>     Build the specified Configuration (Debug or Release, default: Debug)"
    Write-Output "  -Architecture <ARCHITECTURE>       Build the specified architecture (x64, x86, arm, or arm64 , default: x64)"
    Write-Output "  -NoPackage                         Skip packaging targets"
    Write-Output "  -NoBuild                           Skip building the product"
    Write-Output "  -Help                              Display this help message"
    exit 0
}

# The first 'pass' call to "dotnet msbuild build.proj" has a hard-coded "WriteDynamicPropsToStaticPropsFiles" target
#    therefore, this call should not have other targets defined. Remove all targets passed in as 'extra parameters'.
if ($ExtraParameters)
{
    $ExtraParametersNoTargets = $ExtraParameters.GetRange(0,$ExtraParameters.Count)
    foreach ($param in $ExtraParameters)
    {
        if(($param.StartsWith("/t:", [StringComparison]::OrdinalIgnoreCase)) -or ($param.StartsWith("/target:", [StringComparison]::OrdinalIgnoreCase)))
        {
            $ExtraParametersNoTargets.Remove("$param") | Out-Null
        }
    }
}

$env:CONFIGURATION = $Configuration;
$RepoRoot = "$PSScriptRoot"
if(!$env:NUGET_PACKAGES){
  $env:NUGET_PACKAGES = "$RepoRoot\.nuget\packages"
}

if($NoPackage)
{
    $env:DOTNET_BUILD_SKIP_PACKAGING=1
}
else
{
    $env:DOTNET_BUILD_SKIP_PACKAGING=0
}

# Use a repo-local install directory for stage0 (but not the artifacts directory because that gets cleaned a lot
if (!$env:DOTNET_INSTALL_DIR)
{
    $env:DOTNET_INSTALL_DIR="$RepoRoot\.dotnet_stage0\$Architecture"
}

if (!(Test-Path $env:DOTNET_INSTALL_DIR))
{
    mkdir $env:DOTNET_INSTALL_DIR | Out-Null
}

# Disable first run since we want to control all package sources
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1

# Don't resolve shared frameworks from user or global locations
$env:DOTNET_MULTILEVEL_LOOKUP=0

# Turn off MSBuild Node re-use
$env:MSBUILDDISABLENODEREUSE=1

# Enable vs test console logging
$env:VSTEST_BUILD_TRACE=1
$env:VSTEST_TRACE_BUILD=1

# install a stage0
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$dotnetInstallPath = Join-Path $env:DOTNET_INSTALL_DIR "dotnet-install.ps1"
Invoke-WebRequest -Uri "https://dot.net/v1/dotnet-install.ps1" -OutFile "$dotnetInstallPath"

$InstallArchitecture = $Architecture
if($Architecture.StartsWith("arm", [StringComparison]::OrdinalIgnoreCase))
{
    $InstallArchitecture = "x64"
}

Write-Output "$dotnetInstallPath -version ""3.0.100-alpha1-009452"" -InstallDir $env:DOTNET_INSTALL_DIR -Architecture ""$InstallArchitecture"""
Invoke-Expression "$dotnetInstallPath -version ""3.0.100-alpha1-009452"" -InstallDir $env:DOTNET_INSTALL_DIR -Architecture ""$InstallArchitecture"""

if ($LastExitCode -ne 0)
{
    Copy-Item -Recurse -Force $env:DOTNET_TOOL_DIR $env:DOTNET_INSTALL_DIR
}

# Put the stage0 on the path
$env:PATH = "$env:DOTNET_INSTALL_DIR;$env:PATH"

if ($NoBuild)
{
    Write-Output "Not building due to --nobuild"
    Write-Output "Command that would be run: 'dotnet msbuild build.proj /m /p:Architecture=$Architecture $ExtraParameters'"
}
else
{
    dotnet msbuild build.proj /p:Architecture=$Architecture /p:GeneratePropsFile=true /t:WriteDynamicPropsToStaticPropsFiles $ExtraParametersNoTargets
    dotnet msbuild build.proj /m /v:normal /fl /flp:v=diag /p:Architecture=$Architecture $ExtraParameters
    if($LASTEXITCODE -ne 0) { throw "Failed to build" } 
}
