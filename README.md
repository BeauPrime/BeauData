# BeauData

**Current Version: 0.2.0**  
Updated 13 September 2019 | Changelog (Not yet available)

## About
BeauData is an object serialization and deserialization library, intended to reduce the amount of custom logic necessary to save and load objects. It supports the basic object versioning, serialization and deserialization of subclasses, and type aliasing. It can read and write to XML, JSON, binary, and gzipped binary.

### Table of Contents
1. [Usage](#usage)
    * [Installing BeauData](#installing-beaudata)
    * [Setting Up A Serialized Object](#setting-up-a-serialized-object)
    * [Writing the Object](#writing-the-object)
    * [Reading the Object](#reading-the-object)
    * [Versioning](#versioning) (coming soon)
    * [Subclasses](#subclasses)
    * [Custom Serializers](#custom-serializers) (coming soon)
2. [Reference](#reference)
	* [Formats](#formats)
	* [Built-In Serializable Types](#built-in-serializable-types)
	* [Serializer Properties](#serializer-properties)
	* [Serializer Methods](#serializer-methods)
	* [Read/Write Methods](#readwrite-methods)
	* [Misc Methods](#misc-methods)
----------------

## Usage

### Installing BeauData

Download [BeauData.unitypackage](https://github.com/FilamentGames/BeauData/raw/master/BeauData.unitypackage) from the repo. Unpack it into your project.

BeauData uses the ``BeauData`` namespace. You'll need to add the statement ``using BeauData;`` to the top of any scripts using it.

### Setting Up a Serialized Object
Serialized objects must implement the `ISerializedObject` interface.

```csharp
private class TestClassA : ISerializedObject
{
    public Guid someId;
    public float someFloatData;
    public TestClassB child;
    
    public Vector2 vec2Prop { get; set; }

	// This is the only method you are required to implement.
    // It's called for both serialization and deserialization,
    // so you don't need to add/remove/reorder fields
    // in two separate methods.
    public void Serialize(Serializer ioSerializer)
    {
    	// The general format for serialization methods is
        // Serialize(propertyKey, ref field, [default value (if supported by type)], [extra options])
        
        ioSerializer.Serialize("someId", ref someId);
        ioSerializer.Serialize("someFloatData", ref someFloatData);
        
        // You can even nest serialized objects inside of this one.
        // Note that methods for serializing objects start with Object instead of Serialize
        ioSerializer.Object("child", ref child);
        
        // Serializing C# properties instead of fields requires some extra work,
        // since you can't pass a property in by reference.
        // Because of this limitation, it's recommended you use fields instead of properties.
        if (ioSerializer.IsWriting)
        {
			Vector2 vec2PropCopy = vec2Prop;
            ioSerializer.Serialize("vec2Prop", ref vec2PropCopy);
		}
        else
        {
        	Vector2 vec2PropDest = default(Vector2);
            ioSerializer.Serialize("vec2Prop", ref vec2PropDest);
            vec2Prop = vec2PropDest;
        }
    }
}

private class TestClassB : ISerializedObject
{
	[...]
}
```

Most field types also allow you to specify a default value. When writing, if the default value matches the provided value, the field will be omitted. When reading, if a field is omitted but a default value is provided, that value will be used instead.

You can also specify a field as optional by providing ``FieldOptions.Optional``. When writing, if the provided value is null, the field will be omitted. When reading, if the field is omitted but marked as optional, a null value will be used instead.

For the XML format, if you provide ``FieldOptions.PreferAttribute``, certain field types can be written as XML attributes instead of child elements.

### Writing the Object
To write your ``ISerializedObject``, call ``Serializer.Write``.

```csharp
TestClassA testClassA = new TestClassA( ... );

// You can serialize to a string
string serializedObject = Serializer.Write(testClassA, OutputOptions.None, Serializer.Format.JSON);

// You can also serialize to a stream
Stream stream = [...];
Serializer.Write(testClassA, stream, OutputOptions.None, Serializer.Format.Binary);
```

### Reading the Object
To read your ``ISerializedObject`` back into memory, call ``Serializer.Read``.

```csharp

// You can read from a string
string source = [...];
TestClassA deserializedFromString = Serializer.Read<TestClassA>(source);

// You can also read from a stream.
// Note that this is currently limited to streams containing only an ISerializedObject
Stream stream = [...];
TestClassA deserializedFromStream = Serializer.Read<TestClassA>(stream);
```

### Versioning

[documentation coming soon]

### Subclasses

Subclasses of an ``ISerializedObject`` object can be serialized and deserialized. When writing, an object of a different type than the expected type will write out a ``__type`` property, which contains the assembly-qualified name of the type to construct when reading the object back into memory.

The assembly-qualified name can be quite lengthy. If you'd like the ``__type`` field to be shorter, you can also register a type alias. A type with an alias will write/read the alias instead of the assembly-qualified name.

```csharp
TypeUtility.RegisterAlias<TestClassA>("TestClassA");
TypeUtility.RegisterAlias<TestClassASubclass>("TestClassASubclass");
```

### Custom Serializers

[documentation coming soon]

## Reference

### Formats

The following export types are supported.

| **Type** | **Notes** |
| ------- | --------- |
| JSON |  |
| XML | Supports serializing fields as XML attributes instead of nodes. |
| Binary | Custom binary format. |
| Gzip | Compressed form of Binary |

### Built-In Serializable Types

The following types are able to be serialized.

| **Type** | **Category** | **Function Prefix** | **Default Values?**| **XML Attribute?** | **Notes** |
| -------- | --------- | ---------- | ------------ | -------| --- |
| ``Boolean`` | Value | ``Serialize`` | ✓ | ✓ | |
| ``Byte`` | Value | ``Serialize`` | ✓ | ✓ | |
| ``Byte[]`` | Value | ``Serialize`` | X | X | |
| ``Double`` | Value | ``Serialize`` | ✓ | ✓ | |
| ``System.Guid`` | Value | ``Serialize`` | ✓ | ✓ | |
| ``Int16`` | Value | ``Serialize`` | ✓ | ✓ | | 
| ``Int32`` | Value | ``Serialize`` | ✓ | ✓ | |
| ``Int64`` | Value | ``Serialize`` | ✓ | ✓ | |
| ``Single`` | Value | ``Serialize`` | ✓ | ✓ | | 
| ``String`` | Value | ``Serialize`` | ✓ | ✓ | |
| ``UInt16`` | Value | ``Serialize`` | ✓ | ✓ | |
| ``UInt32`` | Value | ``Serialize`` | ✓ | ✓ | |
| ``UInt64`` | Value | ``Serialize`` | ✓ | ✓ | |
| ``Enum`` | Enum | ``Enum`` | ✓ | ✓ | Binary formats respect the enum's backing type (i.e. an Int32 enum is written as Int32, a Byte enum is written as Byte, etc.). Changing an enum's backing type will cause issues when reading binary formats. |
| ``BeauData.FourCC`` | Value | ``Serialize`` | ✓ | ✓ | |
| ``BeauData.ISerializedObject`` | Object | ``Object`` | X | X | |
| ``UnityEngine.Bounds`` | Value | ``Serialize`` | ✓ | X | | 
| ``UnityEngine.BoundsInt`` | Value | ``Serialize`` | ✓ | X | |
| ``UnityEngine.Color`` | Value | ``Serialize`` | ✓ | X | |
| ``UnityEngine.Quaternion`` | Value | ``Serialize`` | ✓ | X | |
| ``UnityEngine.Rect`` | Value | ``Serialize`` | ✓ | X | |
| ``UnityEngine.RectInt`` | Value | ``Serialize`` | ✓ | X | |
| ``UnityEngine.Texture2D`` | Value | ``Serialize`` | X | ✓ | Must specify PNG or JPG export format when writing. |
| ``UnityEngine.Vector2`` | Value | ``Serialize`` | ✓ | X | |
| ``UnityEngine.Vector2Int`` | Value | ``Serialize`` | ✓ | X | |
| ``UnityEngine.Vector3`` | Value | ``Serialize`` | ✓ | X | |
| ``UnityEngine.Vector3Int`` | Value | ``Serialize`` | ✓ | X | |
| ``UnityEngine.Vector4`` | Value | ``Serialize`` | ✓ | X | |

Additionally, collections of all of these types are able to be serialized.

```csharp
// where T is the serialized type
T[]
List<T>
HashSet<T>
Dictionary<int, T>
Dictionary<string, T>
```

### Serializer Properties

| **Property** | **Description** |
| ------------ | -------------- |
| ``IsReading`` | Indicates if the Serializer is in read mode. |
| ``IsWriting`` | Indicates if the Serializer is in write mode. |
| ``ObjectVersion`` | Version number for the most recent ``ISerializedObject``. |
| ``HasErrors`` | Indicates if the Serializer has encountered errors. |
| ``Errors`` | Debug string representing all errors the Serializer has encountered. |


### Serializer Methods

| **Function** | **Description** |
| ------------ | -------------- |
| ``Serialize`` | Reads/writes a key-value pair with a value. |
| ``Array`` | Reads/writes a key-value pair with an array or list of values. |
| ``Set`` | Reads/writes a key-value pair with a set of values. |
| ``Map`` | Reads/writes a key-value pair with a dictionary of values. |
| ``Enum`` | Reads/writes a key-value pair with an enum. |
| ``EnumArray`` | Reads/writes a key-value pair with an array or list of enums. |
| ``EnumSet`` | Reads/writes a key-value pair with a set of enums. |
| ``EnumMap`` | Reads/writes a key-value pair with a dictionary of enums. |
| ``Object`` | Reads/writes a key-value pair with an ``ISerializedObject`` |
| ``ObjectArray`` | Reads/writes a key-value pair with an array or list of ``ISerializedObject``. |
| ``ObjectSet`` | Reads/writes a key-value pair with a set of ``ISerializedObject``. |
| ``ObjectMap`` | Reads/writes a key-value pair with a dictionary of ``ISerializedObject``. |
| ``Custom`` | Reads/writes a key-value pair for a type with a custom serializer. |
| ``CustomArray`` | Reads/writes a key-value pair with an array or list of a type with a custom serializer.. |
| ``CustomSet`` | Reads/writes a key-value pair with a set of a type with a custom serializer.. |
| ``CustomMap`` | Reads/writes a key-value pair with a dictionary of a type with a custom serializer.. |
| ``BeginGroup`` | Creates (if writing) or enters (if reading) a named subgroup. Useful for grouping properties without creating subobjects. |
| ``EndGroup`` | Ends (if writing) or exits (if reading) a named subgroup. |

### Read/Write Methods

| **Function** | **Description** |
| ------------ | -------------- |
| ``Read`` (``UnityEngine.TextAsset``) | Reads an ``ISerializedObject`` from a ``TextAsset``. |
| ``Read`` (``UnityEngine.WWW``) | Reads an ``ISerializedObject`` from a ``WWW``. |
| ``Read`` (``UnityEngine.Networking.UnityWebRequest``) | Reads an ``ISerializedObject`` from a ``UnityWebRequest``. |
| ``Read`` (``String``) | Reads an ``ISerializedObject`` from a ``String``. |
| ``Read`` (``System.IO.Stream``) | Reads an ``ISerializedObject`` from a ``String``. |
| ``Read`` (``Byte[]``) | Reads an ``ISerializedObject`` from a ``Byte[]``. |
| ``ReadFile``| Reads an ``ISerializedObject`` from an external file. |
| ``ReadPrefs`` | Reads an ``ISerializedObject`` from ``PlayerPrefs``. |
| ``Write`` | Writes an ``ISerializedObject`` to a ``String`` |
| ``Write`` (``System.IO.Stream``) | Writes an ``ISerializedObject`` to a ``Stream`` |
| ``WriteFile`` | Writes an ``ISerializedObject`` to an external file. |
| ``WritePrefs`` | Writes an ``ISerializedObject`` to ``PlayerPrefs``. |

### Misc Methods

| **Function** | **Description** |
| ------------ | -------------- |
| ``TypeUtility.RegisterAlias`` | Registers an alias for the given type. Useful if relying on subclass serialization. |
| ``TypeUtility.RegisterSerializer`` | Registers a custom serialization function for the given type. |
| ``PathUtility.CorrectPath`` | Will append the correct extension for the given format to a file path, if an extension is not already present. |
| ``PathUtility.StreamingAssetsPath`` | Returns the given path modified to be relative to the StreamingAssets path. |
| ``PathUtility.PersistentDataPath`` | Returns the given path modified to be relative to the PersistentData path. |
| ``PathUtility.DataPath`` | Returns the given path modified to be relative to the Data path. |
| ``PathUtility.TempCachePath`` | Returns the given path modified to be relative to the TemporaryCache path. |
| ``PathUtility.PathToURL`` | Returns the given path modified to be a valid URL for a ``WWW`` or ``UnityWebRequest``. | 