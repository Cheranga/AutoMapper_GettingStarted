using System.Text.RegularExpressions;
using AutoMapper;

namespace Automapper.DataAccess.UnitTests.Mappers
{
    public class RemoveUnderscoreConvention : INamingConvention
    {
        public string ReplaceValue(Match match)
        {
            return match.Value;
        }

        public Regex SplittingExpression
        {
            get
            {
                return new Regex("^.*?_(.*?)_.*$");
            }
        }

        public string SeparatorCharacter
        {
            get { return "_"; }
        }
    }
}