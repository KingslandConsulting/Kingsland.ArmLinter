using Kingsland.ArmLinter.Functions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;

namespace Kingsland.ArmLinter.Tests.Functions
{

    public static class DataUriTests
    {

        public static class ParseTests
        {

            /// <summary>
            /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#examples-6
            /// </summary>
            [Test]
            public static void DataUriFunctionExample1ParseTest()
            {
                var actual = DataUri.Parse("data:text/plain;charset=utf8;base64,SGVsbG8=");
                var expected = new DataUri(
                    mediaType: "text/plain",
                    parameters: new Dictionary<string, string> {
                        { "charset", "utf8" }
                    },
                    data: Encoding.UTF8.GetBytes("Hello")
                );
                Assert.AreEqual(expected.MediaType, actual.MediaType);
                Assert.AreEqual(expected.Parameters, actual.Parameters);
                Assert.AreEqual(expected.Data, actual.Data);
            }

            /// <summary>
            /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#examples-6
            /// </summary>
            [Test]
            public static void DataUriFunctionExample2ParseTest()
            {
                var actual = DataUri.Parse("data:;base64,SGVsbG8sIFdvcmxkIQ==");
                var expected = new DataUri(
                    mediaType: string.Empty,
                    parameters: new Dictionary<string, string>(),
                    data: Encoding.UTF8.GetBytes("Hello, World!")
                );
                Assert.AreEqual(expected.MediaType, actual.MediaType);
                Assert.AreEqual(expected.Parameters, actual.Parameters);
                Assert.AreEqual(expected.Data, actual.Data);
            }

            /// <summary>
            /// See https://en.wikipedia.org/wiki/Data_URI_scheme
            /// </summary>
            [Test]
            public static void WikipediaExample1ParseTest()
            {
                var actual = DataUri.Parse("data:text/vnd-example+xyz;foo=bar;base64,R0lGODdh");
                var expected = new DataUri(
                    mediaType: "text/vnd-example+xyz",
                    parameters: new Dictionary<string, string> {
                        { "foo", "bar" }
                    },
                    data: Encoding.ASCII.GetBytes("GIF87a")
                );
                Assert.AreEqual(expected.MediaType, actual.MediaType);
                Assert.AreEqual(expected.Parameters, actual.Parameters);
                Assert.AreEqual(expected.Data, actual.Data);
            }

            /// <summary>
            /// See https://en.wikipedia.org/wiki/Data_URI_scheme
            /// </summary>
            [Test]
            public static void WikipediaExample2ParseTest()
            {
                var actual = DataUri.Parse("data:text/plain;charset=UTF-8;page=21,the%20data:1234,5678");
                var expected = new DataUri(
                    mediaType: "text/plain",
                    parameters: new Dictionary<string, string> {
                        { "charset", "UTF-8" },
                        { "page", "21" },
                    },
                    data: Encoding.ASCII.GetBytes("the data:1234,5678")
                );
                Assert.AreEqual(expected.MediaType, actual.MediaType);
                Assert.AreEqual(expected.Parameters, actual.Parameters);
                Assert.AreEqual(expected.Data, actual.Data);
            }

            /// <summary>
            /// See https://en.wikipedia.org/wiki/Data_URI_scheme
            /// </summary>
            [Test]
            public static void WikipediaHtmlExampleParseTest()
            {
                // <img src="data:image/png;base64,iVBORw0KGgoAAA
                // ANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4
                // //8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU
                // 5ErkJggg==" alt="Red dot" />
                var actual = DataUri.Parse(
                    "data:image/png;base64,iVBORw0KGgoAAA" +
                    "ANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4" +
                    "//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU" +
                    "5ErkJggg=="
                );
                var expected = new DataUri(
                    mediaType: "image/png",
                    parameters: new Dictionary<string, string>(),
                    data: new byte[] {
                        137,  80,  78, 71,  13,  10,  26, 10,   0,   0,   0,  13,  73,  72,  68, 82,
                          0,   0,   0,  5,   0,   0,   0,  5,   8,   6,   0,   0,   0, 141, 111, 38,
                        229,   0,   0,  0,  28,  73,  68, 65,  84,   8, 215,  99, 248, 255, 255, 63,
                        195, 127,   6, 32,   5, 195,  32, 18, 132, 208,  49, 241, 130,  88, 205,  4,
                          0,  14, 245, 53, 203, 209, 142, 14,  31,   0,   0,   0,   0,  73,  69, 78,
                         68,  174, 66, 96, 130
                    }
                );
                Assert.AreEqual(expected.MediaType, actual.MediaType);
                Assert.AreEqual(expected.Parameters, actual.Parameters);
                Assert.AreEqual(expected.Data, actual.Data);
            }

