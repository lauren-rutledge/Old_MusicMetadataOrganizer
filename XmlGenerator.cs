using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataOrganizer
{
    internal static class XmlGenerator
    {
        private const string clientId = "834201935-7F828E5FFAED9EFA8048694BBCD7BD05";
        private const string userId = "34438951537836737-80CC83E70D67748C63BF50EA6420C214";

        internal static string CreateRequest(string artist, string title, string album)
        {
            var validArtist = System.Security.SecurityElement.Escape(artist);
            var validTitle = System.Security.SecurityElement.Escape(title);
            var validAlbum = System.Security.SecurityElement.Escape(album);

            return $"<QUERIES><AUTH><CLIENT>{clientId}</CLIENT><USER>{userId}</USER></AUTH><LANG>eng</LANG>" +
                $"<QUERY CMD=\"ALBUM_SEARCH\"><MODE>SINGLE_BEST</MODE><TEXT TYPE=\"ARTIST\">{validArtist}</TEXT>" +
                $"<TEXT TYPE=\"ALBUM_TITLE\">{validAlbum}</TEXT><TEXT TYPE=\"TRACK_TITLE\">{validTitle}</TEXT></QUERY></QUERIES>";
        }

        internal static string CreateRequest(string artist, string title)
        {
            var validArtist = System.Security.SecurityElement.Escape(artist);
            var validTitle = System.Security.SecurityElement.Escape(title);

            return $"<QUERIES><AUTH><CLIENT>{clientId}</CLIENT><USER>{userId}</USER></AUTH><LANG>eng</LANG>" +
                $"<QUERY CMD=\"ALBUM_SEARCH\"><MODE>SINGLE_BEST</MODE><TEXT TYPE=\"ARTIST\">{validArtist}</TEXT>" +
                $"<TEXT TYPE=\"ALBUM_TITLE\"></TEXT><TEXT TYPE=\"TRACK_TITLE\">{validTitle}</TEXT></QUERY></QUERIES>";
        }
    }
}
