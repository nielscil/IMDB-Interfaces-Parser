using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMDB_Parser.Parsers
{
    public class MoviesParser : BaseParser
    {

        private const string _regex = @"^(?'title'.*)\s*\(((?'year'\d{4}(\/.*?)?)|(.{4}(\/.*?)?))\)\s*(\{(?'episodename'.*?)(\(\#?(?'episode'[^\)]{0,15})\))?\})?\s*(\((?'videomovie'V)\))?\s*(\((?'tvmovie'TV)\))?\s*(\((?'videogame'VG)\))?\s*(\{{2}.*\}{2})?\s*(?'extra'\(.*\)[.]?)?\s*((?'period'\d{4}|.{4})(-(?'endperiod'\d{4}|.{4}))?)$";

        public override string Name
        {
            get
            {
                return "Movies";
            }
        }

        protected override string Header
        {
            get
            {
                return "Title;Year;PeriodBegin;PeriodEnd;Serie;Episode;EpisodeName;VideoMovie;TVMovie;VideoGame;Extra";
            }
        }

        protected override int SkipLinesCount
        {
            get
            {
                return 2;
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
            string line, movie;
            while (!EndOfFile)
            {
                line = ReadLine();

                if (line == null)
                {
                    break;
                }

                movie = GetMovie(line);

                if (movie != null)
                {
                    WriteLine(movie);
                }
            }
        }

        private string GetMovie(string line)
        {
            Match match = RegexMatch(line, _regex);

            string movie = null;

            if (match.Success)
            {

                movie = $"\"{GetTitle(match).Trim('"')}\";{GetYear(match)};{GetPeriodBegin(match)};{GetPeriodEnd(match)};{IsSerie(match)};\"{GetEpisode(match)}\";\"{GetEpisodeName(match)}\";{IsVideoMovie(match)};{IsTVMovie(match)};{IsVideoGame(match)};\"{GetExtra(match)}\"";

            }
            else
            {
                WriteToLog($"\"{line}\" not parsed");
            }

            return movie;
        }

        private string GetTitle(Match match)
        {
            return GetValueFromGroup(match, "title");
        }

        private string GetYear(Match match)
        {
            return GetValueFromGroup(match, "year"); ;
        }

        private string GetPeriodBegin(Match match)
        {
            return GetValueFromGroup(match, "period"); ;
        }

        private string GetPeriodEnd(Match match)
        {
            return GetValueFromGroup(match, "endperiod"); ;
        }

        private string GetEpisode(Match match)
        {
            return GetValueFromGroup(match, "episode").Replace("\"", "\\\"");
        }

        private string GetEpisodeName(Match match)
        {
            return GetValueFromGroup(match, "episodename").Replace("\"", "\\\"");
        }

        private string IsSerie(Match match)
        {
            string title = GetTitle(match);

            bool value = false;

            if (!string.IsNullOrEmpty(title))
            {
                value = title.Substring(0, 1) == "\"" && title.Substring(title.Length - 1) == "\"";
            }

            return Convert.ToInt32(value).ToString();
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

        private string GetExtra(Match match)
        {
            return GetValueFromGroup(match, "extra");
        }
    }
}
