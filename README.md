# MF3D Sharp

MF3D Sharp is an open source (MIT License) C# library for basic 3D calculations compatible with many C#/.Net version including .NET Framework 3.5+, .NET Standard 2.0+ and Unity.

This is not the fastest or well optimized library in this domain, if you don't have limitation on your C# version check latest version of [OpenTK](https://github.com/opentk/opentk).
This is not the most complete library either, have look to [geometry3Sharp project](https://github.com/gradientspace/geometry3Sharp) which include lot of additionals structs and classes.


### Requirement(s)

* .NET Framework 3.5+ or .NET Standard 2.0+


### Usage

* Download latest build from release section
* Un-zip release file on your system
* Import MF3D.dll corresponding to your framework version in your project as a dependency


### Build from source

* Be sure .NET SDK that correspond to your target platform is installed on your system before building the library
* Clone the main branch of this repository on your system
* Open the `.sln` file with Visual Studio (2019 or above)
* Select your target platform and build the solution


## What are included?

### Structs

* Vector2d, Vector3d, Vector4d, Vector2f, Vector3f, Vector4f
    * Vector structs including basic operation, dot and cross product, length, normalization and distance calculation

* Matrix2d, Matrix3d, Matrix4d, Matrix2f, Matrix3f, Matrix4f
    * Matrix structs including basic operation, identity and zero matrix, determinant, normalization tranpose and inverse calculation
    * Axis angle to Matrix3d or Matrix3f conversion
    * Translation, rotation and scale Matrix4d/4f calculation and extraction

* Quaterniond, Quaternionf
    * Quaternion structs including basic operation, identity and zero quaternion, length, inverse, dot product, normalization, conjugate, rotation matrix conversion, euler conversion, axis angle, slerp

### Class

* Transformd, Transformf
    * A usefull class to describe and manipulate a 3D object in space using a Vector3 for the position and scale and a Quaternion for rotation.
    * Transform class can have childs which will inherit of parent position
    * It include basic transformations (translate, rotate and scale) and conversion to Matrix4
    * Implement cloneable interface (clone will clone the instance and all attached childs)

All structs and classes are in MF3D namespace and they are serializables by default.


## License

MIT License