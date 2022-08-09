# MF3DSharp


MF3DSharp is a small mathematics library written in C# that contains basic mathematical structures, functions and methods useful for 3D computing. This library is prior made to be compatible with many versions of C#, .NET.

MF3DSharp is not the fastest or well optimized library for 3D computation.If what your are searching for is a fast and reliable library for 3D computing, check latest version of [OpenTK project](https://github.com/opentk/opentk). You can also have look to [geometry3sharp project](https://github.com/gradientspace/geometry3Sharp) which can have additionals features that you need.


### Requirement(s)

* .NET Framework 3.5+ / .NET Standard 2.0+


### Usage

* Download latest build from release section
* Un-zip release file on your system
* Import MF3D.dll corresponding to your framework version in your project as a dependency


### Build from source

`Be sure .NET SDK that correspond to your target platform is installed on your system before building the library`

* Clone the main branch of this repository on your system
* Open the `.sln` file with Visual Studio (2019 or above)
* Select your target platform and build the solution


## What are included?

### Structs

* **Vector2d, Vector3d, Vector4d, Vector2f, Vector3f, Vector4f**
    * Vector structs including basic operation, dot and cross product, length, normalization and distance

* **Matrix2d, Matrix3d, Matrix4d, Matrix2f, Matrix3f, Matrix4f**
    * Matrix structs including basic operation, identity and zero matrix, determinant, normalization tranpose and inverse
    * Axis angle to Matrix3d or Matrix3f conversion
    * Translation, rotation and scale Matrix4d/4f computing and extraction

* **Quaterniond, Quaternionf**
    * Quaternion structs including basic operation, identity and zero quaternion, length, inverse, dot product, normalization, conjugate, rotation matrix conversion, euler conversion, axis angle, slerp

### Classes

* **Transformd, Transformf**
    * A useful class to describe and manipulate a 3D object in space using a Vector3 for the position and scale and a Quaternion for rotation.
    * Transform class can have childs which will inherit of parent position
    * It include basic transformations (translate, rotate and scale) and conversion to Matrix4
    * Implement cloneable interface (clone will clone the instance and all attached childs)

**All structs and classes are in MF3D namespace and they are serializables by default.**


## License

MIT License