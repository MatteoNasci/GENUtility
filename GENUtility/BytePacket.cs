using System.Text;
namespace GENUtility
{
    /// <summary>
    /// Class that manages a packet data. 
    /// </summary>
    public class BytePacket
    {
        /// <summary>
        /// Amount of bytes that has been written in the packet. This may not return a correct value if already written data in the packet has been overwritten or if methods other than the given Write methods in this class has been used to modify the internal buffer, and it must be reset manually. GamePackets created through CreatePacket will be already resetted. This value has no impact in the internal logic of GamePacket and should be used as a way to store the current amount of written byte data
        /// </summary>
        public int CurrentLength;
        /// <summary>
        /// Current seek position in the packet, used for all Read/Write operations
        /// </summary>
        public int CurrentSeek;
        /// <summary>
        /// Max capacity of the packet. Write/Read operations that go beyond this limit will throw exceptions
        /// </summary>
        public int MaxCapacity { get { return Data.Length; } }
        /// <summary>
        /// Internal buffer used by the packet Changes done to the buffer directly will not automatically modify the other variables
        /// </summary>
        public byte[] Data;

        /// <summary>
        /// Copies n elements from internal buffer of the given gamepacket starting from its seek position to the current instance. Elements copied are equal to the minimum between the packet to (copy current length - seek pos), the space effectively available starting from vopy seek pos and the space available in the current instance buffer.
        /// </summary>
        /// <param name="toCopy">gamepacket to copy from</param>
        /// <param name="elementsCopied">effective number of elements copied successfully</param>
        public void Copy(BytePacket toCopy, out int elementsCopied)
        {
            int v1 = Data.Length - CurrentSeek;
            int v2 = toCopy.CurrentLength - toCopy.CurrentSeek;
            int v3 = toCopy.Data.Length - toCopy.CurrentSeek;
            elementsCopied = v1 > v2 ? v2 : (v3 > v1 ? v1 : v3);

            WriteByteData(toCopy, elementsCopied);
        }
        /// <summary>
        /// Reads the internal buffer into the given array
        /// </summary>
        /// <param name="buffer">buffer on which the internal buffer elements will be written</param>
        /// <param name="bufferOffset">buffer offset</param>
        /// <param name="dataOffset">internal buffer offset</param>
        /// <param name="lengthToRead">number of bytes to read</param>
        public void ReadByteData(byte[] buffer, int bufferOffset, int dataOffset, int lengthToRead)
        {
            CurrentSeek = dataOffset;
            ReadByteData(buffer, bufferOffset, lengthToRead);
        }
        /// <summary>
        /// Reads the internal buffer into the given array
        /// </summary>
        /// <param name="buffer">buffer on which the internal buffer elements will be written. Buffer Seek will not be moved</param>
        /// <param name="bufferOffset">buffer offset</param>
        /// <param name="dataOffset">internal buffer offset</param>
        /// <param name="lengthToRead">number of bytes to read</param>
        public void ReadByteData(BytePacket buffer, int bufferOffset, int dataOffset, int lengthToRead)
        {
            CurrentSeek = dataOffset;
            buffer.CurrentSeek = bufferOffset;
            ReadByteData(buffer, lengthToRead);
        }
        /// <summary>
        /// Reads the internal buffer into the given array
        /// </summary>
        /// <param name="buffer">buffer on which the internal buffer elements will be written. Buffer Seek will not be moved</param>
        /// <param name="lengthToRead">number of bytes to read</param>
        public void ReadByteData(BytePacket buffer, int lengthToRead)
        {
            ReadByteData(buffer.Data, buffer.CurrentSeek, lengthToRead);
            buffer.CurrentSeek += lengthToRead;
            buffer.CurrentLength += lengthToRead;
        }
        /// <summary>
        /// Reads the internal buffer into the given array
        /// </summary>
        /// <param name="buffer">buffer on which the internal buffer elements will be written</param>
        /// <param name="bufferOffset">buffer offset</param>
        /// <param name="lengthToRead">number of bytes to read</param>
        public void ReadByteData(byte[] buffer, int bufferOffset, int lengthToRead)
        {
            ByteManipulator.Write(Data, CurrentSeek, buffer, bufferOffset, lengthToRead);
            CurrentSeek += lengthToRead;
        }
        /// <summary>
        /// Writes the given byte array into the internal buffer
        /// </summary>
        /// <param name="buffer">buffer from which elements will be written on the internal buffer</param>
        /// <param name="bufferOffset">buffer offset</param>
        /// <param name="lengthToWrite">number of bytes to write</param>
        public void WriteByteData(byte[] buffer, int bufferOffset, int lengthToWrite)
        {
            ByteManipulator.Write(buffer, bufferOffset, Data, CurrentSeek, lengthToWrite);
            CurrentLength += lengthToWrite;
            CurrentSeek += lengthToWrite;
        }
        /// <summary>
        /// Writes the given byte array into the internal buffer
        /// </summary>
        /// <param name="buffer">buffer from which elements will be written on the internal buffer</param>
        /// <param name="bufferOffset">buffer offset</param>
        /// <param name="dataOffset">internal buffer offset</param>
        /// <param name="lengthToWrite">number of bytes to write</param>
        public void WriteByteData(byte[] buffer, int bufferOffset, int dataOffset, int lengthToWrite)
        {
            CurrentSeek = dataOffset;
            WriteByteData(buffer, bufferOffset, lengthToWrite);
        }
        /// <summary>
        /// Writes the given byte array into the internal buffer
        /// </summary>
        /// <param name="buffer">buffer from which elements will be written on the internal buffer. Buffer Seek will not be moved</param>
        /// <param name="bufferOffset">buffer offset</param>
        /// <param name="dataOffset">internal buffer offset</param>
        /// <param name="lengthToWrite">number of bytes to write</param>
        public void WriteByteData(BytePacket buffer, int bufferOffset, int dataOffset, int lengthToWrite)
        {
            CurrentSeek = dataOffset;
            buffer.CurrentSeek = bufferOffset;
            WriteByteData(buffer, lengthToWrite);
        }
        /// <summary>
        /// Writes the given byte array into the internal buffer
        /// </summary>
        /// <param name="buffer">buffer from which elements will be written on the internal buffer. Buffer Seek will not be moved</param>
        /// <param name="lengthToWrite">number of bytes to write</param>
        public void WriteByteData(BytePacket buffer, int lengthToWrite)
        {
            WriteByteData(buffer.Data, buffer.CurrentSeek, lengthToWrite);
            buffer.CurrentSeek += lengthToWrite;
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="values">values to write</param>
        /// <param name="encoder">encoder to use</param>
        public void Write(char[] values, Encoding encoder)
        {
            int n = ByteManipulator.Write(Data, CurrentSeek, values, encoder);
            CurrentSeek += n;
            CurrentLength += n;
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <param name="values">values to write</param>
        /// <param name="encoder">encoder to use</param>
        public void Write(int offset, char[] values, Encoding encoder)
        {
            CurrentSeek = offset;
            Write(values, encoder);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="encoder">encoder to use</param>
        public void Write(string value, Encoding encoder)
        {
            int n = ByteManipulator.Write(Data, CurrentSeek, value, encoder);

            CurrentSeek += n;
            CurrentLength += n;
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        /// <param name="encoder">encoder to use</param>
        public void Write(string value, int offset, Encoding encoder)
        {
            CurrentSeek = offset;
            Write(value, encoder);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        public void Write(char value)
        {
            ByteManipulator.Write(Data, CurrentSeek, value);
            CurrentSeek += sizeof(char);
            CurrentLength += sizeof(char);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        public void Write(char value, int offset)
        {
            CurrentSeek = offset;
            Write(value);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        public void Write(float value)
        {
            ByteManipulator.Write(Data, CurrentSeek, value);
            CurrentSeek += sizeof(float);
            CurrentLength += sizeof(float);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        public void Write(float value, int offset)
        {
            CurrentSeek = offset;
            Write(value);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        public void Write(double value)
        {
            ByteManipulator.Write(Data, CurrentSeek, value);
            CurrentSeek += sizeof(double);
            CurrentLength += sizeof(double);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        public void Write(double value, int offset)
        {
            CurrentSeek = offset;
            Write(value);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        public void Write(bool value)
        {
            ByteManipulator.Write(Data, CurrentSeek, value);
            CurrentSeek += sizeof(bool);
            CurrentLength += sizeof(bool);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        public void Write(bool value, int offset)
        {
            CurrentSeek = offset;
            Write(value);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        public void Write(short value)
        {
            ByteManipulator.Write(Data, CurrentSeek, value);
            CurrentSeek += sizeof(short);
            CurrentLength += sizeof(short);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        public void Write(short value, int offset)
        {
            CurrentSeek = offset;
            Write(value);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        public void Write(ushort value)
        {
            ByteManipulator.Write(Data, CurrentSeek, value);
            CurrentSeek += sizeof(ushort);
            CurrentLength += sizeof(ushort);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        public void Write(ushort value, int offset)
        {
            CurrentSeek = offset;
            Write(value);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        public void Write(byte value)
        {
            ByteManipulator.Write(Data, CurrentSeek, value);
            CurrentSeek += sizeof(byte);
            CurrentLength += sizeof(byte);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        public void Write(byte value, int offset)
        {
            CurrentSeek = offset;
            Write(value);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        public void Write(int value)
        {
            ByteManipulator.Write(Data, CurrentSeek, value);
            CurrentSeek += sizeof(int);
            CurrentLength += sizeof(int);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        public void Write(int value, int offset)
        {
            CurrentSeek = offset;
            Write(value);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        public void Write(uint value)
        {
            ByteManipulator.Write(Data, CurrentSeek, value);
            CurrentSeek += sizeof(uint);
            CurrentLength += sizeof(uint);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        public void Write(uint value, int offset)
        {
            CurrentSeek = offset;
            Write(value);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        public void Write(long value)
        {
            ByteManipulator.Write(Data, CurrentSeek, value);
            CurrentSeek += sizeof(long);
            CurrentLength += sizeof(long);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        public void Write(long value, int offset)
        {
            CurrentSeek = offset;
            Write(value);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        public void Write(ulong value)
        {
            ByteManipulator.Write(Data, CurrentSeek, value);
            CurrentSeek += sizeof(ulong);
            CurrentLength += sizeof(ulong);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        public void Write(ulong value, int offset)
        {
            CurrentSeek = offset;
            Write(value);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        public void Write(sbyte value)
        {
            ByteManipulator.Write(Data, CurrentSeek, value);
            CurrentSeek += sizeof(sbyte);
            CurrentLength += sizeof(sbyte);
        }
        /// <summary>
        /// Writes the value in the packet
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="offset">move the seek at this offset</param>
        public void Write(sbyte value, int offset)
        {
            CurrentSeek = offset;
            Write(value);
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="out_chars">output char array</param>
        /// <param name="out_charsOffset">output char array offset from which to start writing</param>
        /// <param name="encoder">encoder to use</param>
        /// <returns>total number of char elements</returns>
        public int ReadChars(char[] out_chars, int out_charsOffset, Encoding encoder)
        {
            int charC;
            int byteC;
            ByteManipulator.ReadChars(Data, CurrentSeek, out_chars, out_charsOffset, encoder, out byteC, out charC);
            CurrentSeek += byteC;
            return charC;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <param name="out_chars">output char array</param>
        /// <param name="out_charsOffset">output char array offset from which to start writing</param>
        /// <param name="encoder">encoder to use</param>
        /// <returns>total number of char elements</returns>
        public int ReadChars(int offset, char[] out_chars, int out_charsOffset, Encoding encoder)
        {
            CurrentSeek = offset;
            return ReadChars(out_chars, out_charsOffset, encoder);
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <returns>value</returns>
        public char ReadChar()
        {
            char res = ByteManipulator.ReadChar(Data, CurrentSeek);
            CurrentSeek += sizeof(char);
            return res;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <returns>value</returns>
        public char ReadChar(int offset)
        {
            CurrentSeek = offset;
            return ReadChar();
        }
        /// <summary>
        /// Reads a value from the packet (Causes char array allocation)
        /// </summary>
        /// <param name="encoder">encoder to use</param>
        /// <returns>value</returns>
        public string ReadString(Encoding encoder)
        {
            int n;

            string s = ByteManipulator.ReadString(Data, CurrentSeek, encoder, out n);

            CurrentSeek += n;

            return s;
        }
        /// <summary>
        /// Reads a value from the packet (Causes char array allocation)
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <param name="encoder">encoder to use</param>
        /// <returns>value</returns>
        public string ReadString(int offset, Encoding encoder)
        {
            CurrentSeek = offset;
            return ReadString(encoder);
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <returns>value</returns>
        public float ReadFloat()
        {
            float res = ByteManipulator.ReadSingle(Data, CurrentSeek);
            CurrentSeek += sizeof(float);
            return res;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <returns>value</returns>
        public float ReadFloat(int offset)
        {
            CurrentSeek = offset;
            return ReadFloat();
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <returns>value</returns>
        public double ReadDouble()
        {
            double res = ByteManipulator.ReadDouble(Data, CurrentSeek);
            CurrentSeek += sizeof(double);
            return res;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <returns>value</returns>
        public double ReadDouble(int offset)
        {
            CurrentSeek = offset;
            return ReadDouble();
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <returns>value</returns>
        public short ReadShort()
        {
            short res = ByteManipulator.ReadInt16(Data, CurrentSeek);
            CurrentSeek += sizeof(short);
            return res;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <returns>value</returns>
        public short ReadShort(int offset)
        {
            CurrentSeek = offset;
            return ReadShort();
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <returns>value</returns>
        public ushort ReadUShort()
        {
            ushort res = ByteManipulator.ReadUInt16(Data, CurrentSeek);
            CurrentSeek += sizeof(ushort);
            return res;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <returns>value</returns>
        public ushort ReadUShort(int offset)
        {
            CurrentSeek = offset;
            return ReadUShort();
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <returns>value</returns>
        public int ReadInt()
        {
            int res = ByteManipulator.ReadInt32(Data, CurrentSeek);
            CurrentSeek += sizeof(int);
            return res;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <returns>value</returns>
        public int ReadInt(int offset)
        {
            CurrentSeek = offset;
            return ReadInt();
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <returns>value</returns>
        public uint ReadUInt()
        {
            uint res = ByteManipulator.ReadUInt32(Data, CurrentSeek);
            CurrentSeek += sizeof(uint);
            return res;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <returns>value</returns>
        public uint ReadUInt(int offset)
        {
            CurrentSeek = offset;
            return ReadUInt();
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <returns>value</returns>
        public long ReadLong()
        {
            long res = ByteManipulator.ReadInt64(Data, CurrentSeek);
            CurrentSeek += sizeof(long);
            return res;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <returns>value</returns>
        public long ReadLong(int offset)
        {
            CurrentSeek = offset;
            return ReadLong();
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <returns>value</returns>
        public ulong ReadULong()
        {
            ulong res = ByteManipulator.ReadUInt64(Data, CurrentSeek);
            CurrentSeek += sizeof(ulong);
            return res;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <returns>value</returns>
        public ulong ReadULong(int offset)
        {
            CurrentSeek = offset;
            return ReadULong();
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <returns>value</returns>
        public bool ReadBool()
        {
            bool res = ByteManipulator.ReadBoolean(Data, CurrentSeek);
            CurrentSeek += sizeof(bool);
            return res;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <returns>value</returns>
        public bool ReadBool(int offset)
        {
            CurrentSeek = offset;
            return ReadBool();
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <returns>value</returns>
        public byte ReadByte()
        {
            byte res = Data[CurrentSeek];
            CurrentSeek += sizeof(byte);
            return res;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <returns>value</returns>
        public byte ReadByte(int offset)
        {
            CurrentSeek = offset;
            return ReadByte();
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <returns>value</returns>
        public sbyte ReadSByte()
        {
            sbyte res = ByteManipulator.ReadSByte(Data, CurrentSeek);
            CurrentSeek += sizeof(sbyte);
            return res;
        }
        /// <summary>
        /// Reads a value from the packet
        /// </summary>
        /// <param name="offset">move the seek at this offset</param>
        /// <returns>value</returns>
        public sbyte ReadSByte(int offset)
        {
            CurrentSeek = offset;
            return ReadSByte();
        }
        /// <summary>
        /// Sets current length and seek to 0
        /// </summary>
        public void ResetSeekLength()
        {
            CurrentLength = 0;
            CurrentSeek = 0;
        }
        /// <summary>
        /// Creates new packet instance with the given capacity
        /// </summary>
        /// <param name="maxCapacity">packet capacity</param>
        public BytePacket(int maxCapacity)
        {
            Data = new byte[maxCapacity];
        }
        /// <summary>
        /// Creates new instance with the given buffer as the internal buffer. MaxCapacity == bufferToUse.Length
        /// </summary>
        /// <param name="bufferToUse">buffer to use as the internalBuffer</param>
        public BytePacket(byte[] bufferToUse)
        {
            Data = bufferToUse;
        }
        /// <summary>
        /// Creates new instance by copying the given packet. MaxCapacity == gamePacketToCopy.Length
        /// </summary>
        /// <param name="toCopy">GamePacket to copy</param>
        public BytePacket(BytePacket toCopy)
        {
            int length = toCopy.Data.Length;
            Data = new byte[length];
            ByteManipulator.Write<byte>(toCopy.Data, 0, Data, 0, length);
            CurrentLength = toCopy.CurrentLength;
            CurrentSeek = toCopy.CurrentSeek;
        }
    }
}