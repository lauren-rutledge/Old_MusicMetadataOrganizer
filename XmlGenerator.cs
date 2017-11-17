using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataOrganizer
{
    public static class XmlGenerator
    {
        public static string CreateRequest(string artist, string title, string album)
        {
            return "<QUERIES><AUTH><CLIENT>834201935-7F828E5FFAED9EFA8048694BBCD7BD05</CLIENT>" +
                "<USER>34438951537836737-80CC83E70D67748C63BF50EA6420C214</USER></AUTH><LANG>eng</LANG>" +
                $"<QUERY CMD=\"ALBUM_SEARCH\"><MODE>SINGLE_BEST</MODE><TEXT TYPE=\"ARTIST\">{artist}</TEXT>" +
                $"<TEXT TYPE=\"ALBUM_TITLE\">{album}</TEXT><TEXT TYPE=\"TRACK_TITLE\">{title}</TEXT></QUERY></QUERIES>";
        }
    }
}
