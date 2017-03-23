using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMDB_Parser.Parsers
{
    class GenreParser : BaseParser
    {

        private string _regex = @"^(?'title'.*?)\s*\(((?'year'\d{4}(\/.*)?)|(.{4}(\/.*?)?))\)\s*(\{(?'episodename'.*?)(\(\#?(?'episode'[^\)]*)\))?\})?\s*(\((?'videomovie'V)\))?\s*(\((?'tvmovie'TV)\))?\s*(\((?'videogame'VG)\))?\s*(\{\{.*\}\})?\s*(?'genre'[^\t\r\n]*)$";

        public override string Name
        {
            get
            {
                return "Genres";
            }
        }

        protected override string FileBegin
        {
            get
            {
                return $"8: THE {Name.ToUpper()} LIST";
            }
        }

        protected override string Header
        {
            get
            {
                return "Title; Year; Serie; Episode; EpisodeName; VideoMovie; TVMovie; VideoGame; Genre;";
            }
        }

        protected override int SkipLinesCount
        {
            get
            {
                return 2;
            }
        }

        protected override void Parse()
        {
            string line, genre;
            while (!EndOfFile)
            {
                line = ReadLine();

                if (line == null)
                {
                    break;
                }

                genre = GetGenre(line);

                if (genre != null)
                {
                    WriteLine(genre);
                }
            }
        }

        private string GetGenre(string line)
        {
            Match match = RegexMatch(line, _regex);

            string genre = null;

            if (match.Success)
            {
                genre = $"{GetTitle(match).Trim('"')}; {GetYear(match)}; {IsSerie(match)}; {GetEpisode(match)}; {GetEpisodeName(match)}; {IsVideoMovie(match)}; {IsTVMovie(match)}; {IsVideoGame(match)}; {GetGenreDetails(match)};";
            }
            else
            {
                WriteToLog($"\"{line}\" not parsed");
            }

            return genre;
        }

        private string GetTitle(Match match)
        {
            return GetValueFromGroup(match, "title");
        }

        private string GetYear(Match match)
        {
            return GetValueFromGroup(match, "year");
        }

        private string GetEpisode(Match match)
        {
            return GetValueFromGroup(match, "episode");
        }

        private string GetEpisodeName(Match match)
        {
            return GetValueFromGroup(match, "episodename");
        }

        private string GetGenreDetails(Match match)
        {
            return GetValueFromGroup(match, "genre");
        }

        private string IsSerie(Match match)
        {
            string title = GetTitle(match);

            bool value = false;

            if (!string.IsNullOrEmpty(title))
            {
                value = title.Substring(0, 1) == "\"" && title.Substring(title.Length - 1) == "\"";
            }

            return value.ToString();
        }

        private string IsVideoMovie(Match match)
        {
            return (GetValueFromGroup(match, "videomovie") == "V").ToString();
        }

        private string IsTVMovie(Match match)
        {
            return (GetValueFromGroup(match, "tvmovie") == "TV").ToString();
        }

        private string IsVideoGame(Match match)
        {
            return (GetValueFromGroup(match, "videogame") == "VG").ToString();
        }
    }
}
