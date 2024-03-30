using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using StegSharp.Application.Common.Interfaces;
using StegSharp.Application.Models;
using StegSharp.Domain;
using StegSharp.Infrastructure.Util.Extensions;

namespace StegSharp.Infrastructure.Services;

public class HeaderService : IHeaderService
{
	public void WriteHeaders(BinaryWriter bw, JpegInfo jpeg)
	{
		if (jpeg == null)
		{
			throw new ArgumentNullException("jpeg", "jpeg".ToArgumentNullExceptionMessage());
		}
		if (bw == null)
		{
			throw new ArgumentNullException("bw", "bw".ToArgumentNullExceptionMessage());
		}
		WriteSOI(bw);
		WriteApp0(bw);
		WriteDQT(bw);
		WriteSOF0(bw, jpeg);
		WriteDHT(bw);
		WriteSOS(bw);
	}

	public void WriteEOI(BinaryWriter bw)
	{
		if (bw == null)
		{
			throw new ArgumentNullException("bw", "bw".ToArgumentNullExceptionMessage());
		}
		bw.Write(byte.MaxValue);
		bw.Write((byte)217);
	}

	public void ParseJpegMarkers(BinaryReader br, JpegInfo jpeg)
	{
		if (jpeg == null)
		{
			throw new ArgumentNullException("jpeg", "jpeg".ToArgumentNullExceptionMessage());
		}
		if (br == null)
		{
			throw new ArgumentNullException("br", "br".ToArgumentNullExceptionMessage());
		}
		byte b = br.ReadByte();
		bool flag = true;
		while (flag)
		{
			byte num = b;
			b = br.ReadByte();
			if (num == byte.MaxValue && b != byte.MaxValue)
			{
				switch (ParseJpegMarker(b))
				{
				case JpegMarker.App0:
					b = ParseApp0Segment(br, jpeg);
					break;
				case JpegMarker.DefineQuantizationTable:
					b = ParseQuantizationTable(br, jpeg);
					break;
				case JpegMarker.StartOfFrame0:
					b = ParseSOF0(br, jpeg);
					break;
				case JpegMarker.DefineHuffmanTable:
					b = ParseHuffmanTables(br, jpeg);
					break;
				case JpegMarker.StartOfScan:
					b = ParseStartOfScan(br, jpeg);
					flag = false;
					break;
				default:
					b = ReadUnsupportedSegment(br);
					break;
				case JpegMarker.StartOfImage:
					break;
				}
			}
		}
	}

    public enum JpegMarker : byte
    {
        Padding = byte.MaxValue,
        StartOfImage = 216,
        App0 = 224,
        DefineQuantizationTable = 219,
        StartOfFrame0 = 192,
        StartOfFrame1 = 194,
        DefineHuffmanTable = 196,
        StartOfScan = 218,
        EndOfImage = 217
    }

    private void WriteSOI(BinaryWriter bw)
	{
		bw.Write(byte.MaxValue);
		bw.Write((byte)216);
	}

	private void WriteApp0(BinaryWriter bw)
	{
		bw.Write(byte.MaxValue);
		bw.Write((byte)224);
		byte[] buffer = new byte[16]
		{
			0, 16, 74, 70, 73, 70, 0, 1, 1, 1,
			0, 96, 0, 96, 0, 0
		};
		bw.Write(buffer);
	}

	private void WriteDQT(BinaryWriter bw)
	{
		WriteDQTMarkers(bw, 0);
		WriteDQTData(bw, JpegStandardQuantizationTable.LuminanceTable);
		bw.Write((byte)1);
		WriteDQTData(bw, JpegStandardQuantizationTable.ChrominanceTable);
	}

	private static void WriteDQTMarkers(BinaryWriter bw, byte destination)
	{
		bw.Write(byte.MaxValue);
		bw.Write((byte)219);
		bw.Write(new byte[2] { 0, 132 });
		bw.Write(destination);
	}

