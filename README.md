# RastrPVEq <img src="RastrPVEq\Resources\mainicon.ico" width="20">
## Intro
It is Windows desktop app for equivalence solar PV plant models of Russian System Operator of the United Power System (["SO UPS", JSC](https://www.so-ups.ru))

## Build requirments (dependencies)
_COM object:_ 
* Interop.ASTRALib from [RastrWin3 (x64) v 2.8.1.6430 or better](https://www.rastrwin.ru/rastr/)

_SDK:_
* [.NET 6 SDK Build apps](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

_NuGets:_
* [CommunityToolkit.Mvvm v.8.0.0](https://github.com/CommunityToolkit/dotnet)
* [Extended.Wpf.Toolkit v4.5.0](https://github.com/xceedsoftware/wpftoolkit)
* [MaterialDesignThemes v4.6.1](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)

_Other project_
* [DataGridFilter](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit) forked from [macgile / DataGridFilter](https://github.com/macgile/DataGridFilter)

## Manual
1. Make yours .rg2 file power flow model included solar PV plant model by RastrWin3
```
To find out how to make a .rg2 file use:
RastrWin3 -> Помощь -> Справка -> User Manual EN
```
2. Pick head nodes of equivalent

<img src="RastrPVEq\Resources\pick_nodes.png" width="1000">

3. Create equivalence groups and pick branches

<img src="RastrPVEq\Resources\make_groups.png" width="1000">

4. Fix the errors by correcting the actions in 1 or 2 steps

<img src="RastrPVEq\Resources\errors.png" width="1000">

5. Check result

<img src="RastrPVEq\Resources\check_results.png" width="1000">

## Target result example
_Before equivalence Ininskaya 25MW solar PV plant_

<img src="RastrPVEq\Resources\before.png" width="1000">

_After equivalence Ininskaya 25MW solar PV plant_

<img src="RastrPVEq\Resources\after.png" width="1000">