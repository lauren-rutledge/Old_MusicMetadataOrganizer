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
        static System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
        public static string destinationUrl = "https://c834201935.web.cddbp.net/webapi/xml/1.0/";

        public static string PostXMLData(string destinationUrl, string requestXml)
        {
            /*
            // Using Gracenote library (under construction)
            string clientID = "834201935-7F828E5FFAED9EFA8048694BBCD7BD05";
            string userID = "34438951537836737-80CC83E70D67748C63BF50EA6420C214";
            Endpoint endpoint = new Endpoint(clientID);
            ParkSquare.Gracenote.HttpClient client2 = new ParkSquare.Gracenote.HttpClient(endpoint);
            Client dataTransferClient = new Client(clientID);
            Query query = new Query(requestXml, dataTransferClient);
            // Must include your Gracenote client ID string and User ID string in the AUTH block of each query.
            Queries queries = new Queries()
            {
                Auth = new Auth(clientID, userID),
                Lang = "eng",
                Query = query
            };
            var result = client2.Post(queries);
            return result.ToString();
            */
            
            // Using standard code (works)
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

        public static RESPONSE Query (MasterFile file)
        {
            var artist = file.TagLibProps["Artist"].ToString();
            var songTitle = file.TagLibProps["Title"].ToString();
            var album = file.TagLibProps["Album"].ToString();

            // Do some null check for result variable
            var xml = XmlGenerator.CreateRequest(artist, songTitle, album);
            var result = PostXMLData("https://c834201935.web.cddbp.net/webapi/xml/1.0/", xml);
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

        public bool Equals(MasterFile file)
        {
            if (file.TagLibProps["Artist"].ToString() == this.ALBUM.ARTIST &&
                file.TagLibProps["Album"].ToString() == this.ALBUM.TITLE &&
                file.TagLibProps["Title"].ToString() == this.ALBUM.TRACK.TITLE &&
                Convert.ToInt32(file.TagLibProps["Track"]) == this.ALBUM.TRACK.TRACK_NUM)
                return true;
            return false;
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
