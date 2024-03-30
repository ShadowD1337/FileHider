using FileHider.Core;
using FileHider.Data;
using FileHider.Data.Models;
using FileHider.Web.MVC.Controllers;
using FileHider.Web.MVC.Settings;
using JpegLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StegoSharp;
using StegoSharp.Enums;
using StegSharp.Application.Common.Exceptions;
using StegSharp.Application.Common.Interfaces;
using StegSharp.Application.Models;
using StegSharp.Domain;
using StegSharp.Infrastructure.Services;
using StegSharp.Infrastructure.Util.Extensions;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FileHider.Experiments
{
    internal class Program
    {
        static ILogger<HideInformationController> logger;
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost;Database=filehider;Uid=root;Pwd=root;";

            /*var imageStegoStrategy = new ImageStegoStrategy(new[] { StegoSharp.Enums.ColorChannel.G, StegoSharp.Enums.ColorChannel.R }, 2, 1);

            Console.WriteLine(imageStegoStrategy.ColorChannelsString);*/

            /*
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            optionsBuilder.LogTo(Console.WriteLine, minimumLevel: LogLevel.Information);
            optionsBuilder.UseMySQL(connectionString);

            using var dbContext = new UserDbContext(optionsBuilder.Options);
            dbContext.Database.EnsureCreated();

            var hiddenMessage = new HiddenMessage("test");
            dbContext.HiddenInformations.Add(hiddenMessage);
            dbContext.ImageStegoStrategies.Add(new ImageStegoStrategy("Red,Green", 2, 1));
            dbContext.ImageFiles.Add(new ImageFile(1, "test", 1, 999));

            foreach (var imageFile in dbContext.ImageFiles)
            {
                imageFile.HiddenInformation = dbContext.HiddenInformations.Where(h => h.Id == imageFile.HiddenInformationId).First();
            }

            //Console.WriteLine(dbContext.Users.Where(u => u.FirstName == "gosho").First().Id);

            dbContext.SaveChanges();*/

            ILoggerFactory loggerFactory = new LoggerFactory();
            logger = new Logger<HideInformationController>(loggerFactory);

            var config = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();

            config["ConnectionStrings:DefaultConnection"] = connectionString;

            var firebaseSettings = new GoogleFirebaseSettings { ServiceAccountFilePath = Console.ReadLine(), BucketName = Console.ReadLine() };
            var options = Options.Create(firebaseSettings);

            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();
            optionsBuilder.LogTo(Console.WriteLine, minimumLevel: LogLevel.Information);
            optionsBuilder.UseMySQL(connectionString);

            using var dbContext = new UserDbContext(optionsBuilder.Options);




            var user = new IdentityUser();
            user.Id = "1";
            user.UserName = "Gosho";
            user.Email = "test@test.test";
            var fileUploadeer = new FileUploader();

            var f5Service = new Mock<IF5Service>();
            var services = new ServiceCollection();
            services.AddSingleton(_ => f5Service.Object);

            var serviceProvider = services.BuildServiceProvider();

            var stegoEngine = new StegoEngine(serviceProvider);

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(x => x.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)).Returns("user-id");

            var userEngine = new UserEngine(httpContextAccessor.Object, stegoEngine, dbContext, fileUploadeer);
            var controller = new HideInformationController(logger, userEngine);


            var imagePath = "C:\\Users\\Shadow Dragon\\Desktop\\test.jpg";
            var imagePath2 = "C:\\Users\\Shadow Dragon\\Desktop\\Files_test(6)(1).jpg";
            if (!File.Exists(imagePath)) throw new ArgumentException("No such image file.");

            byte[] fileBytes = File.ReadAllBytes(imagePath);
            var stream1 = new MemoryStream(fileBytes);

            var formFile = new FormFile(stream1, 0, fileBytes.Length, Path.GetFileNameWithoutExtension(imagePath), Path.GetFileName(imagePath));
            //controller.HideMessageInImage(formFile, "test789", "message");






            byte[] fileBytes2 = File.ReadAllBytes(imagePath2);
            var stream = new MemoryStream(fileBytes2);

            var formFile2 = new FormFile(stream, 0, fileBytes2.Length, "rly", Path.GetFileName(imagePath2));
            //Console.WriteLine(controller.ExtractHiddenMessageFromImage(formFile2, "test789"));





            /*Bitmap imageBitmap;
            MemoryStream memoryStream5 = new MemoryStream();
            formFile2.CopyTo(memoryStream5);
            imageBitmap = new Bitmap(memoryStream5);

            MemoryStream memoryStream = new MemoryStream();

            imageBitmap.Save(memoryStream, imageBitmap.RawFormat);
            memoryStream.Position = 0;

            BinaryReader binaryReader = new BinaryReader(memoryStream);

            byte[] data = binaryReader.ReadBytes((int)memoryStream.Length);
            memoryStream.Position = 0;

            var test = new Mock<IEncodingOrchestratorService>().Object;

            var info = new StegSharp.Application.Models.JpegInfo();
            var _headerService = new Mock<HeaderService>().Object;
            ParseJpegMarkers(binaryReader, info);

            DCTData dctData = DecodeData(info, binaryReader);

            //var _extractingService = new Mock<IF5ExtractingService>().Object;

            string res = Extract(dctData, "test789");
            */







        }
        /*

        static IPaddingService _paddingService = new Mock<IPaddingService>().Object;
        static IHuffmanDecodingService _huffmanDecodingService = new Mock<IHuffmanDecodingService>().Object;
        static IHuffmanEncodingService _huffmanEncodingService = new Mock<IHuffmanEncodingService>().Object;
        static DCTData DecodeData(JpegInfo jpeg, BinaryReader br)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                if (jpeg == null)
                {
                    throw new ArgumentNullException("jpeg", "jpeg".ToArgumentNullExceptionMessage());
                }

                if (br == null)
                {
                    throw new ArgumentNullException("br", "br".ToArgumentNullExceptionMessage());
                }

                int num = CalculateMCUCount(jpeg);
                DCTData dCTData = new DCTData(num);
                int prevDc = 0;
                int prevDc2 = 0;
                int prevDc3 = 0;
                for (int i = 0; i < num; i++)
                {
                    dCTData.YDCTData[i] = DecodeMCUComponent(br, prevDc, isLuminance: true);
                    dCTData.CBDCTData[i] = DecodeMCUComponent(br, prevDc3, isLuminance: false);
                    dCTData.CRDCTData[i] = DecodeMCUComponent(br, prevDc2, isLuminance: false);
                    prevDc = (int)dCTData.YDCTData[i][0];
                    prevDc3 = (int)dCTData.CBDCTData[i][0];
                    prevDc2 = (int)dCTData.CRDCTData[i][0];
                }

                _huffmanDecodingService.ResetBitReader();
                return dCTData;
            }
            finally
            {
                stopwatch.Stop();
                Trace.WriteLine("EncodingOrchestratorService.DecodeData " + stopwatch.ElapsedMilliseconds + "ms");
            }
        }

        static void EncodeMCUComponent(int prevDC, JpegBlock8x8F mcu, BinaryWriter bw, bool isLuminance)
        {
            if (isLuminance)
            {
                _huffmanEncodingService.EncodeLuminanceDC((int)mcu[0], prevDC, bw);
                _huffmanEncodingService.EncodeLuminanceAC(mcu, bw);
            }
            else
            {
                _huffmanEncodingService.EncodeChrominanceDC((int)mcu[0], prevDC, bw);
                _huffmanEncodingService.EncodeChrominanceAC(mcu, bw);
            }
        }

        static int CalculateMCUCount(JpegInfo jpeg)
        {
            int num = CalculatePaddedDimension(jpeg.Height);
            int num2 = CalculatePaddedDimension(jpeg.Width);
            return num * num2 / 64;
        }
        static int CalculatePaddedDimension(int input)
        {
            if (input % 8 != 0)
            {
                return input + 8 - input % 8;
            }

            return input;
        }
        static JpegBlock8x8F DecodeMCUComponent(BinaryReader br, int prevDc, bool isLuminance)
        {
            int num;
            JpegBlock8x8F result;
            if (isLuminance)
            {
                num = _huffmanDecodingService.DecodeLuminanceDC(prevDc, br);
                result = _huffmanDecodingService.DecodeLuminanceAC(br);
            }
            else
            {
                num = _huffmanDecodingService.DecodeChrominanceDC(prevDc, br);
                result = _huffmanDecodingService.DecodeChrominanceAC(br);
            }

            result[0] = num;
            return result;
        }

        static int DecodeChrominanceDC(int prevDC, BinaryReader br)
        {
            return DecodeDC(prevDC, br, _dcCrominanceDiffDict);
        }

        static int DecodeLuminanceDC(int prevDC, BinaryReader br)
        {
            return DecodeDC(prevDC, br, _dcLuminanceDiffDict);
        }

        static JpegBlock8x8F DecodeChrominanceAC(BinaryReader br)
        {
            return DecodeAC(br, _acCrominanceCoeffDict);
        }

        static JpegBlock8x8F DecodeLuminanceAC(BinaryReader br)
        {
            return DecodeAC(br, _acLuminanceCoeffDict);
        }

        static Tuple<int, int> CalculateCoefDictValue(Tuple<int, int> item, List<Tuple<int, int>> acLuminanceCoeffList)
        {
            int num = acLuminanceCoeffList.IndexOf(item);
            if (num == 0)
            {
                return new Tuple<int, int>(0, 0);
            }

            num--;
            int num2 = 0;
            int num3 = 0;
            num2 = num / 10;
            if (num2 == 15)
            {
                if (num3 == 10)
                {
                    return new Tuple<int, int>(num2, 0);
                }

                num3 = num % 10;
            }
            else
            {
                num3 = 1 + num % 10;
            }

            if (num2 == 16)
            {
                return new Tuple<int, int>(15, 10);
            }

            return new Tuple<int, int>(num2, num3);
        }

        void ExtractTable(int[] bits, int[] val, Tuple<int, int>[] table)
        {
            int num = 0;
            int num2 = 0;
            for (int i = 1; i < bits.Length; i++)
            {
                for (int j = 0; j < bits[i]; j++)
                {
                    int num3 = val[num];
                    table[num3] = new Tuple<int, int>(num2++, i);
                    num++;
                }

                num2 <<= 1;
            }
        }

        int CalculateDC(int prevDC, int diffValue)
        {
            return diffValue + prevDC;
        }

        int ReadDiffValue(BinaryReader br, int diffValueCodeLength, ref bool isNegativeNumber)
        {
            int num = _bitReaderService.Read(br, reset: true);
            if (num == 0)
            {
                isNegativeNumber = true;
            }

            for (int i = 0; i < diffValueCodeLength - 1; i++)
            {
                num = _bitReaderService.Read(br, reset: false);
            }

            return num;
        }

        int DecodeDC(int prevDC, BinaryReader br, Dictionary<Tuple<int, int>, int> diffDict)
        {
            int num = 0;
            int num2 = 0;
            num2++;
            Tuple<int, int> key = new Tuple<int, int>(_bitReaderService.Read(br, reset: true), num2);
            int value;
            while (!diffDict.TryGetValue(key, out value))
            {
                int item = _bitReaderService.Read(br, reset: false);
                num2++;
                key = new Tuple<int, int>(item, num2);
            }

            int value2 = 0;
            bool isNegativeNumber = false;
            if (value != 0)
            {
                value2 = ReadDiffValue(br, value, ref isNegativeNumber);
            }

            value2 = ToTwosComplement(value2, value, (!isNegativeNumber) ? 1 : 0);
            return CalculateDC(prevDC, value2);
        }

        int ToTwosComplement(int value, int numberOfBits, int firstBit)
        {
            if (firstBit == 1)
            {
                return value;
            }

            value |= 1 << numberOfBits;
            return -(~value & ((1 << numberOfBits) - 1));
        }

        JpegBlock8x8F DecodeAC(BinaryReader br, Dictionary<Tuple<int, int>, Tuple<int, int>> coeffDict)
        {
            List<Tuple<int, int>> pairs = ReadRunLengthPairs(br, coeffDict);
            return _runLengthEncodingService.Decode(pairs);
        }

        List<Tuple<int, int>> ReadRunLengthPairs(BinaryReader br, Dictionary<Tuple<int, int>, Tuple<int, int>> coefDict)
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            int num = 1;
            int num2 = 1;
            Tuple<int, int> key = new Tuple<int, int>(_bitReaderService.Read(br, reset: true), num2);
            while (num < 64)
            {
                if (coefDict.TryGetValue(key, out Tuple<int, int> value))
                {
                    if ((value.Item1 != 0 && value.Item2 != 0) || (value.Item1 != 15 && value.Item2 != 0))
                    {
                        int item = ReadCoeffValueFromCategory(br, value);
                        list.Add(new Tuple<int, int>(value.Item1, item));
                    }
                    else
                    {
                        list.Add(value);
                        if (value.Item1 == 0)
                        {
                            break;
                        }
                    }

                    num = num + 1 + value.Item1;
                    if (num > 63)
                    {
                        break;
                    }

                    int item2 = _bitReaderService.Read(br, reset: true);
                    num2 = 1;
                    key = new Tuple<int, int>(item2, num2);
                }
                else
                {
                    int item3 = _bitReaderService.Read(br, reset: false);
                    num2++;
                    key = new Tuple<int, int>(item3, num2);
                }
            }

            return list;
        }

        int ReadCoeffValueFromCategory(BinaryReader br, Tuple<int, int> runLengthCategoryPair)
        {
            int num = _bitReaderService.Read(br, reset: true);
            int firstBit = num;
            for (int i = 0; i < runLengthCategoryPair.Item2 - 1; i++)
            {
                num = _bitReaderService.Read(br, reset: false);
            }

            return ToTwosComplement(num, runLengthCategoryPair.Item2, firstBit);
        }

        static byte ReadUnsupportedSegment(BinaryReader br)
        {
            int num = br.Read2Bytes();
            byte result = br.ReadByte();
            for (int i = 0; i < num - 3; i++)
            {
                result = br.ReadByte();
            }
            return result;
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
        static void ParseJpegMarkers(BinaryReader br, JpegInfo jpeg)
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
                        case JpegMarker.StartOfFrame1:
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

        static JpegMarker? ParseJpegMarker(byte marker)
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
            byte[] array = new byte[7] { 74, 70, 73, 70, 0, 1, 1 };
            for (int i = 0; i < 7; i++)
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

        static byte ParseQuantizationTable(BinaryReader br, JpegInfo jpeg)
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

        static byte ReadDQTForComponent(BinaryReader br, JpegInfo jpeg, byte destination)
        {
            if (destination != 0 && destination != 1)
            {
                throw new Exception("Unsupported DQT destination.");
            }
            jpeg.QuantizationTables[destination] = new JpegQuantizationTable(destination);
            jpeg.QuantizationTables[destination].Values = ReadDQTData(br);
            return jpeg.QuantizationTables[destination].Values.Last();
        }

        static byte[] ReadDQTData(BinaryReader br)
        {
            byte[] array = new byte[64];
            for (int i = 0; i < 64; i++)
            {
                array[JpegSorting.JpegNaturalOrder[i]] = br.ReadByte();
            }
            return array;
        }

        static byte ParseSOF0(BinaryReader br, JpegInfo jpeg)
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

        static byte ParseHuffmanTables(BinaryReader br, JpegInfo jpeg)
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

        static void SaveHuffmanTable(JpegInfo jpeg, byte classDestination, byte[] bitsArray, byte[] hufValArray)
        {
            jpeg.HuffmanTables.Add(new JpegHuffmanTable
            {
                Id = classDestination,
                Bits = ((IEnumerable<byte>)bitsArray).Select((Func<byte, int>)((byte item) => item)).ToArray(),
                HufVal = ((IEnumerable<byte>)hufValArray).Select((Func<byte, int>)((byte item) => item)).ToArray()
            });
        }

        static byte[] ReadHufValArray(BinaryReader br, int hufValCount)
        {
            byte[] array = new byte[hufValCount];
            for (int i = 0; i < hufValCount; i++)
            {
                array[i] = br.ReadByte();
            }
            return array;
        }

        static byte[] ReadBitsArray(BinaryReader br)
        {
            byte[] array = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                array[i] = br.ReadByte();
            }
            return array;
        }

        static void SaveHuffmanTableData(JpegInfo jpeg, byte dhtClass, byte dhtDestination, byte[] bitsArray, byte[] hufValArray)
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

        static byte ParseConsecutiveHuffmanTables(BinaryReader br, JpegInfo jpeg)
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

        static int CalculateHufvalCount(byte dhtClass)
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












        static IPermutationService _permutationService = new Mock<IPermutationService>().Object;

        static IMCUConverterService _mcuConverterService = new Mock<IMCUConverterService>().Object;

        static IF5ParameterCalculatorService _f5ParameterCalulatorService = new Mock<IF5ParameterCalculatorService>().Object;

        static string Extract(DCTData dctData, string password)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                if (dctData == null)
                {
                    throw new ArgumentNullException("dctData", "dctData".ToArgumentNullExceptionMessage());
                }

                if (string.IsNullOrEmpty(password))
                {
                    throw new ArgumentNullException("password", "password".ToArgumentNullExceptionMessage());
                }

                JpegBlock8x8F[] inputArray = _mcuConverterService.DCTDataToMCUArray(dctData);
                JpegBlock8x8F[] dctArray = _permutationService.PermutateArray(password, inputArray, reverse: false);
                float[] coeffs = MCUArrayToCoeffArray(dctArray);
                int k;
                int msgLen;
                int lastReadIndex = ReadDecodingInfo(coeffs, out k, out msgLen);
                if (k > 9)
                {
                    throw new MatrixEncodingException("Error while reading parameter k from the image.");
                }

                int n = _f5ParameterCalulatorService.CalculateN(k);
                return ReadEmbeddedMessage(coeffs, k, n, msgLen, lastReadIndex);
            }
            finally
            {
                stopwatch.Stop();
                Trace.WriteLine("F5ExtractingService.Extract " + stopwatch.ElapsedMilliseconds + "ms");
            }
        }
        static float[] MCUArrayToCoeffArray(JpegBlock8x8F[] dctArray)
        {
            if (dctArray == null || dctArray.Length == 0)
            {
                throw new ArgumentNullException("dctArray", "dctArray".ToArgumentNullExceptionMessage());
            }

            int num = 64;
            int num2 = dctArray.Length;
            float[] array = new float[num2 * num];
            int num3 = 0;
            for (int i = 0; i < num2; i++)
            {
                JpegBlock8x8F jpegBlock8x8F = dctArray[i];
                for (int j = 0; j < num; j++)
                {
                    array[num3++] = jpegBlock8x8F[j];
                }
            }

            return array;
        }
        static int ReadDecodingInfo(float[] coeffs, out int k, out int msgLen)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 32;
            int num4 = 0;
            while (num2 < num3)
            {
                int num5 = (int)coeffs[num];
                if (num5 != 0 && num % 64 != 0)
                {
                    int num6 = ReadDecodingInfoBitFromCoeff(num5);
                    num4 <<= 1;
                    num4 |= num6;
                    num2++;
                }

                num++;
            }

            ExtractDecodedData(num4, out k, out msgLen);
            return num;
        }

        static int ReadDecodingInfoBitFromCoeff(int coeff)
        {
            if ((coeff < 0 && coeff % 2 == 0) || (coeff > 0 && coeff % 2 == 1))
            {
                return 1;
            }

            return 0;
        }

        static void ExtractDecodedData(int input, out int upperByte, out int lowerInt)
        {
            upperByte = (byte)(input >>> 24);
            lowerInt = input & 0xFFFFFF;
        }

        static string ReadEmbeddedMessage(float[] coeffs, int k, int n, int msgLen, int lastReadIndex)
        {
            int num = 0;
            int coeffCount = 0;
            int index = lastReadIndex + 1;
            byte[] array = new byte[msgLen / 8];
            int messageByteIndex = 0;
            int messageBitIndex = 0;
            while (num < msgLen)
            {
                int[] coefficients = GetCoefficients(coeffs, n, ref coeffCount, ref index);
                int hash = CalculateHash(n, coefficients);
                ExtractMessageBits(k, array, ref messageByteIndex, ref messageBitIndex, hash);
                num += k;
                coeffCount = 0;
            }

            return Encoding.UTF8.GetString(array);
        }

        static void ExtractMessageBits(int k, byte[] messageBytes, ref int messageByteIndex, ref int messageBitIndex, int hash)
        {
            if (messageBitIndex == 8)
            {
                messageBitIndex = 0;
                messageByteIndex++;
            }

            for (int num = GetHashStartIndex(k, messageBytes, messageByteIndex, messageBitIndex); num >= 0; num--)
            {
                int num2 = (hash >> num) & 1;
                if (messageBitIndex == 8)
                {
                    messageBitIndex = 0;
                    messageByteIndex++;
                }

                messageBytes[messageByteIndex] <<= 1;
                messageBytes[messageByteIndex] |= (byte)num2;
                messageBitIndex++;
            }
        }

        static int GetHashStartIndex(int k, byte[] messageBytes, int messageByteIndex, int messageBitIndex)
        {
            bool num = messageBitIndex + k > 7 && messageByteIndex == messageBytes.Count() - 1;
            int result = k - 1;
            if (num)
            {
                result = 8 - (messageBitIndex + 1);
            }

            return result;
        }

        static int CalculateHash(int n, int[] coeffsToRead)
        {
            int num = 0;
            for (int i = 0; i < n; i++)
            {
                int num2 = coeffsToRead[i];
                if (((num2 > 0) ? (num2 & 1) : (1 - (num2 & 1))) == 1)
                {
                    num ^= i + 1;
                }
            }

            return num;
        }

        static int[] GetCoefficients(float[] coeffs, int n, ref int coeffCount, ref int index)
        {
            int[] array = new int[n];
            while (coeffCount < n)
            {
                int num = (int)coeffs[index];
                if (num != 0 && index % 64 != 0)
                {
                    array[coeffCount] = num;
                    coeffCount++;
                }

                index++;
            }

            return array;
        }*/
    }
}
