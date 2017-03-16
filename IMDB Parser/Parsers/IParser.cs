using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_Parser.Parsers
{
    public interface IParser
    {

        string Name { get; }

        bool Parse(string parseableFilePath, string newFilePath);
    }
}