            /// <summary>
            /// See https://en.wikipedia.org/wiki/Data_URI_scheme
            /// </summary>
            [Test]
            public static void WikipediaCssExampleParseTest()
            {
                // ul.checklist li.complete {
                //     padding-left: 20px;
                //     background: white url('data:image/png;base64,iVB\
                // ORw0KGgoAAAANSUhEUgAAABAAAAAQAQMAAAAlPW0iAAAABlBMVEU\
                // AAAD///+l2Z/dAAAAM0lEQVR4nGP4/5/h/1+G/58ZDrAz3D/McH8\
                // yw83NDDeNGe4Ug9C9zwz3gVLMDA/A6P9/AFGGFyjOXZtQAAAAAEl\
                // FTkSuQmCC') no-repeat scroll left top;
                // }
                var actual = DataUri.Parse(
                    "data:image/png;base64,iVB" +
                    "ORw0KGgoAAAANSUhEUgAAABAAAAAQAQMAAAAlPW0iAAAABlBMVEU" +
                    "AAAD///+l2Z/dAAAAM0lEQVR4nGP4/5/h/1+G/58ZDrAz3D/McH8" +
                    "yw83NDDeNGe4Ug9C9zwz3gVLMDA/A6P9/AFGGFyjOXZtQAAAAAEl" +
                    "FTkSuQmCC"
                );
                var expected = new DataUri(
                    mediaType: "image/png",
                    parameters: new Dictionary<string, string>(),
                    data: new byte[] {
                        137,  80,  78,  71,  13,  10,  26,  10,   0,   0,   0,  13,  73,  72,  68,  82,
                          0,   0,   0,  16,   0,   0,   0,  16,   1,   3,   0,   0,   0,  37,  61, 109,
                         34,   0,   0,   0,   6,  80,  76,  84,  69,   0,   0,   0, 255, 255, 255, 165,
                        217, 159, 221,   0,   0,   0,  51,  73,  68,  65,  84, 120, 156,  99, 248, 255,
                        159, 225, 255,  95, 134, 255, 159,  25,  14, 176,  51, 220,  63, 204, 112, 127,
                         50, 195, 205, 205,  12,  55, 141,  25, 238,  20, 131, 208, 189, 207,  12, 247,
                        129,  82, 204,  12,  15, 192, 232, 255, 127,   0,  81, 134,  23,  40, 206,  93,
                        155,  80,   0,   0,   0,   0,  73,  69,  78,  68, 174,  66,  96, 130
                    }
                );
                Assert.AreEqual(expected.MediaType, actual.MediaType);
                Assert.AreEqual(expected.Parameters, actual.Parameters);
                Assert.AreEqual(expected.Data, actual.Data);
            }


            /// <summary>
            /// See https://tools.ietf.org/html/rfc2397
            /// </summary>
            [Test]
            public static void Rfc2397ExampleParseTest()
            {
                var actual = DataUri.Parse(
                    "data:,A%20brief%20note"
                );
                var expected = new DataUri(
                    mediaType: string.Empty,
                    parameters: new Dictionary<string, string>(),
                    data: new byte[] {
                        65, 32, 98, 114, 105, 101, 102, 32, 110, 111, 116, 101
                    }
                );
                Assert.AreEqual(expected.MediaType, actual.MediaType);
                Assert.AreEqual(expected.Parameters, actual.Parameters);
                Assert.AreEqual(expected.Data, actual.Data);
            }

        }

        public static class ToStringTests
        {

            /// <summary>
            /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#examples-6
            /// </summary>
            [Test]
            public static void DataUriFunctionExample1ToStringTest()
            {
                var actual = new DataUri(
                    mediaType: "text/plain",
                    parameters: new Dictionary<string, string> { { "charset", "utf8" } },
                    data: Encoding.UTF8.GetBytes("Hello")
                ).ToString(true);
                var expected = "data:text/plain;charset=utf8;base64,SGVsbG8=";
                Assert.AreEqual(expected, actual);
            }

            /// <summary>
            /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#examples-6
            /// </summary>
            [Test]
            public static void DataUriFunctionExample2ToStringTest()
            {
                var actual = new DataUri(
                    mediaType: null,
                    parameters: null,
                    data: Encoding.UTF8.GetBytes("Hello, World!")
                ).ToString(true);
                var expected = "data:;base64,SGVsbG8sIFdvcmxkIQ==";
                Assert.AreEqual(expected, actual);
            }

            /// <summary>
            /// See https://en.wikipedia.org/wiki/Data_URI_scheme
            /// </summary>
            [Test]
            public static void WikipediaExample1ToStringTest()
            {
                var actual = new DataUri(
                    mediaType: "text/vnd-example+xyz",
                    parameters: new Dictionary<string, string> { { "foo", "bar" } },
                    data: Encoding.ASCII.GetBytes("GIF87a")
                ).ToString(true);
                var expected = "data:text/vnd-example+xyz;foo=bar;base64,R0lGODdh";
                Assert.AreEqual(expected, actual);
            }

