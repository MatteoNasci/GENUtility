﻿using System.Collections.Generic;
using System.Text;
using System;
namespace GENUtility
{
    /// <summary>
    /// Utility class
    /// </summary>
    public static class ByteManipulator
    {
        /// <summary>
        /// Integer with first right 8 bits on. Value = 255;
        /// </summary>
        public const int FullByte = 0xFF;
        /// <summary>
        /// Byte with first left bit on. Value = 128;
        /// </summary>
        public const byte LastBitOnByte = 1 << 7;
        /// <summary>
        /// Byte with first right bit on. Value = 1;
        /// </summary>
        public const byte FirstBitOnByte = 1;
        /// <summary>
        /// Byte size in bits. Value = 8
        /// </summary>
        public const int ByteSize = 8;

        /// <summary>
        /// Writes the given values into the output array. The read of a number of elements superior to the cyclic buffer total length is not supported
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="cyclicBuffer">array from which to get elements, it is considered a cyclic array</param>
        /// <param name="bufferStartIndex">buffer start index</param>
        /// <param name="output">output array to write</param>
        /// <param name="outputStartIndex">output start index</param>
        /// <param name="count">amount of elements to write</param>
        /// <param name="bufferNewOffset">new buffer start index</param>
        public static void WriteFromCycle<T>(T[] cyclicBuffer, int bufferStartIndex, T[] output, int outputStartIndex, int count, out int bufferNewOffset)
        {
            int length = cyclicBuffer.Length;
            int firstPart = length - bufferStartIndex;
            if (firstPart < count)
            {
                Array.Copy(cyclicBuffer, bufferStartIndex, output, outputStartIndex, firstPart);
                Array.Copy(cyclicBuffer, 0, output, outputStartIndex + firstPart, count - firstPart);
                bufferNewOffset = count - firstPart;
            }
            else
            {
                Array.Copy(cyclicBuffer, bufferStartIndex, output, outputStartIndex, count);
                bufferNewOffset = bufferStartIndex + count;
            }
        }
        /// <summary>
        /// Writes the given values into the output array. The write of a number of elements superior to the cyclic buffer total length is not supported
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="buffer">array from which to get elements</param>
        /// <param name="bufferStartIndex">buffer start index</param>
        /// <param name="cyclicOutput">output array to write, it is considered a cyclic array</param>
        /// <param name="outputStartIndex">output start index</param>
        /// <param name="count">amount of elements to write</param>
        /// <param name="outputNewOffset">new output start index</param>
        public static void WriteToCycle<T>(T[] buffer, int bufferStartIndex, T[] cyclicOutput, int outputStartIndex, int count, out int outputNewOffset)
        {
            int length = cyclicOutput.Length;
            int firstPart = length - outputStartIndex;
            if (firstPart < count)
            {
                Array.Copy(buffer, bufferStartIndex, cyclicOutput, outputStartIndex, firstPart);
                Array.Copy(buffer, bufferStartIndex + firstPart, cyclicOutput, 0, count - firstPart);
                outputNewOffset = count - firstPart;
            }
            else
            {
                Array.Copy(buffer, bufferStartIndex, cyclicOutput, outputStartIndex, count);
                outputNewOffset = outputStartIndex + count;
            }
        }
        /// <summary>
        /// Writes the given values into the output array
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="buffer">array from which to get elements</param>
        /// <param name="bufferStartIndex">buffer start index</param>
        /// <param name="output">output array to write</param>
        /// <param name="outputStartIndex">output start index</param>
        /// <param name="count">amount of elements to write</param>
        public static void Write<T>(T[] buffer, int bufferStartIndex, T[] output, int outputStartIndex, int count)
        {
            Array.Copy(buffer, bufferStartIndex, output, outputStartIndex, count);
        }
        #region ByteConversion
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="startIndex">array index from which to start writing</param>
        /// <param name="value">value to write</param>
        /// <param name="encoder">encoder to use</param>
        /// <returns>Number of bytes used to store the given string starting from startIndex</returns>
        public static int Write(byte[] buffer, int startIndex, string value, Encoding encoder)
        {
            int n = encoder.GetBytes(value, 0, value.Length, buffer, startIndex + sizeof(int));
            Write(buffer, startIndex, n);
            return n + sizeof(int);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <param name="encoder">encoder to use</param>
        /// <param name="count">Number of bytes used to store the given string starting from startIndex</param>
        /// <returns>value read</returns>
        public static string ReadString(byte[] buffer, int startIndex, Encoding encoder, out int count)
        {
            int n = ReadInt32(buffer, startIndex);
            count = n + sizeof(int);
            return encoder.GetString(buffer, startIndex + sizeof(int), n);
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="startIndex">array index from which to start writing</param>
        /// <param name="value">value to write</param>
        /// <param name="encoder">encoder to use</param>
        /// <returns>Number of bytes used to store the given string starting from startIndex</returns>
        public static int Write(byte[] buffer, int startIndex, char[] value, Encoding encoder)
        {
            int bytesWritten = encoder.GetBytes(value, 0, value.Length, buffer, startIndex + sizeof(int));
            Write(buffer, startIndex, bytesWritten);
            return bytesWritten + sizeof(int);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <param name="output">output char array</param>
        /// <param name="outputOffset">output char array offset</param>
        /// <param name="encoder">encoder to use</param>
        /// <param name="byteCount">Number of bytes used to store the given string starting from startIndex</param>
        /// <param name="charCount">Number of chars recovered from buffer</param>
        public static void ReadChars(byte[] buffer, int startIndex, char[] output, int outputOffset, Encoding encoder, out int byteCount, out int charCount)
        {
            int n = ReadInt32(buffer, startIndex);
            byteCount = n + sizeof(int);
            charCount = encoder.GetChars(buffer, startIndex + sizeof(int), n, output, outputOffset);
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="startIndex">array index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(byte[] buffer, int startIndex, uint value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
            buffer[startIndex + 2] = (byte)(value >> 16 & FullByte);
            buffer[startIndex + 3] = (byte)(value >> 24 & FullByte);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static uint ReadUInt32(byte[] buffer, int startIndex)
        {
            return (uint)(buffer[startIndex] | buffer[startIndex + 1] << 8 | buffer[startIndex + 2] << 16 | buffer[startIndex + 3] << 24);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static uint ReadUInt32(List<byte> buffer, int startIndex)
        {
            return (uint)(buffer[startIndex] | buffer[startIndex + 1] << 8 | buffer[startIndex + 2] << 16 | buffer[startIndex + 3] << 24);
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="startIndex">array index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(byte[] buffer, int startIndex, int value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
            buffer[startIndex + 2] = (byte)(value >> 16 & FullByte);
            buffer[startIndex + 3] = (byte)(value >> 24 & FullByte);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static int ReadInt32(byte[] buffer, int startIndex)
        {
            return (buffer[startIndex] | buffer[startIndex + 1] << 8 | buffer[startIndex + 2] << 16 | buffer[startIndex + 3] << 24);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static int ReadInt32(List<byte> buffer, int startIndex)
        {
            return (buffer[startIndex] | buffer[startIndex + 1] << 8 | buffer[startIndex + 2] << 16 | buffer[startIndex + 3] << 24);
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="index">array index to write on</param>
        /// <param name="value">value to write</param>
        public static void Write(byte[] buffer, int index, byte value)
        {
            buffer[index] = value;
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static byte ReadByte(byte[] buffer, int startIndex)
        {
            return buffer[startIndex];
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static byte ReadByte(List<byte> buffer, int startIndex)
        {
            return buffer[startIndex];
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="index">array index to write on</param>
        /// <param name="value">value to write</param>
        public static unsafe void Write(byte[] buffer, int index, sbyte value)
        {
            byte b = *((byte*)&value);
            Write(buffer, index, b);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static unsafe sbyte ReadSByte(byte[] buffer, int startIndex)
        {
            byte value = buffer[startIndex];
            return *((sbyte*)&value);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static unsafe sbyte ReadSByte(List<byte> buffer, int startIndex)
        {
            byte value = buffer[startIndex];
            return *((sbyte*)&value);
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="index">array index to write on</param>
        /// <param name="value">value to write</param>
        public static void Write(byte[] buffer, int index, bool value)
        {
            buffer[index] = (byte)(value ? 1 : 0);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static bool ReadBoolean(byte[] buffer, int startIndex)
        {
            return buffer[startIndex] == 1;
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static bool ReadBoolean(List<byte> buffer, int startIndex)
        {
            return buffer[startIndex] == 1;
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="startIndex">array index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(byte[] buffer, int startIndex, short value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static short ReadInt16(byte[] buffer, int startIndex)
        {
            return (short)(buffer[startIndex] | buffer[startIndex + 1] << 8);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static short ReadInt16(List<byte> buffer, int startIndex)
        {
            return (short)(buffer[startIndex] | buffer[startIndex + 1] << 8);
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="startIndex">array index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(byte[] buffer, int startIndex, char value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static char ReadChar(byte[] buffer, int startIndex)
        {
            return (char)(buffer[startIndex] | buffer[startIndex + 1] << 8);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static char ReadChar(List<byte> buffer, int startIndex)
        {
            return (char)(buffer[startIndex] | buffer[startIndex + 1] << 8);
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="startIndex">array index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(byte[] buffer, int startIndex, ushort value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static ushort ReadUInt16(byte[] buffer, int startIndex)
        {
            return (ushort)(buffer[startIndex] | buffer[startIndex + 1] << 8);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static ushort ReadUInt16(List<byte> buffer, int startIndex)
        {
            return (ushort)(buffer[startIndex] | buffer[startIndex + 1] << 8);
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="startIndex">array index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(byte[] buffer, int startIndex, ulong value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
            buffer[startIndex + 2] = (byte)(value >> 16 & FullByte);
            buffer[startIndex + 3] = (byte)(value >> 24 & FullByte);
            buffer[startIndex + 4] = (byte)(value >> 32 & FullByte);
            buffer[startIndex + 5] = (byte)(value >> 40 & FullByte);
            buffer[startIndex + 6] = (byte)(value >> 48 & FullByte);
            buffer[startIndex + 7] = (byte)(value >> 56 & FullByte);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static ulong ReadUInt64(byte[] buffer, int startIndex)
        {
            return ((ulong)buffer[startIndex] | (ulong)buffer[startIndex + 1] << 8 | (ulong)buffer[startIndex + 2] << 16 | (ulong)buffer[startIndex + 3] << 24 | (ulong)buffer[startIndex + 4] << 32 | (ulong)buffer[startIndex + 5] << 40 | (ulong)buffer[startIndex + 6] << 48 | (ulong)buffer[startIndex + 7] << 56);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static ulong ReadUInt64(List<byte> buffer, int startIndex)
        {
            return ((ulong)buffer[startIndex] | (ulong)buffer[startIndex + 1] << 8 | (ulong)buffer[startIndex + 2] << 16 | (ulong)buffer[startIndex + 3] << 24 | (ulong)buffer[startIndex + 4] << 32 | (ulong)buffer[startIndex + 5] << 40 | (ulong)buffer[startIndex + 6] << 48 | (ulong)buffer[startIndex + 7] << 56);
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="startIndex">array index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(byte[] buffer, int startIndex, long value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
            buffer[startIndex + 2] = (byte)(value >> 16 & FullByte);
            buffer[startIndex + 3] = (byte)(value >> 24 & FullByte);
            buffer[startIndex + 4] = (byte)(value >> 32 & FullByte);
            buffer[startIndex + 5] = (byte)(value >> 40 & FullByte);
            buffer[startIndex + 6] = (byte)(value >> 48 & FullByte);
            buffer[startIndex + 7] = (byte)(value >> 56 & FullByte);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static long ReadInt64(byte[] buffer, int startIndex)
        {
            return ((long)buffer[startIndex] | (long)buffer[startIndex + 1] << 8 | (long)buffer[startIndex + 2] << 16 | (long)buffer[startIndex + 3] << 24 | (long)buffer[startIndex + 4] << 32 | (long)buffer[startIndex + 5] << 40 | (long)buffer[startIndex + 6] << 48 | (long)buffer[startIndex + 7] << 56);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static long ReadInt64(List<byte> buffer, int startIndex)
        {
            return ((long)buffer[startIndex] | (long)buffer[startIndex + 1] << 8 | (long)buffer[startIndex + 2] << 16 | (long)buffer[startIndex + 3] << 24 | (long)buffer[startIndex + 4] << 32 | (long)buffer[startIndex + 5] << 40 | (long)buffer[startIndex + 6] << 48 | (long)buffer[startIndex + 7] << 56);
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="startIndex">array index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static unsafe void Write(byte[] buffer, int startIndex, float value)
        {
            uint val = *((uint*)&value);
            Write(buffer, startIndex, val);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static unsafe float ReadSingle(byte[] buffer, int startIndex)
        {
            uint v = (uint)(buffer[startIndex] | buffer[startIndex + 1] << 8 | buffer[startIndex + 2] << 16 | buffer[startIndex + 3] << 24);
            return *((float*)&v);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static unsafe float ReadSingle(List<byte> buffer, int startIndex)
        {
            uint v = (uint)(buffer[startIndex] | buffer[startIndex + 1] << 8 | buffer[startIndex + 2] << 16 | buffer[startIndex + 3] << 24);
            return *((float*)&v);
        }
        /// <summary>
        /// Writes the given value in the given byte array
        /// </summary>
        /// <param name="buffer">byte array to write on</param>
        /// <param name="startIndex">array index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static unsafe void Write(byte[] buffer, int startIndex, double value)
        {
            ulong val = *((ulong*)&value);
            Write(buffer, startIndex, val);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static unsafe double ReadDouble(byte[] buffer, int startIndex)
        {
            ulong v = ((ulong)buffer[startIndex] | (ulong)buffer[startIndex + 1] << 8 | (ulong)buffer[startIndex + 2] << 16 | (ulong)buffer[startIndex + 3] << 24 | (ulong)buffer[startIndex + 4] << 32 | (ulong)buffer[startIndex + 5] << 40 | (ulong)buffer[startIndex + 6] << 48 | (ulong)buffer[startIndex + 7] << 56);
            return *((double*)&v);
        }
        /// <summary>
        /// Reads a value from the given byte array
        /// </summary>
        /// <param name="buffer">byte array to read from</param>
        /// <param name="startIndex">array index from which to start reading</param>
        /// <returns>value read</returns>
        public static unsafe double ReadDouble(List<byte> buffer, int startIndex)
        {
            ulong v = ((ulong)buffer[startIndex] | (ulong)buffer[startIndex + 1] << 8 | (ulong)buffer[startIndex + 2] << 16 | (ulong)buffer[startIndex + 3] << 24 | (ulong)buffer[startIndex + 4] << 32 | (ulong)buffer[startIndex + 5] << 40 | (ulong)buffer[startIndex + 6] << 48 | (ulong)buffer[startIndex + 7] << 56);
            return *((double*)&v);
        }
        /// <summary>
        /// Writes the given value in the given byte list
        /// </summary>
        /// <param name="buffer">byte list to write on</param>
        /// <param name="startIndex">list index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(List<byte> buffer, int startIndex, uint value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
            buffer[startIndex + 2] = (byte)(value >> 16 & FullByte);
            buffer[startIndex + 3] = (byte)(value >> 24 & FullByte);
        }
        /// <summary>
        /// Writes the given value in the given byte list
        /// </summary>
        /// <param name="buffer">byte list to write on</param>
        /// <param name="startIndex">list index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(List<byte> buffer, int startIndex, int value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
            buffer[startIndex + 2] = (byte)(value >> 16 & FullByte);
            buffer[startIndex + 3] = (byte)(value >> 24 & FullByte);
        }
        /// <summary>
        /// Writes the given value in the given byte list
        /// </summary>
        /// <param name="buffer">byte list to write on</param>
        /// <param name="index">list index to write on</param>
        /// <param name="value">value to write</param>
        public static void Write(List<byte> buffer, int index, byte value)
        {
            buffer[index] = value;
        }
        /// <summary>
        /// Writes the given value in the given byte list
        /// </summary>
        /// <param name="buffer">byte list to write on</param>
        /// <param name="index">list index to write on</param>
        /// <param name="value">value to write</param>
        public static unsafe void Write(List<byte> buffer, int index, sbyte value)
        {
            byte b = *((byte*)&value);
            Write(buffer, index, b);
        }
        /// <summary>
        /// Writes the given value in the given byte list
        /// </summary>
        /// <param name="buffer">byte list to write on</param>
        /// <param name="index">list index to write on</param>
        /// <param name="value">value to write</param>
        public static void Write(List<byte> buffer, int index, bool value)
        {
            buffer[index] = (byte)(value ? 1 : 0);
        }
        /// <summary>
        /// Writes the given value in the given byte list
        /// </summary>
        /// <param name="buffer">byte list to write on</param>
        /// <param name="startIndex">list index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(List<byte> buffer, int startIndex, short value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
        }
        /// <summary>
        /// Writes the given value in the given byte list
        /// </summary>
        /// <param name="buffer">byte list to write on</param>
        /// <param name="startIndex">list index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(List<byte> buffer, int startIndex, char value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
        }
        /// <summary>
        /// Writes the given value in the given byte list
        /// </summary>
        /// <param name="buffer">byte list to write on</param>
        /// <param name="startIndex">list index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(List<byte> buffer, int startIndex, ushort value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
        }
        /// <summary>
        /// Writes the given value in the given byte list
        /// </summary>
        /// <param name="buffer">byte list to write on</param>
        /// <param name="startIndex">list index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(List<byte> buffer, int startIndex, ulong value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
            buffer[startIndex + 2] = (byte)(value >> 16 & FullByte);
            buffer[startIndex + 3] = (byte)(value >> 24 & FullByte);
            buffer[startIndex + 4] = (byte)(value >> 32 & FullByte);
            buffer[startIndex + 5] = (byte)(value >> 40 & FullByte);
            buffer[startIndex + 6] = (byte)(value >> 48 & FullByte);
            buffer[startIndex + 7] = (byte)(value >> 56 & FullByte);
        }
        /// <summary>
        /// Writes the given value in the given byte list
        /// </summary>
        /// <param name="buffer">byte list to write on</param>
        /// <param name="startIndex">list index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static void Write(List<byte> buffer, int startIndex, long value)
        {
            buffer[startIndex] = (byte)(value & FullByte);
            buffer[startIndex + 1] = (byte)(value >> 8 & FullByte);
            buffer[startIndex + 2] = (byte)(value >> 16 & FullByte);
            buffer[startIndex + 3] = (byte)(value >> 24 & FullByte);
            buffer[startIndex + 4] = (byte)(value >> 32 & FullByte);
            buffer[startIndex + 5] = (byte)(value >> 40 & FullByte);
            buffer[startIndex + 6] = (byte)(value >> 48 & FullByte);
            buffer[startIndex + 7] = (byte)(value >> 56 & FullByte);
        }
        /// <summary>
        /// Writes the given value in the given byte list
        /// </summary>
        /// <param name="buffer">byte list to write on</param>
        /// <param name="startIndex">list index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static unsafe void Write(List<byte> buffer, int startIndex, float value)
        {
            uint val = *((uint*)&value);
            Write(buffer, startIndex, val);
        }
        /// <summary>
        /// Writes the given value in the given byte list
        /// </summary>
        /// <param name="buffer">byte list to write on</param>
        /// <param name="startIndex">list index from which to start writing</param>
        /// <param name="value">value to write</param>
        public static unsafe void Write(List<byte> buffer, int startIndex, double value)
        {
            ulong val = *((ulong*)&value);
            Write(buffer, startIndex, val);
        }
        #endregion //ByteConversion
        #region BitConversion
        public static bool ReadBit(byte[] toRead, int startIndex, int bitStartIndex)
        {
            return ((LastBitOnByte >> bitStartIndex) & toRead[startIndex]) != 0;
        }
        public static bool ReadBit(byte[] toRead, int bitStartIndex)
        {
            return ((LastBitOnByte >> (bitStartIndex % ByteSize)) & toRead[bitStartIndex / ByteSize]) != 0;
        }
        public static void WriteBit(byte[] toWrite, int startIndex, int bitStartIndex, bool value)
        {
            toWrite[startIndex] = (byte)(value ? toWrite[startIndex] | (LastBitOnByte >> bitStartIndex) : toWrite[startIndex] & ~(LastBitOnByte >> bitStartIndex));
        }
        public static void WriteBit(byte[] toWrite, int bitStartIndex, bool value)
        {
            int index = bitStartIndex / ByteSize;
            toWrite[index] = (byte)(value ? toWrite[index] | (LastBitOnByte >> (bitStartIndex % ByteSize)) : toWrite[index] & ~(LastBitOnByte >> (bitStartIndex % ByteSize)));
        }
        public static bool ReadBit(List<byte> toRead, int startIndex, int bitStartIndex)
        {
            return ((LastBitOnByte >> bitStartIndex) & toRead[startIndex]) != 0;
        }
        public static bool ReadBit(List<byte> toRead, int bitStartIndex)
        {
            return ((LastBitOnByte >> (bitStartIndex % ByteSize)) & toRead[bitStartIndex / ByteSize]) != 0;
        }
        public static void WriteBit(List<byte> toWrite, int startIndex, int bitStartIndex, bool value)
        {
            toWrite[startIndex] = (byte)(value ? toWrite[startIndex] | (LastBitOnByte >> bitStartIndex) : toWrite[startIndex] & ~(LastBitOnByte >> bitStartIndex));
        }
        public static void WriteBit(List<byte> toWrite, int bitStartIndex, bool value)
        {
            int index = bitStartIndex / ByteSize;
            toWrite[index] = (byte)(value ? toWrite[index] | (LastBitOnByte >> (bitStartIndex % ByteSize)) : toWrite[index] & ~(LastBitOnByte >> (bitStartIndex % ByteSize)));
        }


        public static void ReadBits(byte[] toRead, int startIndex, int bitStartIndex, int length, bool[] output, int outputStartIndex)
        {
            int bitIndex = bitStartIndex % ByteSize;
            startIndex += bitStartIndex / ByteSize;
            for (int i = 0; i < length; i++, bitIndex++)
            {
                if (bitIndex >= ByteSize)
                {
                    bitIndex -= ByteSize;
                    startIndex++;
                }
                output[i + outputStartIndex] = ((LastBitOnByte >> bitIndex) & toRead[startIndex]) != 0;
            }
        }
        public static void ReadBits(byte[] toRead, int bitStartIndex, int length, bool[] output, int outputStartIndex)
        {
            int bitIndex = bitStartIndex % ByteSize;
            int startIndex = bitStartIndex / ByteSize;
            for (int i = 0; i < length; i++, bitIndex++)
            {
                if (bitIndex >= ByteSize)
                {
                    bitIndex -= ByteSize;
                    startIndex++;
                }
                output[i + outputStartIndex] = ((LastBitOnByte >> bitIndex) & toRead[startIndex]) != 0;
            }
        }
        public static void WriteBits(byte[] toWrite, int startIndex, int bitStartIndex, int length, bool[] input, int inputStartIndex)
        {
            int bitIndex = bitStartIndex % ByteSize;
            startIndex += bitStartIndex / ByteSize;
            for (int i = 0; i < length; i++, bitIndex++)
            {
                if (bitIndex >= ByteSize)
                {
                    bitIndex -= ByteSize;
                    startIndex++;
                }
                toWrite[startIndex] = (byte)(input[i + inputStartIndex] ? toWrite[startIndex] | (LastBitOnByte >> bitIndex) : toWrite[startIndex] & ~(LastBitOnByte >> bitIndex));
            }
        }
        public static void WriteBits(byte[] toWrite, int bitStartIndex, int length, bool[] input, int inputStartIndex)
        {
            int bitIndex = bitStartIndex % ByteSize;
            int startIndex = bitStartIndex / ByteSize;
            for (int i = 0; i < length; i++, bitIndex++)
            {
                if (bitIndex >= ByteSize)
                {
                    bitIndex -= ByteSize;
                    startIndex++;
                }
                toWrite[startIndex] = (byte)(input[i + inputStartIndex] ? toWrite[startIndex] | (LastBitOnByte >> bitIndex) : toWrite[startIndex] & ~(LastBitOnByte >> bitIndex));
            }
        }
        public static void ReadBits(byte[] toRead, int startIndex, int bitStartIndex, int length, List<bool> output, int outputStartIndex)
        {
            int bitIndex = bitStartIndex % ByteSize;
            startIndex += bitStartIndex / ByteSize;
            for (int i = 0; i < length; i++, bitIndex++)
            {
                if (bitIndex >= ByteSize)
                {
                    bitIndex -= ByteSize;
                    startIndex++;
                }
                output[i + outputStartIndex] = ((LastBitOnByte >> bitIndex) & toRead[startIndex]) != 0;
            }
        }
        public static void ReadBits(byte[] toRead, int bitStartIndex, int length, List<bool> output, int outputStartIndex)
        {
            int bitIndex = bitStartIndex % ByteSize;
            int startIndex = bitStartIndex / ByteSize;
            for (int i = 0; i < length; i++, bitIndex++)
            {
                if (bitIndex >= ByteSize)
                {
                    bitIndex -= ByteSize;
                    startIndex++;
                }
                output[i + outputStartIndex] = ((LastBitOnByte >> bitIndex) & toRead[startIndex]) != 0;
            }
        }
        public static void WriteBits(byte[] toWrite, int startIndex, int bitStartIndex, int length, List<bool> input, int inputStartIndex)
        {
            int bitIndex = bitStartIndex % ByteSize;
            startIndex += bitStartIndex / ByteSize;
            for (int i = 0; i < length; i++, bitIndex++)
            {
                if (bitIndex >= ByteSize)
                {
                    bitIndex -= ByteSize;
                    startIndex++;
                }
                toWrite[startIndex] = (byte)(input[i + inputStartIndex] ? toWrite[startIndex] | (LastBitOnByte >> bitIndex) : toWrite[startIndex] & ~(LastBitOnByte >> bitIndex));
            }
        }
        public static void WriteBits(byte[] toWrite, int bitStartIndex, int length, List<bool> input, int inputStartIndex)
        {
            int bitIndex = bitStartIndex % ByteSize;
            int startIndex = bitStartIndex / ByteSize;
            for (int i = 0; i < length; i++, bitIndex++)
            {
                if (bitIndex >= ByteSize)
                {
                    bitIndex -= ByteSize;
                    startIndex++;
                }
                toWrite[startIndex] = (byte)(input[i + inputStartIndex] ? toWrite[startIndex] | (LastBitOnByte >> bitIndex) : toWrite[startIndex] & ~(LastBitOnByte >> bitIndex));
            }
        }


        public static bool ReadBit(byte toRead, int bitStartIndex)
        {
            return ((LastBitOnByte >> bitStartIndex) & toRead) != 0;
        }
        public static void WriteBit(ref byte toWrite, int bitStartIndex, bool value)
        {
            toWrite = (byte)(value ? toWrite | (LastBitOnByte >> bitStartIndex) : toWrite & ~(LastBitOnByte >> bitStartIndex));
        }
        public static void ReadBits(byte toRead, int bitStartIndex, int length, bool[] output, int outPutStartIndex)
        {
            for (int i = 0; i < length; i++)
            {
                output[i + outPutStartIndex] = ((LastBitOnByte >> (bitStartIndex + i)) & toRead) != 0;
            }
        }
        public static void ReadBits(byte toRead, int bitStartIndex, int length, List<bool> output, int outPutStartIndex)
        {
            for (int i = 0; i < length; i++)
            {
                output[i + outPutStartIndex] = ((LastBitOnByte >> (bitStartIndex + i)) & toRead) != 0;
            }
        }
        public static void WriteBits(byte toWrite, int bitStartIndex, int length, bool[] input, int inputStartIndex)
        {
            for (int i = 0; i < length; i++)
            {
                toWrite = (byte)(input[i + inputStartIndex] ? toWrite | (LastBitOnByte >> (bitStartIndex + i)) : toWrite & ~(LastBitOnByte >> (bitStartIndex + 1)));
            }
        }
        public static void WriteBits(byte toWrite, int bitStartIndex, int length, List<bool> input, int inputStartIndex)
        {
            for (int i = 0; i < length; i++)
            {
                toWrite = (byte)(input[i + inputStartIndex] ? toWrite | (LastBitOnByte >> (bitStartIndex + i)) : toWrite & ~(LastBitOnByte >> (bitStartIndex + 1)));
            }
        }

        public static byte TranformFromBitsByte()
        {
            throw new NotImplementedException();
        }
        public static void TranformToBits(byte value)
        {
            throw new NotImplementedException();
        }
        public static byte TranformFromBitByte()
        {
            throw new NotImplementedException();
        }
        public static bool TranformToBit(byte value)
        {
            throw new NotImplementedException();
        }
        public static uint TranformFromBitsUInt32()
        {
            throw new NotImplementedException();
        }
        public static void TranformToBits(uint value)
        {
            throw new NotImplementedException();
        }
        public static uint TranformFromBitUInt32()
        {
            throw new NotImplementedException();
        }
        public static bool TranformToBit(uint value)
        {
            throw new NotImplementedException();
        }
        public static ushort TranformFromBitsUInt16()
        {
            throw new NotImplementedException();
        }
        public static void TranformToBits(ushort value)
        {
            throw new NotImplementedException();
        }
        public static ushort TranformFromBitUInt16()
        {
            throw new NotImplementedException();
        }
        public static bool TranformToBit(ushort value)
        {
            throw new NotImplementedException();
        }
        public static ulong TranformFromBitsUInt64()
        {
            throw new NotImplementedException();
        }
        public static void TranformToBits(ulong value)
        {
            throw new NotImplementedException();
        }
        public static ulong TranformFromBitUInt64()
        {
            throw new NotImplementedException();
        }
        public static bool TranformToBit(ulong value)
        {
            throw new NotImplementedException();
        }
        public static float TranformFromBitsSingle()
        {
            throw new NotImplementedException();
        }
        public static void TranformToBits(float value)
        {
            throw new NotImplementedException();
        }
        public static float TranformFromBitSingle()
        {
            throw new NotImplementedException();
        }
        public static bool TranformToBit(float value)
        {
            throw new NotImplementedException();
        }
        public static sbyte TranformFromBitsSByte()
        {
            throw new NotImplementedException();
        }
        public static void TranformToBits(sbyte value)
        {
            throw new NotImplementedException();
        }
        public static sbyte TranformFromBitSByte()
        {
            throw new NotImplementedException();
        }
        public static bool TranformToBit(sbyte value)
        {
            throw new NotImplementedException();
        }
        public static int TranformFromBitsInt32()
        {
            throw new NotImplementedException();
        }
        public static void TranformToBits(int value)
        {
            throw new NotImplementedException();
        }
        public static int TranformFromBitInt32()
        {
            throw new NotImplementedException();
        }
        public static bool TranformToBit(int value)
        {
            throw new NotImplementedException();
        }
        public static short TranformFromBitsInt16()
        {
            throw new NotImplementedException();
        }
        public static void TranformToBits(short value)
        {
            throw new NotImplementedException();
        }
        public static short TranformFromBitInt16()
        {
            throw new NotImplementedException();
        }
        public static bool TranformToBit(short value)
        {
            throw new NotImplementedException();
        }
        public static long TranformFromBitsInt64()
        {
            throw new NotImplementedException();
        }
        public static void TranformToBits(long value)
        {
            throw new NotImplementedException();
        }
        public static long TranformFromBitInt64()
        {
            throw new NotImplementedException();
        }
        public static bool TranformToBit(long value)
        {
            throw new NotImplementedException();
        }
        public static double TranformFromBitsDouble()
        {
            throw new NotImplementedException();
        }
        public static void TranformToBits(double value)
        {
            throw new NotImplementedException();
        }
        public static double TranformFromBitDouble()
        {
            throw new NotImplementedException();
        }
        public static bool TranformToBit(double value)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}