	private void WriteDQTData(BinaryWriter bw, byte[] table)
	{
		for (int i = 0; i < table.Length; i++)
		{
			bw.Write(table[JpegSorting.JpegNaturalOrder[i]]);
		}
	}

	private void WriteSOF0(BinaryWriter bw, JpegInfo jpeg)
	{
		bw.Write(byte.MaxValue);
		bw.Write((byte)192);
		List<byte> list = new List<byte>(19)
		{
			0,
			17,
			8,
			(byte)((uint)(jpeg.Height >> 8) & 0xFFu),
			(byte)((uint)jpeg.Height & 0xFFu),
			(byte)((uint)(jpeg.Width >> 8) & 0xFFu),
			(byte)((uint)jpeg.Width & 0xFFu),
			3
		};
		for (int i = 0; i < 3; i++)
		{
			list.Add((byte)(i + 1));
			list.Add(17);
			if (i == 0)
			{
				list.Add(0);
			}
			else
			{
				list.Add(1);
			}
		}
		bw.Write(list.ToArray());
	}

	private void WriteDHT(BinaryWriter bw)
	{
		bw.Write(byte.MaxValue);
		bw.Write((byte)196);
		bw.Write(new byte[2] { 1, 162 });
		bw.Write(ConvertToByteArray(HuffmanEncodingTables.DCLuminanceBits));
		bw.Write(ConvertToByteArray(HuffmanEncodingTables.DCLuminanceValues));
		bw.Write(ConvertToByteArray(HuffmanEncodingTables.ACLuminanceBits));
		bw.Write(ConvertToByteArray(HuffmanEncodingTables.ACLuminanceValues));
		bw.Write(ConvertToByteArray(HuffmanEncodingTables.DCChrominanceBits));
		bw.Write(ConvertToByteArray(HuffmanEncodingTables.DCChrominanceValues));
		bw.Write(ConvertToByteArray(HuffmanEncodingTables.ACChrominanceBits));
		bw.Write(ConvertToByteArray(HuffmanEncodingTables.ACChrominanceValues));
	}

	private static byte[] ConvertToByteArray(int[] input)
	{
		return input.Select((int item) => (byte)item).ToArray();
	}

	private void WriteSOS(BinaryWriter bw)
	{
		bw.Write(byte.MaxValue);
		bw.Write((byte)218);
		bw.Write(new byte[2] { 0, 12 });
		bw.Write((byte)3);
		bw.Write((byte)1);
		bw.Write((byte)0);
		bw.Write((byte)2);
		bw.Write((byte)17);
		bw.Write((byte)3);
		bw.Write((byte)17);
		bw.Write((byte)0);
		bw.Write((byte)63);
		bw.Write((byte)0);
	}

	private byte ReadUnsupportedSegment(BinaryReader br)
	{
		int num = br.Read2Bytes();
		byte result = br.ReadByte();
		for (int i = 0; i < num - 3; i++)
		{
			result = br.ReadByte();
		}
		return result;
	}

	private JpegMarker? ParseJpegMarker(byte marker)
	{
		if (Enum.IsDefined(typeof(JpegMarker), marker))
		{
			return (JpegMarker)marker;
		}
		return null;
	}

    static byte ParseApp0Segment(BinaryReader br, JpegInfo jpeg)
    {
        br.Read2Bytes();
        byte[] array = new byte[8] { 74, 70, 73, 70, 0, 1, 2, 1 };
        for (int i = 0; i < array.Length; i++)
        {
            var t = br.ReadByte();
            if (array[i] != t)
            {
                throw new Exception("Error reading Jfif version.");
            }
        }
        br.ReadByte();
        int horizontalPixelDensity = br.Read2Bytes();
        int verticalPixelDensity = br.Read2Bytes();
        jpeg.HorizontalPixelDensity = horizontalPixelDensity;
        jpeg.VerticalPixelDensity = verticalPixelDensity;
        return (byte)((uint)br.Read2Bytes() & 0xFFu);
    }

