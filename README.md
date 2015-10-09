# CM3D2.MaidFiddler
Maid Fiddler is a plug-in for CM3D2 which functions as a real-time game editor.
The tool uses in-game API, ReiPatcher and UnityInjector to hook itself into game's logic.

## Requirements
To sucessfully build Maid Fiddler from source, one neeeds the following libraries:
* ReiPatcher.exe  (Found in ReiPatcher)
* UnityInjector.dll (Found in UnityInjector)
* Mono.Cecil.dll  (Found in ReiPatcher or from its project on GitHub)
* Mono.Cecil.Inject.dll (Found in its own GitHub project)
* ExIni.exe (Found in ReiPatcher)
* Assembly-CSharp.dll (Found in CM3D2's own assemblies)

## Structure of the plug-in
### CM3D2.MaidFiddler.Hooks
`CM3D2.MaidFiddler.Hooks` acts as a birdge between the game's native API and Maid Fiddler. Maid Fiddler, and other plug-ins,
may use it to link their logic to in-game events and method calls.

### CM3D2.MaidFiddler.Patch
`CM3D2.MaidFiddler.Patch` contains the code that links hook methods specified in `CM3D2.MaidFiddler.Hooks` with in-game method calls.

### CM3D2.MaidFiddler.Plugin
This is the core of Maid Fiddler. It contains the GUI, the plug-in and all its logic
