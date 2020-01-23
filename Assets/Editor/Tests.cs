using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BeauData.Format;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace BeauData.Editor
{
    static public class Tests
    {
        static private readonly Guid CONST_GUID = new Guid("c480fe9e-9116-45ca-a85b-623332bf0a33");

        static private TestContext TestContext;

        #region Object Types

        private class TestClassA : ISerializedObject
        {
            public Guid someId;
            public float someFloatData;

            public virtual void Serialize(Serializer ioSerializer)
            {
                ioSerializer.Serialize("someId", ref someId);
                ioSerializer.Serialize("someFloatData", ref someFloatData);
            }
        }

        private class TestClassB : TestClassA
        {
            public string someStringData;
            public Vector2 someVector;

            public override void Serialize(Serializer ioSerializer)
            {
                base.Serialize(ioSerializer);

                ioSerializer.Serialize("someStringData", ref someStringData);
                ioSerializer.Serialize("someVector", ref someVector);
            }
        }

        private class CustomSerializerWrapper : ISerializedObject
        {
            public CustomSerializerTest Wrapped;

            void ISerializedObject.Serialize(Serializer ioSerializer)
            {
                ioSerializer.Custom("wrapped", ref Wrapped);
            }
        }

        private class CustomSerializerTest
        {
            public Vector2 SomeVal;
            public Vector2Int SomeVal2;
        }

        static private void CustomSerializerTestSerializer(ref CustomSerializerTest inTest, Serializer inSerializer)
        {
            inSerializer.Serialize("someVal", ref inTest.SomeVal);
            inSerializer.Serialize("someVal2", ref inTest.SomeVal2);
        }

        private class AssetRefTest : ISerializedObject
        {
            public Sprite SomeVal;

            void ISerializedObject.Serialize(Serializer ioSerializer)
            {
                ioSerializer.AssetRef("someVal", ref SomeVal);
            }
        }

        private struct TestSerializedStruct : ISerializedObject
        {
            public int Value;

            public TestSerializedStruct(int value)
            {
                Value = value;
            }

            void ISerializedObject.Serialize(Serializer ioSerializer)
            {
                ioSerializer.Serialize("value", ref Value);
            }
        }

        private class TestSerializedStructWrapper : ISerializedObject
        {
            public TestSerializedStruct StructValue;

            void ISerializedObject.Serialize(Serializer ioSerializer)
            {
                ioSerializer.Object("structValue", ref StructValue);
            }
        }

        private struct IntProxyStruct : ISerializedProxy<int>
        {
            public int Value;

            public IntProxyStruct(int inValue)
            {
                Value = inValue;
            }

            int ISerializedProxy<int>.GetProxyValue(ISerializerContext inContext)
            {
                return Value;
            }

            void ISerializedProxy<int>.SetProxyValue(int inValue, ISerializerContext inContext)
            {
                Value = inValue;
            }
        }

        private class IntProxyTest : ISerializedObject
        {
            public IntProxyStruct IntProxy;
            public IntProxyStruct[] Array;
            public HashSet<IntProxyStruct> Set;
            public Dictionary<string, IntProxyStruct> Map;

            void ISerializedObject.Serialize(Serializer ioSerializer)
            {
                ioSerializer.Int32Proxy("intProxy", ref IntProxy);
                ioSerializer.Int32ProxyArray("array", ref Array);
                ioSerializer.Int32ProxySet("set", ref Set);
                ioSerializer.Int32ProxyMap("map", ref Map);
            }
        }

        #endregion // Object Types

        [SetUp]
        static public void Prelude()
        {
            TypeUtility.RegisterAlias<TestClassA>("TestClassA");
            TypeUtility.RegisterAlias<TestClassB>("TestClassB");
            TypeUtility.RegisterSerializer<CustomSerializerTest>(CustomSerializerTestSerializer);
            TestContext = AssetDatabase.LoadAssetAtPath<TestContext>("Assets/Editor/Context/TestContext.asset");
        }

        [Test]
        static public void AClassCanBeSerializedToJSON()
        {
            TestClassA testClass = new TestClassA()
            {
                someId = CONST_GUID,
                someFloatData = 0.1f
            };

            string serialized = Serializer.Write(testClass, OutputOptions.None, Serializer.Format.JSON);
            Assert.IsNotEmpty(serialized);
        }

        [Test]
        static public void AClassCanBeDeserializedFromJSON()
        {
            string serialized = "{\"someId\":\"c480fe9e-9116-45ca-a85b-623332bf0a33\",\"someFloatData\":0.100000001490116}";
            TestClassA deserialized = Serializer.Read<TestClassA>(serialized);

            Assert.AreEqual(deserialized.someId, CONST_GUID);
            Assert.True(Mathf.Approximately(deserialized.someFloatData, 0.1f));
        }

        [Test]
        static public void AClassCanBeSerializedToXML()
        {
            TestClassA testClass = new TestClassA()
            {
                someId = CONST_GUID,
                someFloatData = 0.1f
            };

            string serialized = Serializer.Write(testClass, OutputOptions.None, Serializer.Format.XML);
            Assert.IsNotEmpty(serialized);
        }

        [Test]
        static public void AClassCanBeDeserializedFromXML()
        {
            string serialized = @"<TestClassA><someId>c480fe9e-9116-45ca-a85b-623332bf0a33</someId><someFloatData>0.1</someFloatData></TestClassA>";
            TestClassA deserialized = Serializer.Read<TestClassA>(serialized);

            Assert.AreEqual(deserialized.someId, CONST_GUID);
            Assert.True(Mathf.Approximately(deserialized.someFloatData, 0.1f));
        }

        [Test]
        static public void AClassCanBeSerializedToBinary()
        {
            TestClassA testClass = new TestClassA()
            {
                someId = CONST_GUID,
                someFloatData = 0.1f
            };

            string serialized = Serializer.Write(testClass, OutputOptions.None, Serializer.Format.Binary);
            Assert.IsNotEmpty(serialized);
        }

        [Test]
        static public void AClassCanBeDeserializedFromBinary()
        {
            string serialized = "FBIN\"UQAADSRjNDgwZmU5ZS05MTE2LTQ1Y2EtYTg1Yi02MjMzMzJiZjBhMzMHzczMPQ==";
            TestClassA deserialized = Serializer.Read<TestClassA>(serialized);

            Assert.AreEqual(deserialized.someId, CONST_GUID);
            Assert.True(Mathf.Approximately(deserialized.someFloatData, 0.1f));
        }

        [Test]
        static public void AClassCanBeSerializedToGzip()
        {
            TestClassA testClass = new TestClassA()
            {
                someId = CONST_GUID,
                someFloatData = 0.1f
            };

            string serialized = Serializer.Write(testClass, OutputOptions.None, Serializer.Format.GZIP);
            Assert.IsNotEmpty(serialized);
        }

        [Test]
        static public void AClassCanBeDeserializedFromGzip()
        {
            string serialized = "FBGZ\"H4sIAPdzVFwA/wtkYOBVSTaxMEhLtUzVtTQ0NNM1MU1O1E20ME3SNTMyNjY2SkozSDQ2Zj975owtALZAh4MuAAAA";
            TestClassA deserialized = Serializer.Read<TestClassA>(serialized);

            Assert.AreEqual(deserialized.someId, CONST_GUID);
            Assert.True(Mathf.Approximately(deserialized.someFloatData, 0.1f));
        }

        [Test]
        static public void ASubclassCanBeSerializedAndDeserialized()
        {
            TestClassB testClass = new TestClassB()
            {
                someId = CONST_GUID,
                someFloatData = 0.2771f,
                someStringData = "there's a string here"
            };

            string serialized = Serializer.Write<TestClassA>(testClass, OutputOptions.None, Serializer.Format.JSON);

            TestClassA a = Serializer.Read<TestClassA>(serialized);

            Assert.IsInstanceOf(typeof(TestClassB), a);
        }

        [Test]
        static public void ACustomClassCanBeSerializedAndDeserialized()
        {
            CustomSerializerTest testClass = new CustomSerializerTest()
            {
                SomeVal = Vector2.zero,
                SomeVal2 = Vector2Int.left
            };

            CustomSerializerWrapper wrapper = new CustomSerializerWrapper()
            {
                Wrapped = testClass
            };

            string serialized = Serializer.Write(wrapper, OutputOptions.None, Serializer.Format.JSON);

            CustomSerializerWrapper a = Serializer.Read<CustomSerializerWrapper>(serialized);

            Assert.IsInstanceOf(typeof(CustomSerializerWrapper), a);
        }

        [Test]
        static public void AStructCanBeSerializedAndDeserialized()
        {
            TestSerializedStructWrapper testClass = new TestSerializedStructWrapper();
            testClass.StructValue.Value = 5;

            string serialized = Serializer.Write<TestSerializedStructWrapper>(testClass, OutputOptions.None, Serializer.Format.JSON);

            TestSerializedStructWrapper a = Serializer.Read<TestSerializedStructWrapper>(serialized);

            Assert.AreEqual(5, testClass.StructValue.Value);
        }

        [Test]
        static public void AssetReferencesCanBeSerializedAndDeserialized()
        {
            AssetRefTest test = new AssetRefTest();
            test.SomeVal = TestContext.sprites[2];

            string serialized = Serializer.Write(test, OutputOptions.None, Serializer.Format.Binary, TestContext);

            AssetRefTest loaded = Serializer.Read<AssetRefTest>(serialized, Serializer.Format.AutoDetect, TestContext);
            Assert.AreEqual(TestContext.sprites[2], loaded.SomeVal);
        }

        [Test]
        static public void ProxyValuesCanBeSerializedAndDeserialized()
        {
            IntProxyTest proxyTest = new IntProxyTest();
            proxyTest.IntProxy.Value = 5;
            proxyTest.Array = new IntProxyStruct[] { new IntProxyStruct(5), new IntProxyStruct(2), new IntProxyStruct(3) };

            string serialized = Serializer.Write(proxyTest, OutputOptions.None, Serializer.Format.JSON);
            Debug.Log(serialized);

            proxyTest = Serializer.Read<IntProxyTest>(serialized);
            Assert.AreEqual(5, proxyTest.IntProxy.Value);
            Assert.AreEqual(3, proxyTest.Array.Length);
            Assert.AreEqual(2, proxyTest.Array[1].Value);
        }

        [Test]
        static public void ConstructorStructsCanBeDeserializedAsInterfaces()
        {
            ISerializedObject obj = new TestSerializedStruct(5);
            string serialized = Serializer.Write(obj, OutputOptions.None, Serializer.Format.JSON);
            ISerializedObject readTest = Serializer.Read<ISerializedObject>(serialized);
            Assert.IsInstanceOf(typeof(TestSerializedStruct), readTest);
            Assert.AreEqual(5, ((TestSerializedStruct) readTest).Value);
        }

        [Test]
        static public void SizesOfEachSerializerType()
        {
            BenchmarkMemory<BinarySerializer>();
            BenchmarkMemory<JSONSerializer>();
            BenchmarkMemory<XMLSerializer>();
            BenchmarkMemory<GZIPSerializer>();
        }

        static private void BenchmarkMemory<T>(int iterations = 1000) where T : class, new()
        {
            // prewarm
            T newT = new T();
            newT = newT ?? null;

            T[] arr = new T[iterations];
            GC.Collect();
            long firstMemory = GC.GetTotalMemory(true);
            for (int i = 0; i < iterations; ++i)
            {
                arr[i] = new T();
            }
            long secondMemory = GC.GetTotalMemory(false);
            long totalMemory = (secondMemory - firstMemory) / iterations;
            Debug.Log(string.Format("SizeOf {0}: {1}", typeof(T).FullName, EditorUtility.FormatBytes(totalMemory)));
        }
    }
}