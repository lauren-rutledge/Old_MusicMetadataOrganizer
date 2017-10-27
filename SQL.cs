using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicMetadataOrganizer
{
    public class SQL
    {
        public bool IsValidInput(string sqlInput)
        {
            var isValidInput = true;
            var textPattern = "(''|[^'])*";
            var semiColonPattern = ";";
            var sqlStatementPattern = "\b(ALTER|CREATE|DELETE|DROP|EXEC(UTE){0,1}" +
                "|INSERT( +INTO){0,1}|MERGE|SELECT|UPDATE|UNION( +ALL){0,1})\b";
            Regex textBlocks = new Regex(textPattern);
            Regex statementBreaks = new Regex(semiColonPattern);
            Regex sqlStatements = new Regex(sqlStatementPattern, RegexOptions.IgnoreCase);

            // Fix this so it's not the only one opposite
            // This is separated so that when error handling is added, specific exceptions can be thrown
            if (!textBlocks.IsMatch(sqlInput))
                isValidInput = false;
            if (statementBreaks.IsMatch(sqlInput))
                isValidInput = false;
            if (sqlStatements.IsMatch(sqlInput))
                isValidInput = false;
            return isValidInput;
        }
    }
}
