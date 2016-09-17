@echo off
rem Basic build script for Maid Fiddler
rem Requires MSBuild with C# 6 compatible compiler
rem NOTE: PLACE THE NEEDED ASSEMBLIES INTO "Libs" FOLDER
rem You may specify the build configuration as an argument to this batch
rem If no arguments are specified, will build the Release version

echo Locating MSBuild

set "msbuildpath=%ProgramFiles%\MSBuild\14.0\Bin"
set libspath=%cd%\Libs
set buildconf=Release
set buildplat=AnyCPU

if not -%1-==-- (
	echo Using %1 as building configuration
	set buildconf=%1
)
if -%1-==-- (
	echo No custom build configuration specified. Using Release
)

if not -%2-==-- (
	echo Using %2 as building platform
	set buildplat=%2
)
if -%2-==-- (
	echo No custom platform specified. Using AnyCPU
)

if not exist "%msbuildpath%\msbuild.exe" (
	set "msbuildpath=%ProgramFiles(x86)%\MSBuild\14.0\Bin"
)

if not exist "%msbuildpath%\msbuild.exe" (
	echo Failed to locate MSBuild.exe
	exit /b 1
)

echo PHASE 1: Building Hook and Patch

"%msbuildpath%\msbuild.exe" "%cd%\CM3D2.MaidFiddler.Patch\CM3D2.MaidFiddler.Patch.csproj" /p:Configuration=%buildconf%,Platform=%buildplat%

if not %ERRORLEVEL%==0 (
	echo Failed to compile Hook and Patch! Make sure you have all the needed assemblies in the "Libs" folder!
	pause
	exit /b 1
)

echo Building Sybaris Patcher
"%msbuildpath%\msbuild.exe" "%cd%\CM3D2.MaidFiddler.Sybaris.Patch\CM3D2.MaidFiddler.Sybaris.Patcher.csproj" /p:Configuration=%buildconf%,Platform=%buildplat%

if not %ERRORLEVEL%==0 (
	echo Failed to compile Hook and Patch! Make sure you have all the needed assemblies in the "Libs" folder!
	pause
	exit /b 1
)

mkdir Build
move /Y "%cd%\CM3D2.MaidFiddler.Patch\bin\%buildconf%\CM3D2.MaidFiddler.Hook.dll" "%cd%\Build\CM3D2.MaidFiddler.Hook.dll"
move /Y "%cd%\CM3D2.MaidFiddler.Patch\bin\%buildconf%\CM3D2.MaidFiddler.Patch.dll" "%cd%\Build\CM3D2.MaidFiddler.Patch.dll"
move /Y "%cd%\CM3D2.MaidFiddler.Sybaris.Patch\bin\%buildconf%\CM3D2.MaidFiddler.Sybaris.Patcher.dll" "%cd%\Build\CM3D2.MaidFiddler.Sybaris.Patcher.dll"

echo -----------------
echo PHASE 1 Complete!
echo -----------------
echo.
echo Now do the following:
echo.
echo 1. Move %cd%\Build\CM3D2.MaidFiddler.Hook.dll to "<CM3D2 Root Directory>\CM3D2(x86/x64)_Data\Managed".
echo 2. Move %cd%\Build\CM3D2.MaidFiddler.Patch.dll to "<ReiPatcher Install Directory>\<Patches Directory>".
echo 3. Run ReiPatcher to patch CM3D2.
echo 4. Copy Assembly-CSharp.dll from "<CM3D2 Root Directory>\CM3D2(x86/x64)_Data\Managed to %cd%\Libs".
echo.
echo You may skip these steps if you already have a patched Assembly-CSharp in the Libs folder.
echo Press any key when you have performed the steps above.
pause

echo PHASE 2: Building Plugin

"%msbuildpath%\msbuild.exe" "%cd%\CM3D2.MaidFiddler.Plugin\CM3D2.MaidFiddler.Plugin.csproj" /p:Configuration=%buildconf%,Platform=%buildplat%

if not %ERRORLEVEL%==0 (
	echo Failed to compile Plugin! Make sure you have patched Assembly-CSharp and copied it into "Libs" folder!
	pause
	exit /b 1
)

move /Y "%cd%\CM3D2.MaidFiddler.Plugin\bin\%buildconf%\CM3D2.MaidFiddler.Plugin.dll" "%cd%\Build\CM3D2.MaidFiddler.Plugin.dll"

echo All done!
pause