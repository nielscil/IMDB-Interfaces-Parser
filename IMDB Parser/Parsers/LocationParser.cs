using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMDB_Parser.Parsers
{
    class LocationParser : BaseParser
    {

       private const string _regex = @"^(?'title'.*)\s*\(((?'year'\d{4}(\/.*?)?)|(.{4}(\/.*?)?))\)\s*(\{(?'episodename'.*?)(\(\#?(?'episode'[^\)]{0,15})\))?\})?\s*(\((?'videomovie'V)\))?\s*(\((?'tvmovie'TV)\))?\s*(\((?'videogame'VG)\))?\s*(\{\{.*\}\})?\s*(?'locationdetails'[^\t\r\n]*)\s*(\((?'extra'.*)\))?$";

        public override string Name
        {
            get
            {
                return "Locations";
            }
        }

        protected override string Header
        {
            get
            {
                return "Title;Year;Serie;Episode;EpisodeName;VideoMovie;TVMovie;VideoGame;Location;Extra";
            }
        }

        protected override int SkipLinesCount
        {
            get
            {
                return 1;
            }
        }

        protected override string FileBegin
        {
            get
            {
                return $"{Name.ToUpper()} LIST";
            }
        }

        protected override void Parse()
        {
            string line, location;
            while (!EndOfFile)
            {
                line = ReadLine();

                if (line == null)
                {
                    break;
                }

                location = GetLocation(line);

                if (location != null)
                {
                    WriteLine(location);
                }
            }
        }

        private string GetLocation(string line)
        {
            Match match = RegexMatch(line, _regex);

            string location = null;

            if (match.Success)
            {
                string locationDetails = GetLocationDetails(match);

                if(!string.IsNullOrEmpty(locationDetails))
                {
                    location = $"\"{GetTitle(match)}\";{GetYear(match)};{IsSerie(match)};\"{GetEpisode(match)}\";\"{GetEpisodeName(match)}\";{IsVideoMovie(match)};{IsTVMovie(match)};{IsVideoGame(match)};\"{locationDetails}\";\"{GetExtra(match)}\"";
                }
                else
                {
                    WriteToLog($"\"{line}\" has no location details");
                }
            }
            else
            {
                WriteToLog($"\"{line}\" not parsed");
            }

            return location;
        }

        private string GetLocationDetails(Match match)
        {
            return GetValueFromGroup(match, "locationdetails").Replace("\"","\\\"");
        }

        private string GetTitle(Match match)
        {
            return GetValueFromGroup(match, "title").Trim('"');
        }

        private string GetYear(Match match)
        {
            return GetValueFromGroup(match, "year");
        }

        private string GetEpisode(Match match)
        {
            return GetValueFromGroup(match, "episode").Replace("\"", "\\\"");
        }

        private string GetEpisodeName(Match match)
        {
            return GetValueFromGroup(match, "episodename").Replace("\"", "\\\"");
        }

        private string GetExtra(Match match)
        {
            return GetValueFromGroup(match, "extra").Replace("\"", "\\\"");
        }

        private string IsSerie(Match match)
        {
            string title = GetTitle(match);

            bool value = false;

            if (!string.IsNullOrEmpty(title))
            {
                value = title.Substring(0, 1) == "\"" && title.Substring(title.Length - 1) == "\"";
            }

            return Convert.ToInt32(value).ToString();//value.ToString();
        }

        private string IsVideoMovie(Match match)
        {
            return Convert.ToInt32(GetValueFromGroup(match, "videomovie") == "V").ToString();
        }

        private string IsTVMovie(Match match)
        {
            return Convert.ToInt32(GetValueFromGroup(match, "tvmovie") == "TV").ToString();
        }

        private string IsVideoGame(Match match)
        {
            return Convert.ToInt32(GetValueFromGroup(match, "videogame") == "VG").ToString();
        }

    }
}
