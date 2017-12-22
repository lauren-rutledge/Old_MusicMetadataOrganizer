using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMetadataOrganizer
{
    public class GracenoteSong : IEnumerable<string>
    {
        private RESPONSE _response;
        public RESPONSE Response { get { return _response; } }
        private string _artist;
        public string Artist { get { return _artist; } }
        private string _album;
        public string Album { get { return _album; } }
        private string _title;
        public string Title { get { return _title; } }
        private int _track;
        public int Track { get { return _track; } }
        private string _year;
        public string Year { get { return _year; } }
        private string _genres;
        public string Genres { get { return _genres; } }

        public GracenoteSong(RESPONSE response)
        {
            _response = response;
            _artist = StringCleaner.RemoveInvalidDirectoryChars(StringCleaner.ToActualTitleCase(response.ALBUM.ARTIST));
            _album = StringCleaner.RemoveInvalidDirectoryChars(StringCleaner.ToActualTitleCase(response.ALBUM.TITLE));
            _title = StringCleaner.RemoveInvalidFileNameCharacters(StringCleaner.ToActualTitleCase(response.ALBUM.TRACK.TITLE));
            _track = response.ALBUM.TRACK.TRACK_NUM;
            _year = response.ALBUM.DATE;
            _genres = response.ALBUM.GENRE;
        }

        [Obsolete("Potentially swapping to CheckEquality method only.")]
        public bool Equals(MasterFile file)
        {
            bool isEqual = true;
            foreach (var record in CheckMetadataEquality(file))
            {
                if (record.Value == false)
                    isEqual = false;
            }
            return isEqual;
        }

        public Dictionary<string, bool> CheckMetadataEquality(MasterFile file)
        {
            return new Dictionary<string, bool>()
            {
                { "Artist", file.TagLibProps["Artist"].ToString() == _artist ? true : false },
                { "Album",  file.TagLibProps["Album"].ToString() == _album ? true : false },
                { "Title", file.TagLibProps["Title"].ToString() == _title ? true : false },
                { "Track",  Convert.ToInt32(file.TagLibProps["Track"]) == _track ? true : false },
                { "Year", file.TagLibProps["Year"].ToString() == _year ? true : false },
                { "Genres", file.TagLibProps["Genres"].ToString() == _genres ? true : false }
            };
        }

        public IEnumerator<string> GetEnumerator()
        {
            yield return _artist;
            yield return _album;
            yield return _title;
            yield return _track.ToString();
            yield return _year.ToString();
            yield return _genres;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
