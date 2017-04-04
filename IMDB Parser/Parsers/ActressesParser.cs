using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMDB_Parser.Parsers
{
    public class ActressesParser : BaseParser
    {

        private const string _regexActress = @"^(?'nickname'\'.*\'[ ]?)?((?'surname'[^,\n]*), )?(?'name'.*?)[\t]{1,2}(?'title'.*)$";
        private const string _regexTitle = @"^(?'title'.*)\s*\(((?'year'\d{4}(\/.*?)?)|(.{4}(\/.*?)?))\)\s*(\{(?'episodename'.*?)(\(\#?(?'episode'[^\)]{0,15})\))?\})?\s*(\((?'videomovie'V)\))?\s*(\((?'tvmovie'TV)\))?\s*(\((?'videogame'VG)\))?\s*(?'character1'[^{}\[\]\(\)\<\>]*)\s*(\{{2}.*\}{2})?\s*(?'extra'\(.*\)[.]?)?\s*(\[(?'character'.*)\])?\s*(\<(?'billingposition'.*)\>)?$";
        private char[] _seperator { get; } = { '\t' };

        public override string Name
        {
            get
            {
                return "Actresses";
            }
        }

        protected override string Header
        {
            get
            {
                return "Name;Surname;Nickname;Role;Role Extras;BillingPosition;Title;Year;Serie;Episode;EpisodeName;VideoMovie;TVMovie;VideoGame;";
            }
        }

        protected override int SkipLinesCount
        {
            get
            {
                return 4;
            }
        }

        protected override string FileBegin
        {
            get
            {
                return $"THE {Name.ToUpper()} LIST";
            }
        }

        protected override void Parse()
        {
            string actor = string.Empty;
            string line = string.Empty;
            string title = string.Empty;

            while (!EndOfFile)
            {
                line = ReadLine();

                if (line == null)
                {
                    break;
                }

                if (line == string.Empty)
                {
                    actor = string.Empty;
                    continue;
                }

                if (actor == string.Empty)
                {
                    actor = GetActress(line);
                }

                title = GetTitle(line);

                if (actor != null && title != null)
                {
                    WriteLine(actor + title);
                }
            }
        }

        private string GetActress(string line)
        {
            Match match = RegexMatch(line, _regexActress);

            string actor = null;

            if (match.Success)
            {
                actor = $"\"{GetActorName(match)}\";\"{GetActorSurname(match)}\";\"{GetActorNickname(match)}\";";         
            }
            else
            {
                WriteToLog($"\"{line}\" not parsed");
            }

            return actor;
        }

        private string GetTitle(string line)
        {

            line = GetRightTitlePart(line);

            Match match = RegexMatch(line, _regexTitle);

            string title = null;

            if (match.Success)
            {
                title = $"\"{GetTitleCharacter(match)}\";\"{GetTitleCharacterExtras(match)}\";{GetTitleBillingPosition(match)};\"{GetTitleTitle(match).Trim('"')}\";{GetTitleYear(match)};{IsSerie(match)};\"{GetTitleEpisode(match)}\";\"{GetTitleEpisodeName(match)}\";{IsVideoMovie(match)};{IsTVMovie(match)};{IsVideoMovie(match)}";   
            }
            else
            {
                WriteToLog($"\"{line}\" not parsed");
            }

            return title;
        }

        private string GetRightTitlePart(string line)
        {
            string[] splitted = line.Split(_seperator, StringSplitOptions.RemoveEmptyEntries);

            if (splitted.Length == 2)
            {
                return splitted[1];
            }
            else if (splitted.Length == 1)
            {
                return splitted[0];
            }
            return line;
        }

        private string GetActorNickname(Match match)
        {
            return GetValueFromGroup(match, "nickname").Replace("\"", "\\\"");
        }

        private string GetActorSurname(Match match)
        {
            return GetValueFromGroup(match, "surname").Replace("\"", "\\\"");
        }

        private string GetActorName(Match match)
        {
            return GetValueFromGroup(match, "name").Replace("\"", "\\\"");
        }

        private string GetTitleTitle(Match match)
        {
            return GetValueFromGroup(match, "title");
        }

        private string GetTitleYear(Match match)
        {
            string year = GetValueFromGroup(match, "year");

            if(year == "????")
            {
                year = string.Empty;
            }

            return year;
        }

        private string GetTitleCharacter(Match match)
        {
            string character = GetValueFromGroup(match, "character").Replace("\"", "\\\"");

            if (character == string.Empty)
            {
                character = GetValueFromGroup(match, "character1").Replace("\"", "\\\"");
            }

            return character;
        }

        private string GetTitleCharacterExtras(Match match)
        {
            return GetValueFromGroup(match, "extra").Replace("\"", "\\\"");
        }

        private string GetTitleBillingPosition(Match match)
        {
            return GetValueFromGroup(match, "billingposition");
        }

        private string GetTitleEpisode(Match match)
        {
            return GetValueFromGroup(match, "episode").Replace("\"", "\\\"");
        }

        private string GetTitleEpisodeName(Match match)
        {
            return GetValueFromGroup(match, "episodename").Replace("\"", "\\\"");
        }

        private string IsSerie(Match match)
        {
            string title = GetTitleTitle(match);

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
