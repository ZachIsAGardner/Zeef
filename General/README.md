# Zeef #

## Features ##
* Basic art assets
* Helpful utility scripts

#### SingleInstance.cs ####
* A MonoBehaviour inherits from SingleInstance.cs to ensure that only one if can ever exist.

#### ControlManager.cs ####
* Handles player input
* Contains strongly typed references to different types of player input.
* Eliminates referencing inputs with magic strings.

#### ValidationManager.cs ####
* The goal of the ValidationManager is to point out potential issues before they come up in play.
* This will parse a Scene and deem if it is "valid" or not.
    * A Scene is invalid when any of the GameObjects contained within have fields decorated with the [Required] attribute that have not yet been set in the inspector.

