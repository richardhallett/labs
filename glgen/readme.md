glgen
=====

*Not Maintained - May not work with latest OpenGL API spec*

This is a python script that will use the core xml spec files from the opengl registry to generate C# bindings.

* Always up to-date bindings for OpenGL in C#
* Explicitly define what extensions you want when using the script.
* Compile the output straight into your application.

These are fairly raw bindings, where possible it's been attempted to make the bindings safer for C# users, but in general you should know that it is doing native interop under the hood and that means you need to be aware of all the potential pitfalls that come with that. It is especially true for interop if you have specified "--unsafe=True" on the command line which provides unsafe versions of functions, mostly using unsafe pointers.

This is not intended to be a friendly wrapper, there are other excellent libraries such as OpenTK that wrap a lot more of the guts to allow interfacing easier.

Requirements
------------

* Python 3

Usage
-----

By default running glgen.py will create the latest set of bindings for the core profile of GL in a file called GL.cs

You can customise what you want to happen by using various arguments on the command line see: 
	glgen.py --help