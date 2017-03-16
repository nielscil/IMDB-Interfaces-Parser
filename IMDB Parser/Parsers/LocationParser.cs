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

        //private const string _regex = @"^(?'title'.*?)\s*\(((?'year'\d{4}(\/.*)?)|(.{4}(\/.*?)?))\)\s*(?'episode'\{[^\{\}]*\})?\s*((?'location'.*)??,[ ]*)?((?'city'[^,]*)?,[ ]*)?((?'state'[^,]*)?,[ ]*)?(?'country'[^,\(]*)?(?'extra'\(.*\))?$";
        private const string _regex = @"^(?'title'.*?)\s*\(((?'year'\d{4}(\/.*)?)|(.{4}(\/.*?)?))\)\s*(?'episode'\{[^\{\}]*\})?\s*(\{\{.*\}\})?\s*(?'locationdetails'[^\t]*)\s*(\((?'extra'.*)\))?$";
        private string[] _seperator { get; } = new string[] { ", " };

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
                return "Title; Year; Serie; Episode; Location; City; State; Country; Extra;";
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
                    location = $"{GetTitle(match)}; {GetYear(match)}; {IsSerie(match)}; {GetEpisode(match)}; {locationDetails}; {GetExtra(match)};";
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
            string location = GetValueFromGroup(match, "locationdetails");
            string details = null;

            if(!string.IsNullOrEmpty(location))
            {
                string[] data = location.Split(_seperator, StringSplitOptions.None);

                string loc = string.Empty;
                string city = string.Empty;
                string state = string.Empty;
                string country = string.Empty;

                country = data[data.Length - 1];
                
                if(data.Length - 2 >= 0)
                {
                    state = data[data.Length - 2];
                }
                
                if(data.Length - 3 >= 0)
                {
                    city = data[data.Length - 3];
                }

                int index = data.Length - 4;

                for(int i = 0; i <= index; i++)
                {
                    loc += data[i];
                }

                details = $"{location}; {loc}; {city}; {state}; {country}";
            }

            return details;
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

        private string GetLocation(Match match)
        {
            return GetValueFromGroup(match, "location");
        }

        private string GetCity(Match match)
        {
            return GetValueFromGroup(match, "city");
        }

        private string GetState(Match match)
        {
            return GetValueFromGroup(match, "state");
        }

        private string GetCountry(Match match)
        {
            return GetValueFromGroup(match, "country");
        }

        private string GetExtra(Match match)
        {
            return GetValueFromGroup(match, "extra");
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

    }
}
