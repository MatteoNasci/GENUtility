﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using GENUtility;
using static GENUtility.ByteManipulator;
[Category("Utility")]
[TestOf(typeof(ByteManipulator))]
[TestFixture]
public class ByteManipulatorTest
{
    Encoding encoder;
    [SetUp]
    public void EncodingSetup()
    {
        encoder = System.Text.Encoding.UTF8;
    }
    [TearDown]
    public void EncodingTeardown()
    {
        encoder = null;
    }
    [Test]
    public void TestBitConverterFloat()
    {
        float value = 10f;
        byte[] arr = BitConverter.GetBytes(value);
        Assert.That(BitConverter.ToSingle(arr, 0), Is.EqualTo(10f).Within(0.0001));
    }
    [Test]
    public void TestBitConverterFloatRedLight()
    {
        byte[] arr = BitConverter.GetBytes(10f);
        Assert.That(BitConverter.ToSingle(arr, 0), Is.Not.EqualTo(9f).Within(0.0001));
    }
    [Test]
    public void TestFloatConversion()
    {
        byte[] arr = new byte[sizeof(float)];
        Write(arr, 0, 502144.251f);
        Assert.That(BitConverter.ToSingle(arr, 0), Is.EqualTo(502144.251f).Within(0.0001));
    }
    [Test]
    public void TestFloatConversionRedLight()
    {
        byte[] arr = new byte[sizeof(float)];
        Write(arr, 0, 10f);
        Assert.That(BitConverter.ToSingle(arr, 0), Is.Not.EqualTo(9f).Within(0.0001));
    }
    [Test]
    public void TestStringConversion()
    {
        string s = "dngsgnsiongDDDD@@#[ffa";
        byte[] arr = new byte[Encoding.UTF8.GetByteCount(s) + sizeof(int)];
        Write(arr, 0, s, encoder);
        Assert.That(Encoding.UTF8.GetString(arr, sizeof(int), ReadInt32(arr, 0)), Is.EqualTo("dngsgnsiongDDDD@@#[ffa"));
    }
    [Test]
    public void TestStringConversionRedLight()
    {
        string s = "dngsgnsiongDDDD@@#[ffa ";
        byte[] arr = new byte[Encoding.UTF8.GetByteCount(s) + sizeof(int)];
        Write(arr, 0, s, encoder);
        Assert.That(Encoding.UTF8.GetString(arr, sizeof(int), ReadInt32(arr, 0)), Is.Not.EqualTo("dngsgnsiongDDDD@@#[ffa"));
    }
    [Test]
    public void TestDoubleConversion()
    {
        byte[] arr = new byte[sizeof(double)];
        Write(arr, 0, 1000.251d);
        Assert.That(BitConverter.ToDouble(arr, 0), Is.EqualTo(1000.251d));
    }
    [Test]
    public void TestDoubleConversionRedLight()
    {
        byte[] arr = new byte[sizeof(double)];
        Write(arr, 0, 1000000d);
        Assert.That(BitConverter.ToDouble(arr, 0), Is.Not.EqualTo(0d));
    }
    [Test]
    public void TestShortConversion()
    {
        byte[] arr = new byte[sizeof(short)];
        Write(arr, 0, (short)-2500);
        Assert.That(BitConverter.ToInt16(arr, 0), Is.EqualTo(-2500));
    }
    [Test]
    public void TestShortConversionRedLight()
    {
        byte[] arr = new byte[sizeof(short)];
        Write(arr, 0, (short)-25);
        Assert.That(BitConverter.ToInt16(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestCharConversion()
    {
        byte[] arr = new byte[sizeof(char)];
        Write(arr, 0, 'b');
        Assert.That(BitConverter.ToChar(arr, 0), Is.EqualTo('b'));
    }
    [Test]
    public void TestWriteArrayByte()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        Write(first, 1, second, 0, 9);
        Assert.That(second[0], Is.EqualTo(1));
    }
    [Test]
    public void TestWriteArrayByteRedLight()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        Write(first, 0, second, 0, 9);
        Assert.That(second[0], Is.Not.EqualTo(1));
    }
    [Test]
    public void TestWriteArrayByte2()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        Write(first, 1, second, 0, 9);
        Assert.That(second[5], Is.EqualTo(6));
    }
    [Test]
    public void TestWriteArrayByteRedLight2()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        Write(first, 0, second, 0, 9);
        Assert.That(second[5], Is.Not.EqualTo(6));
    }
    [Test]
    public void TestWriteArrayByte3()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        Write(first, 1, second, 0, 9);
        Assert.That(second[8], Is.EqualTo(9));
    }
    [Test]
    public void TestWriteArrayByteRedLight3()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        ByteManipulator.Write(first, 0, second, 0, 9);
        Assert.That(second[8], Is.Not.EqualTo(9));
    }
    [Test]
    public void TestWriteArrayByte4()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        ByteManipulator.Write(first, 1, second, 0, 9);
        Assert.That(second[9], Is.EqualTo(0));
    }
    [Test]
    public void TestWriteArrayByteRedLight4()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        ByteManipulator.Write(first, 0, second, 1, 9);
        Assert.That(second[9], Is.Not.EqualTo(0));
    }
    [Test]
    public void TestWriteArrayFloat()
    {
        float[] first = new float[] { 0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f };
        float[] second = new float[10];
        ByteManipulator.Write(first, 1, second, 0, 9);
        Assert.That(second[0], Is.EqualTo(1).Within(0.0001));
    }
    [Test]
    public void TestWriteArrayFloatRedLight()
    {
        float[] first = new float[] { 0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f };
        float[] second = new float[10];
        ByteManipulator.Write(first, 0, second, 0, 9);
        Assert.That(second[0], Is.Not.EqualTo(1).Within(0.0001));
    }
    [Test]
    public void TestWriteArrayFloat2()
    {
        float[] first = new float[] { 0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f };
        float[] second = new float[10];
        ByteManipulator.Write(first, 1, second, 0, 9);
        Assert.That(second[5], Is.EqualTo(6).Within(0.0001));
    }
    [Test]
    public void TestWriteArrayFloatRedLight2()
    {
        float[] first = new float[] { 0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f };
        float[] second = new float[10];
        ByteManipulator.Write(first, 0, second, 0, 9);
        Assert.That(second[5], Is.Not.EqualTo(6).Within(0.0001));
    }
    [Test]
    public void TestWriteArrayFloat3()
    {
        float[] first = new float[] { 0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f };
        float[] second = new float[10];
        ByteManipulator.Write(first, 1, second, 0, 9);
        Assert.That(second[8], Is.EqualTo(9).Within(0.0001));
    }
    [Test]
    public void TestWriteArrayFloatRedLight3()
    {
        float[] first = new float[] { 0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f };
        float[] second = new float[10];
        ByteManipulator.Write(first, 0, second, 0, 9);
        Assert.That(second[8], Is.Not.EqualTo(9).Within(0.0001));
    }
    [Test]
    public void TestWriteArrayFloat4()
    {
        float[] first = new float[] { 0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f };
        float[] second = new float[10];
        ByteManipulator.Write(first, 1, second, 0, 9);
        Assert.That(second[9], Is.EqualTo(0).Within(0.0001));
    }
    [Test]
    public void TestWriteArrayFloatRedLight4()
    {
        float[] first = new float[] { 0f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f };
        float[] second = new float[10];
        ByteManipulator.Write(first, 0, second, 1, 9);
        Assert.That(second[9], Is.Not.EqualTo(0).Within(0.0001));
    }
    [Test]
    public void TestWriteArrayByteToCycle()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        int n = ByteManipulator.WriteToCycle(first, 1, second, 0, 9);
        Assert.That(second[0], Is.EqualTo(1));
    }
    [Test]
    public void TestWriteArrayByteToCycleRedLight()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        int n = ByteManipulator.WriteToCycle(first, 0, second, 0, 9);
        Assert.That(second[0], Is.Not.EqualTo(1));
    }
    [Test]
    public void TestWriteArrayByteToCycle2()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        int n = ByteManipulator.WriteToCycle(first, 1, second, 8, 9);
        Assert.That(second[3], Is.EqualTo(6));
    }
    [Test]
    public void TestWriteArrayByteToCycleRedLight2()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        int n = ByteManipulator.WriteToCycle(first, 0, second, 8, 9);
        Assert.That(second[3], Is.Not.EqualTo(6));
    }
    [Test]
    public void TestWriteArrayByteToCycleNewOffset()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        int n = ByteManipulator.WriteToCycle(first, 1, second, 8, 9);
        Assert.That(n, Is.EqualTo(7));
    }
    [Test]
    public void TestWriteArrayByteToCycleNewOffsetRedLight()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        int n = ByteManipulator.WriteToCycle(first, 0, second, 8, 9);
        Assert.That(n, Is.Not.EqualTo(3));
    }
    [Test]
    public void TestWriteArrayByteFromCycle()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        int n = ByteManipulator.WriteFromCycle(first, 1, second, 0, 9);
        Assert.That(second[0], Is.EqualTo(1));
    }
    [Test]
    public void TestWriteArrayByteFromCycleRedLight()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        int n = ByteManipulator.WriteFromCycle(first, 0, second, 0, 9);
        Assert.That(second[0], Is.Not.EqualTo(1));
    }
    [Test]
    public void TestWriteArrayByteFromCycle2()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        int n = ByteManipulator.WriteFromCycle(first, 7, second, 0, 9);
        Assert.That(second[3], Is.EqualTo(0));
    }
    [Test]
    public void TestWriteArrayByteFromCycleRedLight2()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        int n = ByteManipulator.WriteFromCycle(first, 6, second, 1, 9);
        Assert.That(second[3], Is.Not.EqualTo(0));
    }
    [Test]
    public void TestWriteArrayByteFromCycleNewOffset()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        int n = ByteManipulator.WriteFromCycle(first, 7, second, 0, 9);
        Assert.That(n, Is.EqualTo(6));
    }
    [Test]
    public void TestWriteArrayByteFromCycleNewOffsetRedLight()
    {
        byte[] first = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        byte[] second = new byte[10];
        int n = ByteManipulator.WriteFromCycle(first, 6, second, 1, 9);
        Assert.That(n, Is.Not.EqualTo(6));
    }
    [Test]
    public void TestCharConversionRedLight()
    {
        byte[] arr = new byte[sizeof(char)];
        ByteManipulator.Write(arr, 0, 'b');
        Assert.That(BitConverter.ToChar(arr, 0), Is.Not.EqualTo('a'));
    }
    [Test]
    public void TestUshortConversion()
    {
        byte[] arr = new byte[sizeof(ushort)];
        ByteManipulator.Write(arr, 0, (ushort)2500);
        Assert.That(BitConverter.ToUInt16(arr, 0), Is.EqualTo(2500));
    }
    [Test]
    public void TestUshortConversionRedLight()
    {
        byte[] arr = new byte[sizeof(ushort)];
        ByteManipulator.Write(arr, 0, (ushort)25);
        Assert.That(BitConverter.ToUInt16(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestIntConversion()
    {
        byte[] arr = new byte[sizeof(int)];
        ByteManipulator.Write(arr, 0, -2500000);
        Assert.That(BitConverter.ToInt32(arr, 0), Is.EqualTo(-2500000));
    }
    [Test]
    public void TestIntConversionRedLight()
    {
        byte[] arr = new byte[sizeof(int)];
        ByteManipulator.Write(arr, 0, -25);
        Assert.That(BitConverter.ToInt32(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestUintConversion()
    {
        byte[] arr = new byte[sizeof(uint)];
        ByteManipulator.Write(arr, 0, (uint)2500000000);
        Assert.That(BitConverter.ToUInt32(arr, 0), Is.EqualTo(2500000000));
    }
    [Test]
    public void TestUintConversionRedLight()
    {
        byte[] arr = new byte[sizeof(uint)];
        ByteManipulator.Write(arr, 0, (uint)25);
        Assert.That(BitConverter.ToUInt32(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestLongConversion()
    {
        byte[] arr = new byte[sizeof(long)];
        ByteManipulator.Write(arr, 0, (long)-250000000000000);
        Assert.That(BitConverter.ToInt64(arr, 0), Is.EqualTo(-250000000000000));
    }
    [Test]
    public void TestLongConversionRedLight()
    {
        byte[] arr = new byte[sizeof(long)];
        ByteManipulator.Write(arr, 0, (long)-25);
        Assert.That(BitConverter.ToInt64(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestUlongConversion()
    {
        byte[] arr = new byte[sizeof(ulong)];
        ByteManipulator.Write(arr, 0, (ulong)18446744073709551614);
        Assert.That(BitConverter.ToUInt64(arr, 0), Is.EqualTo(18446744073709551614));
    }
    [Test]
    public void TestUlongConversionRedLight()
    {
        byte[] arr = new byte[sizeof(ulong)];
        ByteManipulator.Write(arr, 0, (ulong)25000544);
        Assert.That(BitConverter.ToUInt64(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestByteConversion()
    {
        byte[] arr = new byte[sizeof(byte)];
        ByteManipulator.Write(arr, 0, (byte)128);
        Assert.That(arr[0], Is.EqualTo(128));
    }
    [Test]
    public void TestByteConversionRedLight()
    {
        byte[] arr = new byte[sizeof(byte)];
        ByteManipulator.Write(arr, 0, (byte)25);
        Assert.That(arr[0], Is.Not.EqualTo(9));
    }
    [Test]
    public void TestSByteConversion()
    {
        byte[] arr = new byte[sizeof(sbyte)];
        ByteManipulator.Write(arr, 0, (sbyte)-122);
        Assert.That((sbyte)arr[0], Is.EqualTo(-122));
    }
    [Test]
    public void TestSByteConversionRedLight()
    {
        byte[] arr = new byte[sizeof(sbyte)];
        ByteManipulator.Write(arr, 0, (sbyte)25);
        Assert.That((sbyte)arr[0], Is.Not.EqualTo(9));
    }
    [Test]
    public void TestBoolConversion()
    {
        byte[] arr = new byte[sizeof(bool)];
        ByteManipulator.Write(arr, 0, true);
        Assert.That(BitConverter.ToBoolean(arr, 0), Is.True);
    }
    [Test]
    public void TestBoolConversionRedLight()
    {
        byte[] arr = new byte[sizeof(bool)];
        ByteManipulator.Write(arr, 0, false);
        Assert.That(BitConverter.ToBoolean(arr, 0), Is.Not.True);
    }
    [Test]
    public void TestListFloatConversion()
    {
        List<byte> arr = new List<byte>(sizeof(float));
        for (int i = 0; i < sizeof(float); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, 10.251f);
        Assert.That(BitConverter.ToSingle(arr.ToArray(), 0), Is.EqualTo(10.251f).Within(0.0001));
    }
    [Test]
    public void TestListFloatConversionRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(float));
        for (int i = 0; i < sizeof(float); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, 10f);
        Assert.That(BitConverter.ToSingle(arr.ToArray(), 0), Is.Not.EqualTo(9f).Within(0.0001));
    }
    [Test]
    public void TestListDoubleConversion()
    {
        List<byte> arr = new List<byte>(sizeof(double));
        for (int i = 0; i < sizeof(double); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, 10.251d);
        Assert.That(BitConverter.ToDouble(arr.ToArray(), 0), Is.EqualTo(10.251d));
    }
    [Test]
    public void TestListDoubleConversionRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(double));
        for (int i = 0; i < sizeof(double); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, 10d);
        Assert.That(BitConverter.ToDouble(arr.ToArray(), 0), Is.Not.EqualTo(9d));
    }
    [Test]
    public void TestListShortConversion()
    {
        List<byte> arr = new List<byte>(sizeof(short));
        for (int i = 0; i < sizeof(short); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (short)-25);
        Assert.That(BitConverter.ToInt16(arr.ToArray(), 0), Is.EqualTo(-25));
    }
    [Test]
    public void TestListShortConversionRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(short));
        for (int i = 0; i < sizeof(short); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (short)-25);
        Assert.That(BitConverter.ToInt16(arr.ToArray(), 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListCharConversion()
    {
        List<byte> arr = new List<byte>(sizeof(char));
        for (int i = 0; i < sizeof(char); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, 'z');
        Assert.That(BitConverter.ToChar(arr.ToArray(), 0), Is.EqualTo('z'));
    }
    [Test]
    public void TestListCharConversionRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(char));
        for (int i = 0; i < sizeof(char); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, 'u');
        Assert.That(BitConverter.ToChar(arr.ToArray(), 0), Is.Not.EqualTo('z'));
    }
    [Test]
    public void TestListUshortConversion()
    {
        List<byte> arr = new List<byte>(sizeof(ushort));
        for (int i = 0; i < sizeof(ushort); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (ushort)25);
        Assert.That(BitConverter.ToUInt16(arr.ToArray(), 0), Is.EqualTo(25));
    }
    [Test]
    public void TestListUshortConversionRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(ushort));
        for (int i = 0; i < sizeof(ushort); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (ushort)25);
        Assert.That(BitConverter.ToUInt16(arr.ToArray(), 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListIntConversion()
    {
        List<byte> arr = new List<byte>(sizeof(int));
        for (int i = 0; i < sizeof(int); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, -25);
        Assert.That(BitConverter.ToInt32(arr.ToArray(), 0), Is.EqualTo(-25));
    }
    [Test]
    public void TestListIntConversionRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(int));
        for (int i = 0; i < sizeof(int); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, -25);
        Assert.That(BitConverter.ToInt32(arr.ToArray(), 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListUintConversion()
    {
        List<byte> arr = new List<byte>(sizeof(uint));
        for (int i = 0; i < sizeof(uint); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (uint)25);
        Assert.That(BitConverter.ToUInt32(arr.ToArray(), 0), Is.EqualTo(25));
    }
    [Test]
    public void TestListUintConversionRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(uint));
        for (int i = 0; i < sizeof(uint); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (uint)25);
        Assert.That(BitConverter.ToUInt32(arr.ToArray(), 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListLongConversion()
    {
        List<byte> arr = new List<byte>(sizeof(long));
        for (int i = 0; i < sizeof(long); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (long)-25);
        Assert.That(BitConverter.ToInt64(arr.ToArray(), 0), Is.EqualTo(-25));
    }
    [Test]
    public void TestListLongConversionRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(long));
        for (int i = 0; i < sizeof(long); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (long)-25);
        Assert.That(BitConverter.ToInt64(arr.ToArray(), 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListUlongConversion()
    {
        List<byte> arr = new List<byte>(sizeof(ulong));
        for (int i = 0; i < sizeof(ulong); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (ulong)18446744073709551614);
        Assert.That(BitConverter.ToUInt64(arr.ToArray(), 0), Is.EqualTo(18446744073709551614));
    }
    [Test]
    public void TestListUlongConversionRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(ulong));
        for (int i = 0; i < sizeof(ulong); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (ulong)25000544);
        Assert.That(BitConverter.ToUInt64(arr.ToArray(), 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListByteConversion()
    {
        List<byte> arr = new List<byte>(sizeof(byte));
        for (int i = 0; i < sizeof(byte); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (byte)25);
        Assert.That(arr[0], Is.EqualTo(25));
    }
    [Test]
    public void TestListByteConversionRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(byte));
        for (int i = 0; i < sizeof(byte); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (byte)25);
        Assert.That(arr[0], Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListSByteConversion()
    {
        List<byte> arr = new List<byte>(sizeof(sbyte));
        for (int i = 0; i < sizeof(sbyte); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (sbyte)-121);
        Assert.That((sbyte)arr[0], Is.EqualTo(-121));
    }
    [Test]
    public void TestListSByteConversionRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(sbyte));
        for (int i = 0; i < sizeof(sbyte); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (sbyte)25);
        Assert.That((sbyte)arr[0], Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListBoolConversion()
    {
        List<byte> arr = new List<byte>(sizeof(bool));
        for (int i = 0; i < sizeof(bool); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, true);
        Assert.That(BitConverter.ToBoolean(arr.ToArray(), 0), Is.True);
    }
    [Test]
    public void TestListBoolConversionRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(bool));
        for (int i = 0; i < sizeof(bool); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, false);
        Assert.That(BitConverter.ToBoolean(arr.ToArray(), 0), Is.Not.True);
    }

    [Test]
    public void TestFloatRead()
    {
        byte[] arr = new byte[sizeof(float)];
        ByteManipulator.Write(arr, 0, 502144.251f);
        Assert.That(ByteManipulator.ReadSingle(arr, 0), Is.EqualTo(502144.251f).Within(0.0001));
    }
    [Test]
    public void TestFloatReadRedLight()
    {
        byte[] arr = new byte[sizeof(float)];
        ByteManipulator.Write(arr, 0, 10f);
        Assert.That(ByteManipulator.ReadSingle(arr, 0), Is.Not.EqualTo(9f).Within(0.0001));
    }
    [Test]
    public void TestDoubleRead()
    {
        byte[] arr = new byte[sizeof(double)];
        ByteManipulator.Write(arr, 0, 1000.251d);
        Assert.That(ByteManipulator.ReadDouble(arr, 0), Is.EqualTo(1000.251d));
    }
    [Test]
    public void TestDoubleReadRedLight()
    {
        byte[] arr = new byte[sizeof(double)];
        ByteManipulator.Write(arr, 0, 1000000d);
        Assert.That(ByteManipulator.ReadDouble(arr, 0), Is.Not.EqualTo(0d));
    }
    [Test]
    public void TestShortRead()
    {
        byte[] arr = new byte[sizeof(short)];
        ByteManipulator.Write(arr, 0, (short)-2500);
        Assert.That(ByteManipulator.ReadInt16(arr, 0), Is.EqualTo(-2500));
    }
    [Test]
    public void TestShortReadRedLight()
    {
        byte[] arr = new byte[sizeof(short)];
        ByteManipulator.Write(arr, 0, (short)-25);
        Assert.That(ByteManipulator.ReadInt16(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestCharRead()
    {
        byte[] arr = new byte[sizeof(char)];
        ByteManipulator.Write(arr, 0, 'b');
        Assert.That(ByteManipulator.ReadChar(arr, 0), Is.EqualTo('b'));
    }
    [Test]
    public void TestCharReadRedLight()
    {
        byte[] arr = new byte[sizeof(char)];
        ByteManipulator.Write(arr, 0, 'b');
        Assert.That(ByteManipulator.ReadChar(arr, 0), Is.Not.EqualTo('a'));
    }
    [Test]
    public void TestUshortRead()
    {
        byte[] arr = new byte[sizeof(ushort)];
        ByteManipulator.Write(arr, 0, (ushort)60001);
        Assert.That(ByteManipulator.ReadUInt16(arr, 0), Is.EqualTo(60001));
    }
    [Test]
    public void TestUshortReadRedLight()
    {
        byte[] arr = new byte[sizeof(ushort)];
        ByteManipulator.Write(arr, 0, (ushort)25);
        Assert.That(ByteManipulator.ReadUInt16(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestStringRead()
    {
        string s = "dngsgnsiongDDDD@@#[ffa";
        byte[] arr = new byte[Encoding.UTF8.GetByteCount(s) + sizeof(int)];
        ByteManipulator.Write(arr, 0, s, encoder);
        int n;
        Assert.That(ByteManipulator.ReadString(arr, 0, encoder, out n), Is.EqualTo("dngsgnsiongDDDD@@#[ffa"));
    }
    [Test]
    public void TestStringReadRedLight()
    {
        string s = "dngsgnsiongDDDD@@#[ffa ";
        byte[] arr = new byte[Encoding.UTF8.GetByteCount(s) + sizeof(int)];
        ByteManipulator.Write(arr, 0, s, encoder);
        int n;
        Assert.That(ByteManipulator.ReadString(arr, 0, encoder, out n), Is.Not.EqualTo("dngsgnsiongDDDD@@#[ffa"));
    }
    [Test]
    public void TestCharsConversions()
    {
        char[] s = "dngsgnsiongDDDD@@#[ffa".ToCharArray();
        char[] s2 = new char[s.Length];
        byte[] arr = new byte[Encoding.UTF8.GetByteCount(s) + sizeof(int)];
        ByteManipulator.Write(arr, 0, s, encoder);
        int n = ByteManipulator.ReadInt32(arr, 0);
        Assert.That(Encoding.UTF8.GetString(arr, sizeof(int), n), Is.EqualTo("dngsgnsiongDDDD@@#[ffa"));
    }
    [Test]
    public void TestCharsConversionsRedLight()
    {
        char[] s = "dngsgnsiongDDDD@@#[ffa ".ToCharArray();
        char[] s2 = new char[s.Length];
        byte[] arr = new byte[Encoding.UTF8.GetByteCount(s) + sizeof(int)];
        ByteManipulator.Write(arr, 0, s, encoder);
        int n = ByteManipulator.ReadInt32(arr, 0);
        Assert.That(Encoding.UTF8.GetString(arr, sizeof(int), n), Is.Not.EqualTo("dngsgnsiongDDDD@@#[ffa"));
    }
    [Test]
    public void TestCharsRead()
    {
        char[] s = "dngsgnsiongDDDD@@#[ffa".ToCharArray();
        char[] s2 = new char[s.Length];
        byte[] arr = new byte[Encoding.UTF8.GetByteCount(s) + sizeof(int)];
        ByteManipulator.Write(arr, 0, s, encoder);
        int n;
        int n2;
        ByteManipulator.ReadChars(arr, 0, s2, 0, encoder, out n, out n2);
        Assert.That(new string(s2), Is.EqualTo("dngsgnsiongDDDD@@#[ffa"));
    }
    [Test]
    public void TestCharsReadRedLight()
    {
        char[] s = "dngsgnsiongDDDD@@#[ffa ".ToCharArray();
        char[] s2 = new char[s.Length];
        byte[] arr = new byte[Encoding.UTF8.GetByteCount(s) + sizeof(int)];
        ByteManipulator.Write(arr, 0, s, encoder);
        int n;
        int n2;
        ByteManipulator.ReadChars(arr, 0, s2, 0, encoder, out n, out n2);
        Assert.That(new string(s2), Is.Not.EqualTo("dngsgnsiongDDDD@@#[ffa"));
    }
    [Test]
    public void TestIntRead()
    {
        byte[] arr = new byte[sizeof(int)];
        ByteManipulator.Write(arr, 0, -2500001);
        Assert.That(ByteManipulator.ReadInt32(arr, 0), Is.EqualTo(-2500001));
    }
    [Test]
    public void TestIntReadRedLight()
    {
        byte[] arr = new byte[sizeof(int)];
        ByteManipulator.Write(arr, 0, -25);
        Assert.That(ByteManipulator.ReadInt32(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestUintRead()
    {
        byte[] arr = new byte[sizeof(uint)];
        ByteManipulator.Write(arr, 0, (uint)2500000001);
        Assert.That(ByteManipulator.ReadUInt32(arr, 0), Is.EqualTo(2500000001));
    }
    [Test]
    public void TestUintReadRedLight()
    {
        byte[] arr = new byte[sizeof(uint)];
        ByteManipulator.Write(arr, 0, (uint)25);
        Assert.That(ByteManipulator.ReadUInt32(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestLongRead()
    {
        byte[] arr = new byte[sizeof(long)];
        ByteManipulator.Write(arr, 0, (long)-250000000000001);
        Assert.That(ByteManipulator.ReadInt64(arr, 0), Is.EqualTo(-250000000000001));
    }
    [Test]
    public void TestLongReadRedLight()
    {
        byte[] arr = new byte[sizeof(long)];
        ByteManipulator.Write(arr, 0, (long)-25);
        Assert.That(ByteManipulator.ReadInt64(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestUlongRead()
    {
        byte[] arr = new byte[sizeof(ulong)];
        ByteManipulator.Write(arr, 0, (ulong)18446744073709551614);
        Assert.That(ByteManipulator.ReadUInt64(arr, 0), Is.EqualTo(18446744073709551614));
    }
    [Test]
    public void TestUlongReadRedLight()
    {
        byte[] arr = new byte[sizeof(ulong)];
        ByteManipulator.Write(arr, 0, (ulong)25000544);
        Assert.That(ByteManipulator.ReadUInt64(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestByteRead()
    {
        byte[] arr = new byte[sizeof(byte)];
        ByteManipulator.Write(arr, 0, (byte)128);
        Assert.That(ByteManipulator.ReadByte(arr, 0), Is.EqualTo(128));
    }
    [Test]
    public void TestByteReadRedLight()
    {
        byte[] arr = new byte[sizeof(byte)];
        ByteManipulator.Write(arr, 0, (byte)25);
        Assert.That(ByteManipulator.ReadByte(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestSByteRead()
    {
        byte[] arr = new byte[sizeof(sbyte)];
        ByteManipulator.Write(arr, 0, (sbyte)-111);
        Assert.That(ByteManipulator.ReadSByte(arr, 0), Is.EqualTo(-111));
    }
    [Test]
    public void TestSByteReadRedLight()
    {
        byte[] arr = new byte[sizeof(sbyte)];
        ByteManipulator.Write(arr, 0, (sbyte)25);
        Assert.That(ByteManipulator.ReadSByte(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestBoolRead()
    {
        byte[] arr = new byte[sizeof(bool)];
        ByteManipulator.Write(arr, 0, true);
        Assert.That(ByteManipulator.ReadBoolean(arr, 0), Is.True);
    }
    [Test]
    public void TestBoolReadRedLight()
    {
        byte[] arr = new byte[sizeof(bool)];
        ByteManipulator.Write(arr, 0, false);
        Assert.That(ByteManipulator.ReadBoolean(arr, 0), Is.Not.True);
    }
    [Test]
    public void TestListFloatRead()
    {
        List<byte> arr = new List<byte>(sizeof(float));
        for (int i = 0; i < sizeof(float); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, 10.251f);
        Assert.That(ByteManipulator.ReadSingle(arr, 0), Is.EqualTo(10.251f).Within(0.0001));
    }
    [Test]
    public void TestListFloatReadRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(float));
        for (int i = 0; i < sizeof(float); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, 10f);
        Assert.That(ByteManipulator.ReadSingle(arr, 0), Is.Not.EqualTo(9f).Within(0.0001));
    }
    [Test]
    public void TestListDoubleRead()
    {
        List<byte> arr = new List<byte>(sizeof(double));
        for (int i = 0; i < sizeof(double); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, 10.251d);
        Assert.That(ByteManipulator.ReadDouble(arr, 0), Is.EqualTo(10.251d));
    }
    [Test]
    public void TestListDoubleReadRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(double));
        for (int i = 0; i < sizeof(double); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, 10d);
        Assert.That(ByteManipulator.ReadDouble(arr, 0), Is.Not.EqualTo(9d));
    }
    [Test]
    public void TestListShortRead()
    {
        List<byte> arr = new List<byte>(sizeof(short));
        for (int i = 0; i < sizeof(short); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (short)-25);
        Assert.That(ByteManipulator.ReadInt16(arr, 0), Is.EqualTo(-25));
    }
    [Test]
    public void TestListShortReadRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(short));
        for (int i = 0; i < sizeof(short); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (short)-25);
        Assert.That(ByteManipulator.ReadInt16(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListCharRead()
    {
        List<byte> arr = new List<byte>(sizeof(char));
        for (int i = 0; i < sizeof(char); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, 'z');
        Assert.That(ByteManipulator.ReadChar(arr, 0), Is.EqualTo('z'));
    }
    [Test]
    public void TestListCharReadRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(char));
        for (int i = 0; i < sizeof(char); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, 'u');
        Assert.That(ByteManipulator.ReadChar(arr, 0), Is.Not.EqualTo('z'));
    }
    [Test]
    public void TestListUshortRead()
    {
        List<byte> arr = new List<byte>(sizeof(ushort));
        for (int i = 0; i < sizeof(ushort); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (ushort)25);
        Assert.That(ByteManipulator.ReadUInt16(arr, 0), Is.EqualTo(25));
    }
    [Test]
    public void TestListUshortReadRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(ushort));
        for (int i = 0; i < sizeof(ushort); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (ushort)25);
        Assert.That(ByteManipulator.ReadUInt16(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListIntRead()
    {
        List<byte> arr = new List<byte>(sizeof(int));
        for (int i = 0; i < sizeof(int); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, -25);
        Assert.That(ByteManipulator.ReadInt32(arr, 0), Is.EqualTo(-25));
    }
    [Test]
    public void TestListIntReadRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(int));
        for (int i = 0; i < sizeof(int); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, -25);
        Assert.That(ByteManipulator.ReadInt32(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListUintRead()
    {
        List<byte> arr = new List<byte>(sizeof(uint));
        for (int i = 0; i < sizeof(uint); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (uint)25);
        Assert.That(ByteManipulator.ReadUInt32(arr, 0), Is.EqualTo(25));
    }
    [Test]
    public void TestListUintReadRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(uint));
        for (int i = 0; i < sizeof(uint); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (uint)25);
        Assert.That(ByteManipulator.ReadUInt32(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListLongRead()
    {
        List<byte> arr = new List<byte>(sizeof(long));
        for (int i = 0; i < sizeof(long); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (long)-25);
        Assert.That(ByteManipulator.ReadInt64(arr, 0), Is.EqualTo(-25));
    }
    [Test]
    public void TestListLongReadRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(long));
        for (int i = 0; i < sizeof(long); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (long)-25);
        Assert.That(ByteManipulator.ReadInt64(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListUlongRead()
    {
        List<byte> arr = new List<byte>(sizeof(ulong));
        for (int i = 0; i < sizeof(ulong); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (ulong)18446744073709551614);
        Assert.That(ByteManipulator.ReadUInt64(arr, 0), Is.EqualTo(18446744073709551614));
    }
    [Test]
    public void TestListUlongReadRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(ulong));
        for (int i = 0; i < sizeof(ulong); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (ulong)25000544);
        Assert.That(ByteManipulator.ReadUInt64(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListByteRead()
    {
        List<byte> arr = new List<byte>(sizeof(byte));
        for (int i = 0; i < sizeof(byte); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (byte)25);
        Assert.That(ByteManipulator.ReadByte(arr, 0), Is.EqualTo(25));
    }
    [Test]
    public void TestListByteReadRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(byte));
        for (int i = 0; i < sizeof(byte); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (byte)25);
        Assert.That(ByteManipulator.ReadByte(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListSByteRead()
    {
        List<byte> arr = new List<byte>(sizeof(sbyte));
        for (int i = 0; i < sizeof(sbyte); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (sbyte)-111);
        Assert.That(ByteManipulator.ReadSByte(arr, 0), Is.EqualTo(-111));
    }
    [Test]
    public void TestListSByteReadRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(sbyte));
        for (int i = 0; i < sizeof(sbyte); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, (sbyte)-25);
        Assert.That(ByteManipulator.ReadSByte(arr, 0), Is.Not.EqualTo(9));
    }
    [Test]
    public void TestListBoolRead()
    {
        List<byte> arr = new List<byte>(sizeof(bool));
        for (int i = 0; i < sizeof(bool); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, true);
        Assert.That(ByteManipulator.ReadBoolean(arr, 0), Is.True);
    }
    [Test]
    public void TestListBoolReadRedLight()
    {
        List<byte> arr = new List<byte>(sizeof(bool));
        for (int i = 0; i < sizeof(bool); i++)
        {
            arr.Add(0);
        }
        ByteManipulator.Write(arr, 0, false);
        Assert.That(ByteManipulator.ReadBoolean(arr, 0), Is.Not.True);
    }
    byte value;
    byte[] arrValue;
    [Test]
    public void TestBitRead()
    {
        Assert.That(ByteManipulator.ReadBit(value, 1), Is.True);
    }
    [Test]
    public void TestBitReadRedLight()
    {
        Assert.That(ByteManipulator.ReadBit(value, 1), Is.Not.False);
    }
    [Test]
    public void TestBitRead2()
    {
        Assert.That(ByteManipulator.ReadBit(value, 3), Is.False);
    }
    [Test]
    public void TestBitReadRedLight2()
    {
        Assert.That(ByteManipulator.ReadBit(value, 3), Is.Not.True);
    }
    [Test]
    public void TestBitWrite()
    {
        WriteBit(ref value, 3, true);
        Assert.That(ByteManipulator.ReadBit(value, 3), Is.True);
    }
    [Test]
    public void TestBitWriteRedLight()
    {
        WriteBit(ref value, 3, true);
        Assert.That(ByteManipulator.ReadBit(value, 3), Is.Not.False);
    }
    [Test]
    public void TestBitWrite2()
    {
        WriteBit(ref value, 2, false);
        Assert.That(ByteManipulator.ReadBit(value, 2), Is.False);
    }
    [Test]
    public void TestBitWriteRedLight2()
    {
        WriteBit(ref value, 2, false);
        Assert.That(ByteManipulator.ReadBit(value, 2), Is.Not.True);
    }
    [SetUp]
    public void BitValueSetup()
    {
        value = 1 | (1 << 3) | (1 << 6) | (1 << 7);
        arrValue = new byte[]
        {
            (1 | (1 << 3) | (1 << 6) | (1 << 7)),
            (1 | (1 << 1) | (1 << 2) | (1 << 3)),
            0,
            byte.MaxValue,
            1
        };
    }
}