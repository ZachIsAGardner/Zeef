# Style Guide #

* Methods and related are written with the left bracket on the same line as the declaration
```
void Method() {
    // Do something
}
```

* Namespaces and classes get a space after declaration
```
namespace Namespace {

    public class Class {

        // Class stuff
    }
}
```

* Namespaces are seperated between standard c# and unity namespaces, and project and framework namespaces
```
using System;
using UnityEngine;
// ---
using Zeef;
using JacketGame.Fights;

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

* Public fields and properties are capitalized, private and protected are lowercase

* This is how you deal with fields that are to be set in the inspector that also need to get referenced by other objects.
```
[SerializeField] private GameObject player;
public GameObject Player { get { return player; }}
```

* All enums end with Enum
* Fight me
```
public enum SomeEnum {

}
```

* Partial scenes are prepended with an '_'

* methods that return an IEnumerator are appended with "Coroutine"
```
public IEnumerator DoSomethingCoroutine() {
    // yield and Do something
}
```

* Methods named "Execute" are placed on existing monobehaviours that when called will perform some action

* Methods named "Initialize" are placed on monobehaviours that are attached to gameobjects. Initilialize is called to return that gamobject filled with data passed in through arguments given to the "Initialize" method. Initiliaze can either create an empty gameobject for you and attach itself to it or you would also pass it a prefab that it will find itself on and fill the info.

* The main difference between "Execute" and "Initialize" is that one already exists and the other doesn't