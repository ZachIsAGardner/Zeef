# Zeef.GameManager #

## Features ##
* Handles transitions and persistence betweens scenes
* Holds assets for use in scenes
* Holds information about game state
* Holds other helpful scripts

### GameDB.cs ###
* Holds references to information for use between scenes and play sessions
* A DB Object's information is expected to change

### GameReference.cs ###
* This holds the actual information that is referenced by DB objects
* A Reference Object's information never changes

### Dependencies: ###
* Zeef.Sound
