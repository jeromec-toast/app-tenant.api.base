using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using iT = iTextSharp.text;
using iTextSharp.text.pdf;
using ImageMagick;
using SDI = System.Drawing;
using System.Drawing.Imaging;
using System.Drawing;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;

namespace Tenant.API.Base.Util
{
    public class TnUtil
    {
        #region String

        /// <summary>
        /// String.
        /// </summary>
        public class String
        {
            /// <summary>
            /// Split the specified value with seperator.
            /// </summary>
            /// <returns>The split.</returns>
            /// <param name="value">Value.</param>
            /// <param name="seperator">Seperator.</param>
            public static string[] Split(string value, string seperator)
            {

                if (string.IsNullOrEmpty(value))
                    return null;

                if (!value.Contains(seperator))
                    return null;

                return value.Split(seperator);
            }

            /// <summary>
            /// Join the string with seperator
            /// </summary>
            /// <param name="values"></param>
            /// <param name="seperator"></param>
            /// <returns></returns>
            public static string Join(List<string> values, string seperator)
            {
                if (values != null && values.Count() > 0)
                    return null;

                return string.Join(seperator, values);
            }

            /// <summary>
            /// separates the given string to specified chunkSize
            /// </summary>
            /// <param name="value"></param>
            /// <param name="chunkSize"></param>
            /// <returns></returns>
            public static IEnumerable<string> Split(string value, int chunkSize)
            {

                if (!string.IsNullOrEmpty(value) && chunkSize > 0)
                {
                    return Enumerable.Range(0, value.Length / chunkSize)
                                .Select(i => value.Substring(i * chunkSize, chunkSize));
                }
                else
                    return null;
            }
        }

        #endregion

        #region Date

