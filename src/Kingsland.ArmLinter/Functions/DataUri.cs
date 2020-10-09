using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// See https://tools.ietf.org/html/rfc2397
    ///     https://en.wikipedia.org/wiki/Data_URI_scheme
    /// </summary>
    public sealed class DataUri
    {

        // dataurl    := "data:" [ mediatype ] [ ";base64" ] "," data
        // mediatype  := [type "/" subtype] *( ";" parameter )
        // data       := *urlchar
        // parameter  := attribute "=" value

        #region Constructors

        public DataUri(byte[] data)
            : this("text/plain", new Dictionary<string, string> { { "charset", "US-ASCII" } },  data)
        {
        }

        public DataUri(string mediaType, Dictionary<string, string> parameters, byte[] data)
        {
            this.MediaType = mediaType;
            this.Parameters = new ReadOnlyDictionary<string, string>(
                parameters ?? new Dictionary<string, string>()
            );
            this.Data = data;
        }

        #endregion

        #region Properties

        public string MediaType
        {
            get;
            private set;
        }

        public ReadOnlyDictionary<string, string> Parameters
        {
            get;
            private set;
        }

        public byte[] Data
        {
            get;
            private set;
        }

        #endregion

        #region Parse

        public static DataUri Parse(string dataUri)
        {

            // data:text/plain;charset=UTF-8;page=21,the%20data:1234,5678
            // data:text/plain;charset=utf8;base64,SGVsbG8=
            // data:;base64,SGVsbG8sIFdvcmxkIQ==
            // data:,A%20brief%20note

            // data:
            if (!dataUri.StartsWith("data:"))
            {
                throw new ArgumentException("data uri must start with 'data:'", nameof(dataUri));
            }
            dataUri = dataUri.Substring("data:".Length);

            // media type
            var mediaType = string.Empty;
            var index = dataUri.IndexOf(";");
            if (index > -1)
            {
                mediaType = dataUri.Substring(0, index);
                dataUri = dataUri.Substring(index + 1);
            }

            // parameters
            var parameters = new Dictionary<string, string>();

            // all parts except the last one are basic key-value pairs
            index = dataUri.IndexOf(";");
            while (index > -1)
            {
                // add the next parameter
                var nextPart = dataUri.Substring(0, index);
                var kvp = nextPart.Split("=");
                parameters.Add(kvp[0], kvp[1]);
                // remove it from the remaining string
                dataUri = dataUri.Substring(index + 1);
                index = dataUri.IndexOf(";");
            }

            // the last part could be one of three options
            // "<base64>,<data>"
            // "<key>=<value>,data"
            // ",data"
            var data = default(byte[]);
            var parts = dataUri.Split(",");
            if (parts[0].Length == 0)
            {
                // ",data"
                data = Encoding.ASCII.GetBytes(
                    Uri.UnescapeDataString(parts[1])
                );
            }
            else if(parts[0] == "base64")
            {
                // "<base64>,<data>"
                data = Convert.FromBase64String(
                    Uri.UnescapeDataString(parts[1])
                );
            }
            else
            {
                // "<key>=<value>,data"
                var kvp = parts[0].Split("=");
                parameters.Add(kvp[0], kvp[1]);
                data = Encoding.ASCII.GetBytes(
                    Uri.UnescapeDataString(
                        dataUri.Substring(parts[0].Length + ",".Length)
                    )
                );
            }

            return new DataUri(
                mediaType: mediaType,
                parameters: parameters,
                data: data
            );

        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return this.ToString(false);
        }

        public string ToString(bool base64)
        {
            var dataUri = new StringBuilder();
            // scheme
            dataUri.Append("data:");
            // media type
            var mediaTypes = new List<string>();
            mediaTypes.Add(this.MediaType);
            mediaTypes.AddRange(
                this.Parameters.Select(
                    kvp => $"{kvp.Key}={kvp.Value}"
                )
            );
            dataUri.Append(string.Join(';', mediaTypes));
            // ;base64
            if (base64)
            {
                dataUri.Append(";base64");
            }
            // ,data
            var data = base64 ?
                Convert.ToBase64String(this.Data) :
                Encoding.ASCII.GetString(this.Data);
            dataUri.Append(",");
            // https://stackoverflow.com/a/21771206/3156906
            // [space] => %20
            dataUri.Append(Uri.EscapeUriString(data));
            // return the result
            return dataUri.ToString();
        }

        #endregion

    }

}
