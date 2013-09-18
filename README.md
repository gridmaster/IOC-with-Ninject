IOC-with-Ninject
================

Create a simple example of an IOC Container using Ninject. I plan to make several changes to the original code but for now I will start with this. In the mean time this code should work as is.

1) I wan't to the container to work with the service, not the guitar. UPDATE: This change has been implemented.
2) I've removed the Gibson/Fender classes are replaced them with Guitar
3) I've added the name property to the Guitar so guitars can say who they are
4) Added a Guitar constructor that requires a name.
5) Overide ToString in Guitar to return the name.
6) Made changes to Main to match these changes.
7) Added the .WithConstructorArgument("name", "Fender") to the bind module for Ninject.


