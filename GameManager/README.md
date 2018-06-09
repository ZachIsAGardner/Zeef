# ZeeGame
## Features
* Handles transitions and persistance betweens scenes
* Holds assets for use in scenes
* Holds information about game state
* Holds other helpful scripts

### GameDB.cs
* Holds references to information for use between scenes and play sessions
* A DB Object's information is expected to change

### GameReference.cs
* This holds the actual information that is referenced by DB objects
* A Reference Object's information never changes

## Info
* Other ZeeFramework components are expected to be attached to the ZeeGame singleton object
#### Dependencies:
* ZeeSound
