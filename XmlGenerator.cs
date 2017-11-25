using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataOrganizer
{
    public static class XmlGenerator
    {
        private const string userId = "34438951537836737-80CC83E70D67748C63BF50EA6420C214";
        private const string clientId = "834201935-7F828E5FFAED9EFA8048694BBCD7BD05";

        public static string CreateRequest(string artist, string title, string album)
        {
            return $"<QUERIES><AUTH><CLIENT>{clientId}</CLIENT><USER>{userId}</USER></AUTH><LANG>eng</LANG>" +
                $"<QUERY CMD=\"ALBUM_SEARCH\"><MODE>SINGLE_BEST</MODE><TEXT TYPE=\"ARTIST\">{artist}</TEXT>" +
                $"<TEXT TYPE=\"ALBUM_TITLE\">{album}</TEXT><TEXT TYPE=\"TRACK_TITLE\">{title}</TEXT></QUERY></QUERIES>";
        }

        public static string CreateRequest(string artist, string title)
        {
            return $"<QUERIES><AUTH><CLIENT>{clientId}</CLIENT><USER>{userId}</USER></AUTH><LANG>eng</LANG>" +
                $"<QUERY CMD=\"ALBUM_SEARCH\"><MODE>SINGLE_BEST</MODE><TEXT TYPE=\"ARTIST\">{artist}</TEXT>" +
                $"<TEXT TYPE=\"ALBUM_TITLE\"></TEXT><TEXT TYPE=\"TRACK_TITLE\">{title}</TEXT></QUERY></QUERIES>";
        }
    }
}
