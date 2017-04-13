Tumonz Portable
================

A program that patches the Tumonz 3.41,3.44 Pro, 4.21, 3D 3.2 and 6.1 Pro binaries so that they can be run from any directory without being previously installed, and tells Tumonz to store all user data in the application directory.
Other versions can be included by arrangement.

Normally Tumonz looks for its application path in the registry, so it knows where to find its stuff.  Because we haven't installed it, this key isnt there.  We modify the exes (see note below) to look in the application itself for the path, which is written there by this program before Tumonz starts (we can't just set the registry key as it requires elevation, which we might not have). It also sets a registry key that tells Tumonz to store all user data in the application directory.  This makes it portable (ie on external hard drive).

Options:
--------
```
-t3     Start Tumonz 3  
-t3pro  Start Tumonz 3 Pro  
-t4     Start Tumonz 4  
-t6pro  Start Tumonz 6 Pro  
-vls    Start License Server  
-v      Don't suppress errors (verbose)  
-sud    Set user data directory to application directory  
```

Notes:
------
Path must be less than 68 chars (the size of the area we write to in the exe).
Your Tumonz binaries must have had previous modifications to remove the installation checks.  This program will NOT do this for you (it's a bit of a legal grey area).
Licensing is NOT circumvented by this program (obviously not legal), you must have some other arrangement for managing roaming licenses.  This program does provide a switch to start the License Server if you have it.
