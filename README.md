# BDO-Boss-Timer-Overlay

## Submodules
This program uses a [modified version](https://github.com/Jynsaillar/Overlay.NET) of [Overlay.NET](https://github.com/lolp1/Overlay.NET) by Jacob Kemple.
If you clone this repository, do so with  
`git clone --recurse-submodules https://github.com/Jynsaillar/BDO-Boss-Timer-Overlay.git`.

## NuGet Packages
If you get a missing references error in the Overlay.NET project and the Boss Timer Overlay project, Visual Studio likely did not load the NuGet packages correctly.
You can either try to let Visual Studio restore them by using the NuGet package manager console with  
`nuget restore "Boss Timer Overlay.sln"`  
or reinstall them with  
`Update-Package -reinstall`.
