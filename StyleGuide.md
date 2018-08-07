# Style Guide #

## General ##

* Methods and related are written with the left bracket on the same line as the declaration
```
void Method() {
    if (loveIsReal) {
        // Do something
    }
}
```

* Long methods get a space after declaration, short ones do not
```
void ShortMethod() {
    // Do something
}
void LongMethod() {

    // Do something
    // Do something
    // Do something
    // Do something
    // Do something
    // Do something
    // Do something
    // Do something
    // Do something
    // Do something
}
```

<hr/>

* Namespaces and classes get a return after declaration
```
namespace SomeNamespace {

    public class SomeClass {

        // Class stuff
    }
}
```

* Namespaces are seperated between standard c# and unity unity namespaces, and project and framework namespaces
```
using System;
using UnityEngine;
// ---
using Zeef;
using JacketGame.Fights;

```

<hr/>

* This is how you deal with fields that are to be set in the inspector that also need to get referenced by other objects.
```
[SerializeField] private GameObject player;
public GameObject Player { get { return player; }}
```

## Naming Conventions ##

* Public fields and properties are capitalized, private and protected are lowercase

<hr/>

* All enums end with Enum
```
public enum SomeEnum {

}
```

<hr/>

* Partial scenes are prepended with an '_'

<hr/>

* methods that return an IEnumerator are appended with "Coroutine"
```
public IEnumerator DoSomethingCoroutine() {
    // yield and Do something
}
```

<hr/>

* Classes that end with "Data" are used by MonoBehaviours, usually to fill in properties
    * These classes are usually seeded once and stored in a list for later reference
    * These classes can be stored in a .dat file for persistance between play sessions
* Classes that end with "Model" are only used to fill another class's properties
    * These classes are only created at runtime and discared quickly
* Classes that end with "Info" carry data that is used for other actions, but isn't necessarily persisted anywhere
    * These classes are only created at runtime and discared quickly

<hr/>

* Methods named "Execute" are written in MonoBehaviours. Execute is called on MonoBehaviours that already exist in the scene.
    * Execute will often take a model as an argumement with which it will fill in properties on the MonoBehaviour and do other actions.
    * (This is pretty much just the Start method, but with arguments)

* Methods named "Initialize" are written in MonoBehaviours. Initialize will instantiate a GameObject and will fill in properties on the MonoBehaviour attached to the GameObject. Initialize can either...
    * Create an empty GameObject for you and attach itself to that GameObject or...
    * Require a prefab in which it will find itself attached to.
    * (This is pretty much just the Instantiate method, but with arguments)

* The main difference between "Execute" and "Initialize" is that one already exists and the other doesn't