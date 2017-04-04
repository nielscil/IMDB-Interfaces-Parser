using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMDB_Parser.Parsers
{
    class PlotParser : BaseParser
    {
        private string _movieRegex = @"^MV:\ (?'title'.*)\s*\(((?'year'\d{4}(\/.*?)?)|(.{4}(\/.*?)?))\)\s*(\{(?'episodename'.*?)(\(\#?(?'episode'[^\)]{0,15})\))?\})?\s*(\((?'videomovie'V)\))?\s*(\((?'tvmovie'TV)\))?\s*(\((?'videogame'VG)\))?\s*(\{\{.*\}\})?$";
        private string _plotRegex = @"^PL: (?'plot'.*)$";

        public override string Name
        {
            get
            {
                return "Plots";
            }
        }

        protected override string FileBegin
        {
            get
            {
                return "PLOT SUMMARIES LIST";
            }
        }

        protected override string Header
        {
            get
            {
                return "Title;Year;Serie;Episode;EpisodeName;VideoMovie;TVMovie;VideoGame;Plot";
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
            string line, plot;
            while (!EndOfFile)
            {
                line = ReadLine();

                if (string.IsNullOrEmpty(line) || !IsMovieTitle(line))
                {
                    continue;
                }

                plot = ParsePlot(line);

                if (plot != null)
                {
                    WriteLine(plot);
                }
            }
        }

        private bool IsMovieTitle(string line)
        {
            return line.Substring(0, 3) == "MV:";
        }

        private string ParsePlot(string line)
        {
            string plot = null;

            string movieTitle = GetMovieTitle(line);

            SkipLines(1);

            if(movieTitle != null)
            {
                plot = string.Empty;
                string plotLine = null;

                while(!EndOfFile)
                {
                    string readLine = ReadLine();

                    if (string.IsNullOrEmpty(readLine))
                    {
                        break;
                    }

                    plotLine = GetPlotLine(readLine);

                    if(!string.IsNullOrEmpty(plotLine))
                    {
                        plot += plotLine;
                    }
                }

                plot = $"{movieTitle}\"{plot}\"";
            }

            return plot;
        }

        private string GetPlotLine(string line)
        {
            Match match = RegexMatch(line, _plotRegex);

            string plotLine = null;

            if (match.Success)
            {
                plotLine = $"{GetPlotDetails(match)} ";
            }
            else
            {
                WriteToLog($"\"{line}\" not parsed");
            }

            return plotLine;
        }

        private string GetMovieTitle(string line)
        {
            Match match = RegexMatch(line, _movieRegex);

            string movieTitle = null;

            if (match.Success)
            {
                movieTitle = $"\"{GetTitle(match).Trim('"')}\";{GetYear(match)};{IsSerie(match)};\"{GetEpisode(match)}\";\"{GetEpisodeName(match)}\";{IsVideoMovie(match)};{IsTVMovie(match)};{IsVideoGame(match)};";
            }
            else
            {
                WriteToLog($"\"{line}\" not parsed");
            }

            return movieTitle;
        }

        private string GetPlotDetails(Match match)
        {
            return GetValueFromGroup(match, "plot").Replace("\"", "\\\"").Replace(";","\\;");
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
    }
}
