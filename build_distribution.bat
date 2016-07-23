@echo off

echo Locating MSBuild

set msbuildpath=%ProgramFiles%\MSBuild\14.0\Bin
set libspath=%cd%\Libs
set buildconf=Release

if not -%1-==-- (
	echo Using %1 as building configuration
	set buildconf=%1
)
if -%1-==-- (
	echo No custom build configuration specified. Using Release
)

if not exist %msbuildpath%\msbuild.exe (
	set msbuildpath=%ProgramFiles(x86)%\MSBuild\14.0\Bin
)

if not exist "%msbuildpath%\msbuild.exe" (
	echo Failed to locate MSBuild.exe
	exit /b 1
)

echo PHASE 1: Building Hook and Patch

"%msbuildpath%\msbuild.exe" %cd%\CM3D2.MaidFiddler.Patch\CM3D2.MaidFiddler.Patch.csproj /p:Configuration=%buildconf%

if not %ERRORLEVEL%==0 (
	echo Failed to compile Hook and Patch! Make sure you have all the needed assemblies in the "Libs" folder!
	pause
	exit /b 1
)

echo Building Sybaris Patcher
"%msbuildpath%\msbuild.exe" %cd%\CM3D2.MaidFiddler.Sybaris.Patch\CM3D2.MaidFiddler.Sybaris.Patcher.csproj /p:Configuration=%buildconf%

if not %ERRORLEVEL%==0 (
	echo Failed to compile Hook and Patch! Make sure you have all the needed assemblies in the "Libs" folder!
	pause
	exit /b 1
)

mkdir Build
move /Y %cd%\CM3D2.MaidFiddler.Patch\bin\%buildconf%\CM3D2.MaidFiddler.Hook.dll %cd%\Build\CM3D2.MaidFiddler.Hook.dll
move /Y %cd%\CM3D2.MaidFiddler.Patch\bin\%buildconf%\CM3D2.MaidFiddler.Patch.dll %cd%\Build\CM3D2.MaidFiddler.Patch.dll
move /Y %cd%\CM3D2.MaidFiddler.Sybaris.Patch\bin\%buildconf%\CM3D2.MaidFiddler.Sybaris.Patcher.dll %cd%\Build\CM3D2.MaidFiddler.Sybaris.Patcher.dll

echo -----------------
echo PHASE 1 Complete!
echo -----------------
echo Press any key to build plugin
pause

echo PHASE 2: Building Plugin

"%msbuildpath%\msbuild.exe" %cd%\CM3D2.MaidFiddler.Plugin\CM3D2.MaidFiddler.Plugin.csproj /p:Configuration=%buildconf%

if not %ERRORLEVEL%==0 (
	echo Failed to compile Plugin! Make sure you have patched Assembly-CSharp and copied it into "Libs" folder!
	pause
	exit /b 1
)

move /Y %cd%\CM3D2.MaidFiddler.Plugin\bin\%buildconf%\CM3D2.MaidFiddler.Plugin.dll %cd%\Build\CM3D2.MaidFiddler.Plugin.dll

echo All done!

echo Creating ReiPatcher distribution
mkdir Distribution\ReiPatcher\Managed 2>NUL
mkdir Distribution\ReiPatcher\Patches 2>NUL
mkdir Distribution\ReiPatcher\UnityInjector 2>NUL
mkdir Distribution\ReiPatcher\UnityInjector\Config\MaidFiddler\Translations 2>NUL

copy /Y %cd%\Build\CM3D2.MaidFiddler.Hook.dll %cd%\Distribution\ReiPatcher\Managed\CM3D2.MaidFiddler.Hook.dll >NUL
copy /Y %cd%\Build\CM3D2.MaidFiddler.Patch.dll %cd%\Distribution\ReiPatcher\Patches\CM3D2.MaidFiddler.Patch.dll >NUL
copy /Y %cd%\Build\CM3D2.MaidFiddler.Plugin.dll %cd%\Distribution\ReiPatcher\UnityInjector\CM3D2.MaidFiddler.Plugin.dll >NUL

copy /Y %cd%\Resources\Translations\ENG.txt %cd%\Distribution\ReiPatcher\UnityInjector\Config\MaidFiddler\Translations\ENG.txt >NUL

echo Creating Sybaris distribution
mkdir Distribution\Sybaris\Managed 2>NUL
mkdir Distribution\Sybaris\Loader 2>NUL
mkdir Distribution\Sybaris\UnityInjector 2>NUL
mkdir Distribution\Sybaris\UnityInjector\Config\MaidFiddler\Translations 2>NUL

copy /Y %cd%\Build\CM3D2.MaidFiddler.Hook.dll %cd%\Distribution\Sybaris\Managed\CM3D2.MaidFiddler.Hook.dll >NUL
copy /Y %cd%\Build\CM3D2.MaidFiddler.Sybaris.Patcher.dll %cd%\Distribution\Sybaris\Loader\CM3D2.MaidFiddler.Sybaris.Patcher.dll >NUL
copy /Y %cd%\Build\CM3D2.MaidFiddler.Plugin.dll %cd%\Distribution\Sybaris\UnityInjector\CM3D2.MaidFiddler.Plugin.dll >NUL

copy /Y %cd%\Resources\Translations\ENG.txt %cd%\Distribution\Sybaris\UnityInjector\Config\MaidFiddler\Translations\ENG.txt >NUL

echo Done
pause