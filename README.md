# Zeef #
Zachary's Framework (Zeef) is a C# game framework built for Unity.

#### Current Unity Version ####
Unity 2018.3.1f1

## Setup ##
This project requires minimal setup to get working.

###### Step 1: Scripting Runtime Version
* This project requires a higher version of C# that Unity regularly uses. To enable the higher version of C# you must change the <strong>Scripting Runtime Version</strong> setting from <strong>.Net 3.5 Equivalent</strong> to <strong>.Net 4.x Equivalent</strong>
  * This setting is found at Edit => Project Settings => Player => Other Settings => Configuration => Scripting Runtime Version

  ![ScriptingRuntimeVersion](https://github.com/ZachIsAGardner/Zeef/blob/master/Documents/runtime.png)

###### Step 2: AsyncAwaitUtil Extension
* This project also relies on the <strong>AsyncAwaitUtil</strong> extension which can found here: https://github.com/svermeulen/Unity3dAsyncAwaitUtil/releases
  * Make sure this is in the Plugins folder within the Assets folder along with Zeef

  ![Plugins](https://github.com/ZachIsAGardner/Zeef/blob/master/Documents/async.png)

## Namespace Navigation ##

* [General](https://github.com/ZachIsAGardner/Zeef/tree/master/General)
* [GameManagement](https://github.com/ZachIsAGardner/Zeef/tree/master/GameManagement)
* [Menu](https://github.com/ZachIsAGardner/Zeef/tree/master/Menu)
* [Perform](https://github.com/ZachIsAGardner/Zeef/tree/master/Perform)
* [Persistence](https://github.com/ZachIsAGardner/Zeef/tree/master/Persistence)
* [Sound](https://github.com/ZachIsAGardner/Zeef/tree/master/Sound)
* [TwoDimensional](https://github.com/ZachIsAGardner/Zeef/tree/master/TwoDimensional)
