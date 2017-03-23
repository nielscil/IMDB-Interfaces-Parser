using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_Parser.Parsers
{
    public static class Parsers
    {

        public static Encoding ISO88591
        {
            get
            {
                return Encoding.GetEncoding("iso-8859-1");
            }
        }

        public static IEnumerable<IParser> GetParser()
        {
            yield return new ActorsParser();
            yield return new ActressesParser();
            yield return new MoviesParser();
            yield return new LocationParser();
            yield return new GenreParser();
            yield return new KeywordsParser();
            yield return new PlotParser();
        }

        public static string ConvertToUTF8(string old)
        {
            byte[] bytes = ISO88591.GetBytes(old);
            bytes = Encoding.Convert(ISO88591, Encoding.UTF8, bytes);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