            /// <summary>
            /// See https://en.wikipedia.org/wiki/Data_URI_scheme
            /// </summary>
            [Test]
            public static void WikipediaExample2ToStringTest()
            {
                var actual = new DataUri(
                    mediaType: "text/plain",
                    parameters: new Dictionary<string, string> {
                        { "charset", "UTF-8" },
                        { "page", "21" },
                    },
                    data: Encoding.ASCII.GetBytes("the data:1234,5678")
                ).ToString(false);
                var expected = "data:text/plain;charset=UTF-8;page=21,the%20data:1234,5678";
                Assert.AreEqual(expected, actual);
            }

            /// <summary>
            /// See https://en.wikipedia.org/wiki/Data_URI_scheme
            /// </summary>
            [Test]
            public static void WikipediaHtmlExampleToStringTest()
            {
                var actual = new DataUri(
                    mediaType: "image/png",
                    parameters: null,
                    data: new byte[] {
                        137,  80,  78, 71,  13,  10,  26, 10,   0,   0,   0,  13,  73,  72,  68, 82,
                          0,   0,   0,  5,   0,   0,   0,  5,   8,   6,   0,   0,   0, 141, 111, 38,
                        229,   0,   0,  0,  28,  73,  68, 65,  84,   8, 215,  99, 248, 255, 255, 63,
                        195, 127,   6, 32,   5, 195,  32, 18, 132, 208,  49, 241, 130,  88, 205,  4,
                          0,  14, 245, 53, 203, 209, 142, 14,  31,   0,   0,   0,   0,  73,  69, 78,
                         68,  174, 66, 96, 130
                    }
                ).ToString(true);
                // <img src="data:image/png;base64,iVBORw0KGgoAAA
                // ANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4
                // //8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU
                // 5ErkJggg==" alt="Red dot" />
                var expected =
                    "data:image/png;base64,iVBORw0KGgoAAA" +
                    "ANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4" +
                    "//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU" +
                    "5ErkJggg==";
                Assert.AreEqual(expected, actual);
            }

            /// <summary>
            /// See https://en.wikipedia.org/wiki/Data_URI_scheme
            /// </summary>
            [Test]
            public static void WikipediaCssExampleToStringTest()
            {
                var actual = new DataUri(
                    mediaType: "image/png",
                    parameters: null,
                    data: new byte[] {
                        137,  80,  78,  71,  13,  10,  26,  10,   0,   0,   0,  13,  73,  72,  68,  82,
                          0,   0,   0,  16,   0,   0,   0,  16,   1,   3,   0,   0,   0,  37,  61, 109,
                         34,   0,   0,   0,   6,  80,  76,  84,  69,   0,   0,   0, 255, 255, 255, 165,
                        217, 159, 221,   0,   0,   0,  51,  73,  68,  65,  84, 120, 156,  99, 248, 255,
                        159, 225, 255,  95, 134, 255, 159,  25,  14, 176,  51, 220,  63, 204, 112, 127,
                         50, 195, 205, 205,  12,  55, 141,  25, 238,  20, 131, 208, 189, 207,  12, 247,
                        129,  82, 204,  12,  15, 192, 232, 255, 127,   0,  81, 134,  23,  40, 206,  93,
                        155,  80,   0,   0,   0,   0,  73,  69,  78,  68, 174,  66,  96, 130
                    }
                ).ToString(true);
                // ul.checklist li.complete {
                //     padding-left: 20px;
                //     background: white url('data:image/png;base64,iVB\
                // ORw0KGgoAAAANSUhEUgAAABAAAAAQAQMAAAAlPW0iAAAABlBMVEU\
                // AAAD///+l2Z/dAAAAM0lEQVR4nGP4/5/h/1+G/58ZDrAz3D/McH8\
                // yw83NDDeNGe4Ug9C9zwz3gVLMDA/A6P9/AFGGFyjOXZtQAAAAAEl\
                // FTkSuQmCC') no-repeat scroll left top;
                // }
                var expected =
                    "data:image/png;base64,iVB" +
                    "ORw0KGgoAAAANSUhEUgAAABAAAAAQAQMAAAAlPW0iAAAABlBMVEU" +
                    "AAAD///+l2Z/dAAAAM0lEQVR4nGP4/5/h/1+G/58ZDrAz3D/McH8" +
                    "yw83NDDeNGe4Ug9C9zwz3gVLMDA/A6P9/AFGGFyjOXZtQAAAAAEl" +
                    "FTkSuQmCC";
                Assert.AreEqual(expected, actual);
            }

            /// <summary>
            /// See https://tools.ietf.org/html/rfc2397
            /// </summary>
            [Test]
            public static void Rfc2397ExampleToString()
            {
                var actual = new DataUri(
                    mediaType: string.Empty,
                    parameters: new Dictionary<string, string>(),
                    data: new byte[] {
                        65, 32, 98, 114, 105, 101, 102, 32, 110, 111, 116, 101
                    }
                ).ToString(false);
                var expected = "data:,A%20brief%20note";
                Assert.AreEqual(expected, actual);
            }

        }

    }

}