    private byte ParseQuantizationTable(BinaryReader br, JpegInfo jpeg)
	{
		if (br.Read2Bytes() > 67)
		{
			byte destination = br.ReadByte();
			ReadDQTForComponent(br, jpeg, destination);
			destination = br.ReadByte();
			return ReadDQTForComponent(br, jpeg, destination);
		}
		byte destination2 = br.ReadByte();
		return ReadDQTForComponent(br, jpeg, destination2);
	}

	private byte ReadDQTForComponent(BinaryReader br, JpegInfo jpeg, byte destination)
	{
		if (destination != 0 && destination != 1)
		{
			throw new Exception("Unsupported DQT destination.");
		}
		jpeg.QuantizationTables[destination] = new JpegQuantizationTable(destination);
		jpeg.QuantizationTables[destination].Values = ReadDQTData(br);
		return jpeg.QuantizationTables[destination].Values.Last();
	}

	private byte[] ReadDQTData(BinaryReader br)
	{
		byte[] array = new byte[64];
		for (int i = 0; i < 64; i++)
		{
			array[JpegSorting.JpegNaturalOrder[i]] = br.ReadByte();
		}
		return array;
	}

	private byte ParseSOF0(BinaryReader br, JpegInfo jpeg)
	{
		br.Read2Bytes();
		byte precision = br.ReadByte();
		int height = br.Read2Bytes();
		int width = br.Read2Bytes();
		byte b = br.ReadByte();
		byte result = b;
		jpeg.Height = height;
		jpeg.Width = width;
		jpeg.Precision = precision;
		jpeg.Components = new JpegComponent[b];
		for (int i = 0; i < b; i++)
		{
			byte id = br.ReadByte();
			byte samplingFactor = br.ReadByte();
			byte b2 = br.ReadByte();
			result = b2;
			jpeg.Components[i] = new JpegComponent
			{
				SamplingFactor = samplingFactor,
				Id = id,
				QuantizationTableId = b2
			};
		}
		return result;
	}

	private byte ParseHuffmanTables(BinaryReader br, JpegInfo jpeg)
	{
		int num = br.Read2Bytes();
		if (num > 255)
		{
			return ParseConsecutiveHuffmanTables(br, jpeg);
		}
		byte classDestination;
		byte num2 = (classDestination = br.ReadByte());
		byte dhtClass = (byte)(num2 >> 4);
		byte dhtDestination = (byte)(num2 & 0xFu);
		byte[] array = ReadBitsArray(br);
		int hufValCount = num - 3 - array.Length;
		byte[] array2 = ReadHufValArray(br, hufValCount);
		SaveHuffmanTableData(jpeg, dhtClass, dhtDestination, array, array2);
		SaveHuffmanTable(jpeg, classDestination, array, array2);
		return array2.Last();
	}

	private void SaveHuffmanTable(JpegInfo jpeg, byte classDestination, byte[] bitsArray, byte[] hufValArray)
	{
		jpeg.HuffmanTables.Add(new JpegHuffmanTable
		{
			Id = classDestination,
			Bits = ((IEnumerable<byte>)bitsArray).Select((Func<byte, int>)((byte item) => item)).ToArray(),
			HufVal = ((IEnumerable<byte>)hufValArray).Select((Func<byte, int>)((byte item) => item)).ToArray()
		});
	}

	private byte[] ReadHufValArray(BinaryReader br, int hufValCount)
	{
		byte[] array = new byte[hufValCount];
		for (int i = 0; i < hufValCount; i++)
		{
			array[i] = br.ReadByte();
		}
		return array;
	}

	private byte[] ReadBitsArray(BinaryReader br)
	{
		byte[] array = new byte[16];
		for (int i = 0; i < 16; i++)
		{
			array[i] = br.ReadByte();
		}
		return array;
	}

