using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Appli.Utils.ConstanteApp;

namespace Appli.Utils
{
    public static class UpperLetters
    {
        public static string UpperFirstLetter(string word)
        {
            var i = 1;
            var builder = new StringBuilder();
            foreach (var ch in word)
            {
                builder.Append(i == 1 ? char.ToUpper(ch) : ch);
                i++;
            }

            return builder.ToString();
        }

            public static IEnumerable<string> UpperFirstAfterSpace(string mode, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return new []{value};

            if (!value.Contains(" ")) return new []{UpperFirstLetter(value)};

            var builder = new StringBuilder();
            var separate = value.Split(" ");

            if (separate.Length <= 1)
                return new[] {UpperFirstLetter(separate[0])};

            switch (mode)
            {
                case OEUVRE_PARAM :
                {

                    foreach (var val in separate)
                    {
                        if (string.IsNullOrWhiteSpace(val)) continue;

                        builder.Append(separate.ToList().IndexOf(val) == 0 
                            ? UpperFirstLetter(val)
                            : $" {UpperFirstLetter(val)}");
                    }

                    return new[] {builder.ToString()};
                }

                case PERSONNE_PARAM :
                {
                    var values = new List<string> {UpperFirstLetter(separate[0])};

                    foreach (var val in separate[1].Split(" "))
                        builder.Append($" {UpperFirstLetter(val)}");

                    values.Add(builder.ToString());
                    return values;
                }

                default: return new[] {value};
            }
        }
    }
}
