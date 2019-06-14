using System;
using NUnit.Framework;
using UnityEngine;

namespace BeauData.Editor
{
    static public class Tests
    {
        static private readonly Guid CONST_GUID = new Guid("c480fe9e-9116-45ca-a85b-623332bf0a33");

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

        #endregion // Object Types

        [SetUp]
        static public void Prelude()
        {
            TypeUtility.RegisterAlias<TestClassA>("TestClassA");
            TypeUtility.RegisterAlias<TestClassB>("TestClassB");
            TypeUtility.RegisterSerializer<CustomSerializerTest>(CustomSerializerTestSerializer);
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
    }
}