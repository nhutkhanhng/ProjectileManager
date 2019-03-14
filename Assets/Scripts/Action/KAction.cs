//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//using UnityEngine;
///// </summary>
//[Serializable]
//public sealed class NetException : Exception
//{
//    /// <summary>
//    /// NetException constructor
//    /// </summary>
//    public NetException()
//        : base()
//    {
//    }

//    /// <summary>
//    /// NetException constructor
//    /// </summary>
//    public NetException(string message)
//        : base(message)
//    {
//    }

//    /// <summary>
//    /// NetException constructor
//    /// </summary>
//    public NetException(string message, Exception inner)
//        : base(message, inner)
//    {
//    }

//    /// <summary>
//    /// NetException constructor
//    /// </summary>
//    private NetException(SerializationInfo info, StreamingContext context)
//        : base(info, context)
//    {
//    }

//    /// <summary>
//    /// Throws an exception, in DEBUG only, if first parameter is false
//    /// </summary>
//    [Conditional("DEBUG")]
//    public static void Assert(bool isOk, string message)
//    {
//        if (!isOk)
//            throw new NetException(message);
//    }

//    /// <summary>
//    /// Throws an exception, in DEBUG only, if first parameter is false
//    /// </summary>
//    [Conditional("DEBUG")]
//    public static void Assert(bool isOk)
//    {
//        if (!isOk)
//            throw new NetException();
//    }
//}

//public enum EAction
//{
//    Movement,
//    Die
//}

//public interface WriteData
//{
//    void Write(byte[] args);
//}

//public interface ReadByte
//{
//    byte ReadByte();
//}
//public abstract class KAction : WriteData, ReadByte
//{

//    protected byte[] m_argData = new byte[128];
//    public EAction OpCode { get; private set; }

//    public byte Offset { get; set; }
//    public byte[] Args
//    {
//        get
//        {
//            return m_argData;
//        }
//    }

//    public byte Length
//    {
//        get
//        {
//            return Offset;
//        }
//    }

//    public KAction()
//    {

//    }
//    public KAction(EAction opCode, params byte[] args)
//    {
//        OpCode = opCode;
//        args.CopyTo(m_argData, 0);
//    }

//    public virtual void Write(byte[] args)
//    {
//        // Todo Something.
//    }
//    public static byte ReadByte(byte[] fromBuffer, int numberOfBits, int readBitOffset)
//    {
//        NetException.Assert(((numberOfBits > 0) && (numberOfBits < 9)), "Read() can only read between 1 and 8 bits");

//        int bytePtr = readBitOffset >> 3;
//        int startReadAtIndex = readBitOffset - (bytePtr * 8); // (readBitOffset % 8);

//        if (startReadAtIndex == 0 && numberOfBits == 8)
//            return fromBuffer[bytePtr];

//        // mask away unused bits lower than (right of) relevant bits in first byte
//        byte returnValue = (byte)(fromBuffer[bytePtr] >> startReadAtIndex);

//        int numberOfBitsInSecondByte = numberOfBits - (8 - startReadAtIndex);

//        if (numberOfBitsInSecondByte < 1)
//        {
//            // we don't need to read from the second byte, but we DO need
//            // to mask away unused bits higher than (left of) relevant bits
//            return (byte)(returnValue & (255 >> (8 - numberOfBits)));
//        }

//        byte second = fromBuffer[bytePtr + 1];

//        // mask away unused bits higher than (left of) relevant bits in second byte
//        second &= (byte)(255 >> (8 - numberOfBitsInSecondByte));

//        return (byte)(returnValue | (byte)(second << (numberOfBits - numberOfBitsInSecondByte)));
//    }

//    public static ushort ReadUInt16(byte[] fromBuffer, int numberOfBits, int readBitOffset)
//    {
//        Debug.Assert(((numberOfBits > 0) && (numberOfBits <= 16)), "ReadUInt16() can only read between 1 and 16 bits");
//#endif
//        ushort returnValue;
//        if (numberOfBits <= 8)
//        {
//            returnValue = ReadByte(fromBuffer, numberOfBits, readBitOffset);
//            return returnValue;
//        }
//        returnValue = ReadByte(fromBuffer, 8, readBitOffset);
//        numberOfBits -= 8;
//        readBitOffset += 8;

//        if (numberOfBits <= 8)
//        {
//            returnValue |= (ushort)(ReadByte(fromBuffer, numberOfBits, readBitOffset) << 8);
//        }

//#if BIGENDIAN
//			// reorder bytes
//			uint retVal = returnValue;
//			retVal = ((retVal & 0x0000ff00) >> 8) | ((retVal & 0x000000ff) << 8);
//			return (ushort)retVal;
//#else
//        return returnValue;
//#endif
//    }

//    public byte ReadByte()
//    {
//        byte size = sizeof(ushort);
//        ushort res = ReadUInt16(m_argData, size * 8, Offset * 8);
//        Offset += size;
//        return res;
//    }
//}

//public class SellAction : KAction
//{
//    public int UDID;

//    public void Write(byte[] args)
//    {
//        this.UDID = args[0].
//    }
//}





