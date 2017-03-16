using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IMDB_Parser.Parsers
{
    public abstract class BaseParser : IParser
    {

        public abstract string Name { get; }

        protected abstract string Header { get; set; }

        protected bool EndOfFile
        {
            get
            {
                return _stream.EndOfStream;
            }
        }        

        protected virtual string FileBegin
        {
            get
            {
                return $"THE {Name.ToUpper()} LIST";
            }
        }

        private StringBuilder ParsedData { get; set; }

        private StreamReader _stream;

        private string _newFilePath;

        private string LogFile
        {
            get
            {
                return _newFilePath.Substring(0, _newFilePath.Length - 4) + "_log.txt";
            }
        }

        public bool Parse(string parseableFilePath, string newFilePath)
        {
            _newFilePath = newFilePath;
            ParsedData = new StringBuilder();

            using (_stream = new StreamReader(parseableFilePath, Parsers.ISO88591))
            {
                string line = string.Empty;

                do
                {
                    line = ReadLine();
                }
                while (!_stream.EndOfStream && line != FileBegin);

                if(_stream.EndOfStream)
                {
                    throw new Exception("File ended before something was found, maybe you loaded the wrong file!");
                }

                SkipLines(4);

                ClearWriteFile();

                WriteLine(Header);

                Parse();

                WriteFile();
            }

            CleanUp();

            return true;
        }

        private void CleanUp()
        {
            ParsedData = null;
            _stream = null;
            _newFilePath = null;
            _lineCount = 0;
        }

        private void WriteFile()
        {
            using (var write = new StreamWriter(_newFilePath, true, Encoding.UTF8))
            {
                write.Write(ParsedData.ToString());
            }

            ParsedData.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void ClearWriteFile()
        {
            if(File.Exists(_newFilePath))
            {
                File.Create(_newFilePath).Close();
            }
        }

        protected void WriteToLog(string logMessage)
        {
            using (var writer = new StreamWriter(LogFile, true))
            {
                writer.WriteLine($"({DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}) [Line {_lineCount}] : {logMessage}" );
            }
        }

        private int _lineCount;
        protected string ReadLine()
        {
            string line = null;

            if(!EndOfFile)
            {
                _lineCount++;
                line = Parsers.ConvertToUTF8(_stream.ReadLine());

                if(line.Contains("----------------------"))
                {
                    line = null;
                }
            }
            return line;
        }

        private void SkipLines(int lineCount)
        {
            _lineCount += lineCount;
            for (int i = 0; i < lineCount; i++)
            {
                _stream.ReadLine();
            }
        }

        protected void WriteLine(string line)
        {

            ParsedData.AppendLine(line);

            if(_lineCount % 400000 == 0)
            {
                WriteFile();
            }
        }

        public override string ToString()
        {
            return Name;
        }

        protected abstract void Parse();

        protected string GetValueFromGroup(Match match, string groupName)
        {
            string value = string.Empty;

            Group group = match.Groups[groupName];
            if (group != null && group.Success)
            {
                value = group.Value;
            }

            return value.Trim(' ','\t');
        }

        protected Match RegexMatch(string line, string regex)
        {
            Match match = Regex.Match(line, regex);

            if(!match.Success)
            {
                //try to repair the line by removing parts with wrong braces and check again
                line = TryRepairLine(line);
                match = Regex.Match(line, regex);
            }

            return match;
        }

        protected string TryRepairLine(string line)
        {
            string[] lineParts = line.Split(' ', '\t');

            for(int i = 0; i < lineParts.Length; i++)
            {
                if(!ValidLinePart(lineParts[i]))
                {
                    lineParts[i] = string.Empty;
                }
            }
            return string.Join(" ", lineParts);
        }

        private bool ValidLinePart(string linePart)
        {
            int countParenthesis = 0;
            int countBracket = 0;
            int countBraces = 0;
            int countChevrons = 0;

            foreach (char c in linePart)
            {
                switch(c)
                {
                    case '(':
                        countParenthesis++;
                        break;
                    case ')':
                        countParenthesis--;
                        break;
                    case '[':
                        countBracket++;
                        break;
                    case ']':
                        countBracket--;
                        break;
                    case '{':
                        countBraces++;
                        break;
                    case '}':
                        countBraces--;
                        break;
                    case '<':
                        countChevrons++;
                        break;
                    case '>':
                        countChevrons--;
                        break;
                }
            }

            return countParenthesis == 0 && countBracket == 0 && countBraces == 0 && countChevrons == 0;
        }
    }
}