        /// <summary>
        /// Date.
        /// </summary>
        public class Date
        {
            /// <summary>
            /// Validates the date range.
            /// </summary>
            /// <param name="dateRange">Date range.</param>
            public static DateTime[] GetDates(string dateRange)
            {
                try
                {
                    string[] dateElements = String.Split(dateRange, Core.Constant.Date.SPERATOR);

                    DateTime[] values = new DateTime[2];
                    values[0] = DateTime.ParseExact(dateElements[0], Core.Constant.Date.FORMAT, CultureInfo.InvariantCulture);
                    values[1] = DateTime.ParseExact(dateElements[1], Core.Constant.Date.FORMAT, CultureInfo.InvariantCulture);

                    return values;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            /// <summary>
            /// Gets the current time.
            /// </summary>
            /// <returns>The current time.</returns>
            public static DateTime GetCurrentTime()
            {
                return DateTime.UtcNow;
            }

            /// <summary>
            /// Parse and format date string
            /// </summary>
            /// <param name="date"></param>
            /// <returns></returns>
            public static DateTime ParseDate(string date)
            {
                DateTime dateElement = DateTime.ParseExact(date, Core.Constant.Date.FORMAT, CultureInfo.InvariantCulture);
                return dateElement;
            }

            /// <summary>
            /// Get date string
            /// </summary>
            /// <param name="dateTime"></param>
            /// <returns></returns>
            public static string ParseDate(DateTime dateTime)
            {
                return dateTime.ToString(Core.Constant.Date.FORMAT);
            }
            /// <summary>
            /// Get dateTime with milli second as string
            /// </summary>
            /// <param name="dateTime"></param>
            /// <returns></returns>
            public static string ParseDateTime(DateTime dateTime)
            {
                return dateTime.ToString(Core.Constant.Date.FORMAT_DATE_TIME_MS);
            }

            /// <summary>
            /// Get Date from timeTick
            /// </summary>
            /// <param name="timeTick"></param>
            /// <returns></returns>
            public static DateTime ParseDate(long timeTick)
            {
                return new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(Convert.ToInt64(timeTick));
            }

            /// <summary>
            /// Get date string
            /// </summary>
            /// <param name="dateTime"></param>
            /// <returns></returns>
            public static string ParseDateYear(DateTime dateTime)
            {
                return dateTime.ToString(Core.Constant.Date.FORMAT_DATE_YEAR);
            }

            /// <summary>
            /// Custom XcDateConverter type
            /// </summary>
            public class TnDateConverter : IsoDateTimeConverter
            {
                /// <summary>
                /// Constructor setup the XcDate format 
                /// </summary>
                public TnDateConverter()
                {
                    base.DateTimeFormat = Core.Constant.Date.FORMAT;
                }
            }

            /// <summary>
            /// Custorm XcDateTimeConverter type
            /// </summary>
            public class TnDateTimeConverter : IsoDateTimeConverter
            {
                /// <summary>
                /// Constructor setup the XcDateTime format 
                /// </summary>
                public TnDateTimeConverter()
                {
                    base.DateTimeFormat = Core.Constant.Date.FORMAT_DATE_TIME;
                }
            }
            /// <summary>
            /// Custorm XcDateTimeConverter type Two
            /// </summary>
            public class TnDateTimeConverter_Aws : IsoDateTimeConverter
            {
                /// <summary>
                /// Constructor setup the XcDateTime format 
                /// </summary>
                public TnDateTimeConverter_Aws()
                {
                    base.DateTimeFormat = Core.Constant.Date.FORMAT_DATE_TIME_2;
                }
            }
            /// <summary>
            /// Get date string
            /// </summary>
            /// <param name="dateTime"></param>
            /// <returns></returns>
            public static string ParseDate(DateTime dateTime, string dateformat)
            {
                return dateTime.ToString(dateformat);
            }

            /// <summary>
            /// Custorm XcDateTimeConverter type Two
            /// </summary>
            public class TnDateTimeConverter_AwsDate : IsoDateTimeConverter
            {
                /// <summary>
                /// Constructor setup the XcDateTime format 
                /// </summary>
                //public XcDateTimeConverter_AwsDate()
                //{
                //    base.DateTimeFormat = Core.Constant.Date.FORMAT_DATE_TIME_MSF;
                //}
            }

            /// <summary>
            /// Get the datetime from string
            /// </summary>
            /// <param name="date"></param>
            /// <returns></returns>
            public static DateTime ParseDateYear(string date)
            {
                DateTime dateElement = DateTime.ParseExact(date, Core.Constant.Date.FORMAT_DATE_YEAR, CultureInfo.InvariantCulture);
                return dateElement;
            }

            /// <summary>
            /// Custorm XcDateTimeConverter Awsdate with twenty four hours format
            /// </summary>
            public class XcDateTimeConverter_AwsDateTF : IsoDateTimeConverter
            {
                /// <summary>
                /// Constructor setup the XcDateTime twenty four hours format 
                /// </summary>
                //public XcDateTimeConverter_AwsDateTF()
                //{
                //    base.DateTimeFormat = Core.Constant.Date.FORMAT_DATE_TIME_MSF_TF;
                //}
            }
        }

        #endregion

        #region Generate Id

        /// <summary>
        /// Generates the unique number.
        /// </summary>
        /// <returns>The unique number.</returns>
        public static string GenerateUniqueNumber()
        {
            object lockObject = new object();

            lock (lockObject)
            {
                //get epoch time 
                long epochTime = new DateTimeOffset(DateTime.Now).ToUniversalTime().ToUnixTimeMilliseconds();
                string sEpochTime = epochTime.ToString();

                //reverse the string so that it doesn't have consecutive numbers in case of consecutive access 
                if (epochTime % 2 == 0)
                {
                    char[] epochTimeArray = sEpochTime.ToCharArray();
                    Array.Reverse(epochTimeArray);

                    sEpochTime = new string(epochTimeArray);
                }
                string uniqueNumber = string.Format("{0:X}", sEpochTime.GetHashCode());

                return $"TN-{uniqueNumber}";
            }
        }

        #endregion

        #region File Utils

        /// <summary>
        /// Remove special characters
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static string RemoveSpecialChar(string strValue)
        {
            return strValue.Replace(' ', '_').Replace(',', '_').Replace('\'', '_').Replace('\"', '_').Replace('#', '_').Replace('\\', '_').Replace('/', '_').Replace(':', '_').Replace('?', '_').Replace('<', '_').Replace('>', '_').Replace('|', '_').Replace('.', '_');
        }

        #endregion

        #region Decimal

        public class Amount
        {
            /// <summary>
            /// Decimal Formation
            /// </summary>
            public class Decimal : JsonConverter
            {
                public override bool CanConvert(Type objectType)
                {
                    return objectType == typeof(decimal);
                }

                public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
                {
                    throw new NotImplementedException();
                }

                public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                {
                    writer.WriteRawValue(((decimal)value).ToString("F2", CultureInfo.InvariantCulture));
                }
            }
        }

        #endregion

        #region Object xml Serilize and xml Deserilize

        /// <summary>
        /// Deserializer generic method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public T Deserializer<T>(string xmlString)
        {
            try
            {
                //Deserialize
                TextReader reader = new StringReader(xmlString);
                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                T result = (T)deserializer.Deserialize(reader);

                //return
                return result;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Serializer generic method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize<T>(T obj)
        {
            try
            {
                //Setting namespace
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");

                var stringwriter = new System.IO.StringWriter();
                var serializer = new XmlSerializer(typeof(T));

                //Serializing
                serializer.Serialize(stringwriter, obj, namespaces);

                //return xml as string
                return stringwriter.ToString();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region TimeZone

        public class TimeZone
        {
            public List<Region> GetAllTimeZone()
            {
                List<Region> regionList = new List<Region>()
                {
                    new Region(){ RegionId=3,RegionName="Hawaiian Standard Time"},
                    new Region(){ RegionId=4,RegionName="Alaskan Standard Time"},
                    new Region(){ RegionId=6,RegionName="SA Pacific Standard Time"},
                    new Region(){ RegionId=7,RegionName="Mountain Standard Time"},
                    new Region(){ RegionId=9,RegionName="Mountain Standard Time"},
                    new Region(){ RegionId=10,RegionName="Central Standard Time"},
                    new Region(){ RegionId=15,RegionName="Eastern Standard Time"}

                };
                return regionList;
            }
            public Region GetTimeZoneByRegion(long regionId)
            {
                return GetAllTimeZone().Find(r => r.RegionId == regionId);
            }

        }

        public class Region
        {
            public long RegionId { get; set; }

            public string RegionName { get; set; }
        }

        #endregion

        #region Generate Pdf
        /// <summary>
        /// Generate pdf
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="controllerContext"></param>
        /// <param name="pdfView"></param>
        /// <param name="model"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static byte[] GeneratePdf<T>(ActionContext controllerContext, ViewAsPdf pdfView, T model, string filename)
        {
            try
            {
                //generate pdf
                pdfView.Model = model;
                pdfView.FileName = filename;
                pdfView.IsLowQuality = false;
                pdfView.PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait;
                pdfView.PageSize = Rotativa.AspNetCore.Options.Size.A4;
                pdfView.PageMargins = new Rotativa.AspNetCore.Options.Margins(15, 10, 15, 10);
                //pdfView.WkhtmlPath ="E:/Branch/API/Invoice/1.1/Command/source/wwwroot/Rotativa/wkhtmltopdf.exe";

                //getting file bytes
                byte[] fileBytes = pdfView.BuildFile(controllerContext).Result;
                return fileBytes;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Convert pdf to jpeg
        /// <summary>
        /// get jpeg from pdf
        /// </summary>
        /// <param name="fileContents"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public static dynamic GetJpegFromPDF(byte[] fileContents, Int32 pageNumber)
        {
            MemoryStream mswrite = new MemoryStream();
            PdfReader reader = null;
            try
            {
                byte[] bms = null;
                reader = new PdfReader(fileContents);

                if (reader.NumberOfPages > 1)
                {
                    bms = ExtractPage(fileContents, pageNumber);
                }
                else
                {
                    bms = fileContents;
                }

                MagickReadSettings settings = new MagickReadSettings();
                settings.Density = new Density(500, 500);
                //settings.FillColor = MagickColor.FromRgb(255, 255, 255);


                using (MagickImage image = new MagickImage())
                {
                    image.Read(bms, settings);

                    if (image.Format == MagickFormat.Png)
                    {
                        image.Alpha(AlphaOption.Remove);
                    }

                    //image.SetAttribute("-quality", "40");
                    image.Format = MagickFormat.Jpeg;
                    //image.BackgroundColor = MagickColor.FromRgba(255, 255, 255, 255);

                    image.Write(mswrite);
                }
                SDI.Image img = SDI.Image.FromStream(mswrite);
                byte[] jpeg = ImageToByteArray(img);
                return jpeg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                mswrite.Close();
                reader.Close();
            }
        }

        /// <summary>
        /// convert stream to byte array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] StreamToByteArray(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        #endregion

        #region Image conversion
        /// <summary>
        /// extract page from fil
        /// </summary>
        /// <param name="filecontents"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public static byte[] ExtractPage(byte[] filecontents, int pageNumber)
        {
            iT.pdf.PdfReader reader = null;
            iT.Document document = null;
            iT.pdf.PdfCopy pdfCopyProvider = null;
            iT.pdf.PdfImportedPage importedPage = null;
            try
            {

                reader = new iT.pdf.PdfReader(filecontents);

                document = new iT.Document(reader.GetPageSizeWithRotation(pageNumber));
                // Initialize an instance of the PdfCopyClass with the source
                // document and an output file stream:
                MemoryStream ms = new MemoryStream();

                pdfCopyProvider = new iT.pdf.PdfCopy(document, ms);

                document.Open();
                // Extract the desired page number:
                importedPage = pdfCopyProvider.GetImportedPage(reader, pageNumber);
                pdfCopyProvider.AddPage(importedPage);
                document.Close();
                reader.Close();
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get the page count
        /// </summary>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        public static Int32 GetByteDataPageCount(byte[] fileContents)
        {

            PdfReader reader = null;
            try
            {
                reader = new PdfReader(fileContents);

                return reader.NumberOfPages;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Image to byte array conversion
        /// </summary>
        /// <param name="imageIn"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, ImageFormat.Jpeg);
                bytes = ms.ToArray();
            }
            return bytes;
        }

        /// <summary>
        /// byte array to image conversion
        /// </summary>
        /// <param name="byteArrayIn"></param>
        /// <returns></returns>
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        /// <summary>
        /// image to stream conversion
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static Stream ToStream(System.Drawing.Image image, ImageFormat format)
        {
            var stream = new System.IO.MemoryStream();
            image.Save(stream, format);
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Get width for the jpeg
        /// </summary>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <param name="targetWidth"></param>
        /// <param name="targetHeight"></param>
        /// <returns></returns>
        public static int GetWidthPropotional(int imageWidth, int imageHeight, int targetWidth, int targetHeight)
        {
            int width = targetWidth;
            if (imageWidth > imageHeight)
            {
                width = targetWidth;
            }
            else
            {
                width = (int)((double)(targetHeight * imageWidth) / imageHeight);

            }
            return width;
        }

        /// <summary>
        /// Get height for the jpeg
        /// </summary>
        /// <param name="imageWidth"></param>
        /// <param name="imageHeight"></param>
        /// <param name="targetWidth"></param>
        /// <param name="targetHeight"></param>
        /// <returns></returns>
        public static int GetHeightPropotional(int imageWidth, int imageHeight, int targetWidth, int targetHeight)
        {
            int height = targetHeight;

            if (imageHeight > imageWidth)
            {
                height = targetHeight;
            }
            else
            {
                height = (int)((double)(targetWidth * imageHeight) / imageWidth);

            }
            return height;
        }

        /// <summary>
        /// generate thumbnail image
        /// </summary>
        /// <param name="File"></param>
        /// <param name="thumbWidth"></param>
        /// <param name="thumbHeight"></param>
        /// <returns></returns>
        public static byte[] GenerateThumbImage(byte[] File, int thumbWidth, int thumbHeight)
        {
            Image image = TnUtil.byteArrayToImage(File);
            byte[] jpegFile;

            //thumblarge file creation 
            int thumbImageWidth = TnUtil.GetWidthPropotional(image.Width, image.Height, thumbWidth, thumbHeight);

            int thumbImageHeight = TnUtil.GetHeightPropotional(image.Width, image.Height, thumbWidth, thumbHeight);

            if (image.Width < thumbWidth && image.Height < thumbHeight)
            {
                thumbImageWidth = image.Width;
                thumbImageHeight = image.Height;
            }

            MemoryStream msThmbImage = new MemoryStream();

            using (Image thumbImage = new Bitmap(image, thumbImageWidth, thumbImageHeight))
            {
                thumbImage.Save(msThmbImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                jpegFile = TnUtil.ImageToByteArray(thumbImage);
            }
            msThmbImage.Position = 0;

            return jpegFile;
        }
        #endregion

        #region Enum description

        /// <summary>
        /// Get enum description grom given enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetEnumDescription<T>(string enumValue)
        {
            try
            {

                string description = string.Empty;

                //Activate instance
                var enumType = Activator.CreateInstance<T>();

                //reading description for the 
                description = enumType.GetType()
                    .GetMember(enumValue)
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description;

                //return enum description
                return description;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region UOM Conversion

        public class UOM
        {
            /// <summary>
            /// Convert the specified sourceUOM and targetUOM.
            /// </summary>
            /// <returns>The convert.</returns>
            /// <param name="sourceUOM">Source uom.</param>
            /// <param name="targetUOM">Target uom.</param>
            //public static double ConvertUOM(string sourceUOM, string targetUOM, Core.Enum.UOMType.Type uomType)
            //{
            //    return ConvertUOM(1, sourceUOM, targetUOM, uomType);
            //}

        }

        #endregion

        #region Encryption

        public class EncryptionHelper
        {
            /// <summary>
            /// Encrypt and decrypt
            /// </summary>
            /// <param name="input"></param>
            /// <param name="decrypt"></param>
            /// <returns></returns>
            public static string EnDecrypt(string input, bool decrypt = false)
            {
                string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ984023";

                if (decrypt)
                {
                    Dictionary<string, uint> _index = null;
                    Dictionary<string, Dictionary<string, uint>> _indexes =
                        new Dictionary<string, Dictionary<string, uint>>(2, StringComparer.InvariantCulture);

                    if (_index == null)
                    {
                        Dictionary<string, uint> cidx;

                        string indexKey = "I" + _alphabet;

                        if (!_indexes.TryGetValue(indexKey, out cidx))
                        {
                            lock (_indexes)
                            {
                                if (!_indexes.TryGetValue(indexKey, out cidx))
                                {
                                    cidx = new Dictionary<string, uint>(_alphabet.Length, StringComparer.InvariantCulture);
                                    for (int i = 0; i < _alphabet.Length; i++)
                                    {
                                        cidx[_alphabet.Substring(i, 1)] = (uint)i;
                                    }

                                    _indexes.Add(indexKey, cidx);
                                }
                            }
                        }

                        _index = cidx;
                    }

                    MemoryStream ms = new MemoryStream(Math.Max((int)Math.Ceiling(input.Length * 5 / 8.0), 1));

                    for (int i = 0; i < input.Length; i += 8)
                    {
                        int chars = Math.Min(input.Length - i, 8);

                        ulong val = 0;

                        int bytes = (int)Math.Floor(chars * (5 / 8.0));

                        for (int charOffset = 0; charOffset < chars; charOffset++)
                        {
                            uint cbyte;
                            if (!_index.TryGetValue(input.Substring(i + charOffset, 1), out cbyte))
                            {
                                throw new ArgumentException(string.Format("Invalid character {0} valid characters are: {1}",
                                    input.Substring(i + charOffset, 1), _alphabet));
                            }

                            val |= (((ulong)cbyte) << ((((bytes + 1) * 8) - (charOffset * 5)) - 5));
                        }

                        byte[] buff = BitConverter.GetBytes(val);
                        Array.Reverse(buff);
                        ms.Write(buff, buff.Length - (bytes + 1), bytes);
                    }

                    return System.Text.ASCIIEncoding.ASCII.GetString(ms.ToArray());
                }
                else
                {
                    byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(input);

                    StringBuilder result = new StringBuilder(Math.Max((int)Math.Ceiling(data.Length * 8 / 5.0), 1));

                    byte[] emptyBuff = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    byte[] buff = new byte[8];

                    for (int i = 0; i < data.Length; i += 5)
                    {
                        int bytes = Math.Min(data.Length - i, 5);

                        Array.Copy(emptyBuff, buff, emptyBuff.Length);
                        Array.Copy(data, i, buff, buff.Length - (bytes + 1), bytes);
                        Array.Reverse(buff);
                        ulong val = BitConverter.ToUInt64(buff, 0);

                        for (int bitOffset = ((bytes + 1) * 8) - 5; bitOffset > 3; bitOffset -= 5)
                        {
                            result.Append(_alphabet[(int)((val >> bitOffset) & 0x1f)]);
                        }
                    }


                    return result.ToString();
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="input"></param>
            /// <param name="password"></param>
            /// <param name="decrypt"></param>
            /// <returns></returns>
            public static string AES_EnDecrypt(string input, string password, bool decrypt = false)
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                byte[] saltBytes = new byte[] { 21, 52, 23, 34, 85, 56, 37, 38, 34, 66, 77, 37, 23, 34, 23, 19 };

                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Mode = CipherMode.CBC;
                    AES.Padding = PaddingMode.PKCS7;

                    var key = new Rfc2898DeriveBytes(password, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    //encrypt string
                    if (!decrypt)
                    {
                        byte[] inputBytes = Encoding.Unicode.GetBytes(input);
                        using (var encryptor = AES.CreateEncryptor())
                        {
                            var cypherText = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                            return Convert.ToBase64String(cypherText);
                        }
                    }
                    else
                    {
                        //decrypt string
                        byte[] encryptedBytes = Convert.FromBase64String(input);
                        using (var decryptor = AES.CreateDecryptor())
                        {
                            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                            return Encoding.Unicode.GetString(decryptedBytes);
                        }
                    }
                }
            }

            public static string EnDecryptwithCDV(string input, bool decrypt = false)
            {
                string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ984023";

                if (decrypt)
                {
                    Dictionary<string, uint> _index = null;
                    Dictionary<string, Dictionary<string, uint>> _indexes =
                        new Dictionary<string, Dictionary<string, uint>>(2, StringComparer.InvariantCulture);

                    if (_index == null)
                    {
                        Dictionary<string, uint> cidx;

                        string indexKey = "I" + _alphabet;

                        if (!_indexes.TryGetValue(indexKey, out cidx))
                        {
                            lock (_indexes)
                            {
                                if (!_indexes.TryGetValue(indexKey, out cidx))
                                {
                                    cidx = new Dictionary<string, uint>(_alphabet.Length, StringComparer.InvariantCulture);
                                    for (int i = 0; i < _alphabet.Length; i++)
                                    {
                                        cidx[_alphabet.Substring(i, 1)] = (uint)i;
                                    }

                                    _indexes.Add(indexKey, cidx);
                                }
                            }
                        }

                        _index = cidx;
                    }

                    MemoryStream ms = new MemoryStream(Math.Max((int)Math.Ceiling(input.Length * 5 / 8.0), 1));

                    for (int i = 0; i < input.Length; i += 8)
                    {
                        int chars = Math.Min(input.Length - i, 8);

                        ulong val = 0;

                        int bytes = (int)Math.Floor(chars * (5 / 8.0));

                        for (int charOffset = 0; charOffset < chars; charOffset++)
                        {
                            uint cbyte;
                            if (!_index.TryGetValue(input.Substring(i + charOffset, 1), out cbyte))
                            {
                                throw new ArgumentException(string.Format("Invalid character {0} valid characters are: {1}",
                                    input.Substring(i + charOffset, 1), _alphabet));
                            }

                            val |= (((ulong)cbyte) << ((((bytes + 1) * 8) - (charOffset * 5)) - 5));
                        }

                        byte[] buff = BitConverter.GetBytes(val);
                        Array.Reverse(buff);
                        ms.Write(buff, buff.Length - (bytes + 1), bytes);
                    }

                    string invoiceindex = string.Empty;
                    if (CheckSumDigit.CheckCDV(System.Text.ASCIIEncoding.ASCII.GetString(ms.ToArray()), out invoiceindex))
                    {
                        return invoiceindex;
                    }
                    else
                    {
                        throw new ArgumentException("Invalid URL");
                    }
                }
                else
                {
                    string encryptedinput = CheckSumDigit.AppendCDV(input);
                    byte[] data = System.Text.ASCIIEncoding.ASCII.GetBytes(encryptedinput);

                    StringBuilder result = new StringBuilder(Math.Max((int)Math.Ceiling(data.Length * 8 / 5.0), 1));

                    byte[] emptyBuff = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
                    byte[] buff = new byte[8];

                    for (int i = 0; i < data.Length; i += 5)
                    {
                        int bytes = Math.Min(data.Length - i, 5);

                        Array.Copy(emptyBuff, buff, emptyBuff.Length);
                        Array.Copy(data, i, buff, buff.Length - (bytes + 1), bytes);
                        Array.Reverse(buff);
                        ulong val = BitConverter.ToUInt64(buff, 0);

                        for (int bitOffset = ((bytes + 1) * 8) - 5; bitOffset > 3; bitOffset -= 5)
                        {
                            result.Append(_alphabet[(int)((val >> bitOffset) & 0x1f)]);
                        }
                    }


                    return result.ToString();
                }
            }
        }

        #endregion

        #region CheckSumDigit
        class CheckSumDigit
        {
            static readonly int[][] op = new int[10][];
            static readonly int[][] F = new int[8][];
            static readonly int[] inv = { 0, 4, 3, 2, 1, 5, 6, 7, 8, 9 };

            static CheckSumDigit()
            {

                op[0] = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                op[1] = new int[] { 1, 2, 3, 4, 0, 6, 7, 8, 9, 5 };
                op[2] = new int[] { 2, 3, 4, 0, 1, 7, 8, 9, 5, 6 };
                op[3] = new int[] { 3, 4, 0, 1, 2, 8, 9, 5, 6, 7 };
                op[4] = new int[] { 4, 0, 1, 2, 3, 9, 5, 6, 7, 8 };
                op[5] = new int[] { 5, 9, 8, 7, 6, 0, 4, 3, 2, 1 };
                op[6] = new int[] { 6, 5, 9, 8, 7, 1, 0, 4, 3, 2 };
                op[7] = new int[] { 7, 6, 5, 9, 8, 2, 1, 0, 4, 3 };
                op[8] = new int[] { 8, 7, 6, 5, 9, 3, 2, 1, 0, 4 };
                op[9] = new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 };

                F[0] = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                F[1] = new int[] { 1, 5, 7, 6, 2, 8, 3, 0, 9, 4 };
                for (int i = 2; i < 8; i++)
                {
                    F[i] = new int[10];
                    for (int j = 0; j < 10; j++)
                        F[i][j] = F[i - 1][F[1][j]];
                }
            }

            public static int GetCDV(string digits)
            {
                int[] reversedInput = digits.Select(n => Convert.ToInt32(int.Parse(n.ToString()))).ToArray().Reverse().ToArray();
                int check = 0;
                for (int i = 0; i < reversedInput.Length; i++)
                {
                    check = op[check][F[(i + 1) % 8][reversedInput[i]]];
                }
                int checkDigit = inv[check];

                return checkDigit;
            }

            public static string AppendCDV(string digits)
            {
                return $"{digits}{GetCDV(digits)}";
            }

            public static bool CheckCDV(string digits, out string value)
            {
                value = string.Empty;
                int[] reversedInput = digits.Select(n => Convert.ToInt32(int.Parse(n.ToString()))).ToArray().Reverse().ToArray();
                int check = 0;
                for (int i = 0; i < reversedInput.Length; i++)
                {
                    check = op[check][F[i % 8][reversedInput[i]]];
                }

                if (check == 0)
                {
                    value = digits.Remove(digits.Length - 1);
                    return true;
                }else
                {
                    return false;
                }
            }
        }
        #endregion

        #region ConnectionString
        public class DecryptConnection
        {
            public static string SetConnectionString(string connectionString)
            {
                var builder = new SqlConnectionStringBuilder(connectionString);
                builder.Password = EncryptionHelper.EnDecrypt(builder.Password, true);
                return builder.ToString();
            }
        }

        #endregion
    }
}