	private void SaveHuffmanTableData(JpegInfo jpeg, byte dhtClass, byte dhtDestination, byte[] bitsArray, byte[] hufValArray)
	{
		if (dhtClass == 0 && dhtDestination == 0)
		{
			jpeg.HuffmanTableData.DCLuminanceBits = ((IEnumerable<byte>)bitsArray).Select((Func<byte, int>)((byte item) => item)).ToArray();
			jpeg.HuffmanTableData.DCLuminanceValues = ((IEnumerable<byte>)hufValArray).Select((Func<byte, int>)((byte item) => item)).ToArray();
		}
		if (dhtClass == 0 && dhtDestination == 1)
		{
			jpeg.HuffmanTableData.DCChrominanceBits = ((IEnumerable<byte>)bitsArray).Select((Func<byte, int>)((byte item) => item)).ToArray();
			jpeg.HuffmanTableData.DCChrominanceValues = ((IEnumerable<byte>)hufValArray).Select((Func<byte, int>)((byte item) => item)).ToArray();
		}
		if (dhtClass == 1 && dhtDestination == 0)
		{
			jpeg.HuffmanTableData.ACLuminanceBits = ((IEnumerable<byte>)bitsArray).Select((Func<byte, int>)((byte item) => item)).ToArray();
			jpeg.HuffmanTableData.ACLuminanceValues = ((IEnumerable<byte>)hufValArray).Select((Func<byte, int>)((byte item) => item)).ToArray();
		}
		if (dhtClass == 1 && dhtDestination == 1)
		{
			jpeg.HuffmanTableData.ACChrominanceBits = ((IEnumerable<byte>)bitsArray).Select((Func<byte, int>)((byte item) => item)).ToArray();
			jpeg.HuffmanTableData.ACChrominanceValues = ((IEnumerable<byte>)hufValArray).Select((Func<byte, int>)((byte item) => item)).ToArray();
		}
	}

	private byte ParseConsecutiveHuffmanTables(BinaryReader br, JpegInfo jpeg)
	{
		for (int i = 0; i < 4; i++)
		{
			byte b = br.ReadByte();
			byte dhtClass = (byte)(b >> 4);
			byte[] source = ReadBitsArray(br);
			int hufValCount = CalculateHufvalCount(dhtClass);
			byte[] source2 = ReadHufValArray(br, hufValCount);
			jpeg.HuffmanTables.Add(new JpegHuffmanTable
			{
				Id = b,
				Bits = ((IEnumerable<byte>)source).Select((Func<byte, int>)((byte item) => item)).ToArray(),
				HufVal = ((IEnumerable<byte>)source2).Select((Func<byte, int>)((byte item) => item)).ToArray()
			});
		}
		return (byte)jpeg.HuffmanTables.Last().HufVal.Last();
	}

	private static int CalculateHufvalCount(byte dhtClass)
	{
		if (dhtClass == 0)
		{
			return 12;
		}
		return HuffmanEncodingTables.ACLuminanceValues.Length;
	}

    static byte ParseStartOfScan(BinaryReader br, JpegInfo jpeg)
    {
        int num = br.Read2Bytes();
        byte b = br.ReadByte();
        byte result = b;
        for (int i = 0; i < b; i++)
        {
            byte componentId = br.ReadByte();
            byte num2 = br.ReadByte();
            if (componentId == 0 && num2 == 0)
            {
                componentId = br.ReadByte();
                num2 = br.ReadByte();
            }
            byte dCHuffmanTableId = (byte)(num2 >> 4);
            byte aCHuffmanTableId = (byte)(num2 & 0xFu);
            JpegComponent? jpegComponent = jpeg.Components.Where((JpegComponent item) => item.Id == componentId).FirstOrDefault();
            jpegComponent.DCHuffmanTableId = dCHuffmanTableId;
            jpegComponent.ACHuffmanTableId = aCHuffmanTableId;
        }
        for (int j = 0; j < num - 3 - 2 * b; j++)
        {
            result = br.ReadByte();
        }
        return result;
    }
}
