using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using ParkSquare.Gracenote;
using ParkSquare.Gracenote.DataTransfer;

namespace MusicMetadataOrganizer
{
    public static class GracenoteWebAPI
    {
        private static System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        private const string destinationUrl = "https://c834201935.web.cddbp.net/webapi/xml/1.0/";

        private static string PostXmlData(string requestXml)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
            byte[] bytes = Encoding.ASCII.GetBytes(requestXml);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            request.Method = "POST";
            try
            {
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    return responseStr;
                }
            }
            catch (Exception ex)
            {
                var log = new LogWriter($"Could not connect to the Gracenote web API. " + ex.Message);
            }
            return null;
        }

        public static RESPONSE Query(MasterFile file)
        {
            var artist = file.TagLibProps["Artist"].ToString();
            var songTitle = file.TagLibProps["Title"].ToString();
            var album = file.TagLibProps["Album"].ToString();
            var xml = XmlGenerator.CreateRequest(artist, songTitle, album);
            var result = PostXmlData(xml);
            if (String.IsNullOrEmpty(result))
            {
                var log = new LogWriter($"Received a null result from the PostXMLData() method. ArgumentNullException- Application terminated.");
                throw new ArgumentNullException(nameof(result));
            }
            return XmlParser.XmlToObject(result)[0];
        }
    }

    public class RESPONSE
    {
        public ALBUM ALBUM { get; set; }

        [Obsolete("Potentially swapping to CheckEquality method only.")]
        public bool Equals(MasterFile file)
        {
            bool isEqual = true;
            foreach (var record in CheckEquality(file))
            {
                if (record.Value == false)
                    isEqual = false;
            }
            return isEqual;
        }

        public Dictionary<string, bool> CheckEquality(MasterFile file)
        {
            return new Dictionary<string, bool>()
            {
                { "Artist", file.TagLibProps["Artist"].ToString() == StringCleaner.ToActualTitleCase(ALBUM.ARTIST) ? true : false },
                { "Album",  file.TagLibProps["Album"].ToString() == StringCleaner.ToActualTitleCase(ALBUM.TITLE) ? true : false },
                { "Title", file.TagLibProps["Title"].ToString() == StringCleaner.ToActualTitleCase(ALBUM.TRACK.TITLE) ? true : false },
                { "Track",  Convert.ToInt32(file.TagLibProps["Track"]) == ALBUM.TRACK.TRACK_NUM ? true : false },
                { "Year", file.TagLibProps["Year"].ToString() == ALBUM.DATE ? true : false }
            };
        }
    }

    public class ALBUM
    {
        public string GN_ID { get; set; }
        public string ARTIST { get; set; }
        public string TITLE { get; set; }
        public string PKG_LANG { get; set; }
        public string DATE { get; set; }
        public string GENRE { get; set; }
        public int MATCHED_TRACK_NUM { get; set; }
        public int TRACK_COUNT { get; set; }
        public TRACK TRACK { get; set; }
    }

    public class TRACK
    {
        public int TRACK_NUM { get; set; }
        public string GN_ID { get; set; }
        public string TITLE { get; set; }
    }
}
