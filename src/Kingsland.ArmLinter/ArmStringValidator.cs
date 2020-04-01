using System.Linq;

namespace Kingsland.ArmLinter
{

    public static class ArmStringValidator
    {

        public static bool IsArmExpression(string value)
        {
            return !ArmStringValidator.IsLiteralString(value);
        }

        public static bool IsLiteralString(string value)
        {
            // from https://docs.microsoft.com/en-us/azure/azure-resource-manager/template-expressions#escape-characters
            //
            // Expressions
            //
            // To have a literal string start with a left bracket [ and end with a right bracket ], but not have it
            // interpreted as an expression, add an extra bracket to start the string with [[. For example, the variable:
            //
            //     "demoVar1": "[[test value]"
            //
            // Resolves to [test value].
            //
            // However, if the literal string doesn't end with a bracket, don't escape the first bracket. For example,
            // the variable:
            //
            //     "demoVar2": "[test] value"
            //
            // Resolves to [test] value.
            //
            if (string.IsNullOrEmpty(value))
            {
                // "demoVar2": ""
                return true;
            }
            if (!value.StartsWith("[") || !value.EndsWith("]"))
            {
                // "demoVar2": "test value"
                // "demoVar2": "[test] value"
                // "demoVar2": "test [value]"
                return true;
            }
            if (value.StartsWith("[["))
            {
                // "demoVar1": "[[test value]"
                return true;
            }
            // "demoVar2": "[test value]"
            return false;
        }

        public static bool IsUpperAlpha(char @char)
        {
            return (@char >= '\u0041') && (@char <= '\u005A');
        }
        public static bool IsLowerAlpha(char @char)
        {
            return (@char >= '\u0061') && (@char <= '\u007A');
        }

        public static bool IsFirstIdentifierChar(char value)
        {
            return (value == '_') ||
                   ArmStringValidator.IsUpperAlpha(value) ||
                   ArmStringValidator.IsLowerAlpha(value);
        }

        public static bool IsNextIdentifierChar(char value)
        {
            return ArmStringValidator.IsFirstIdentifierChar(value) ||
                   ArmStringValidator.IsDigit(value);
        }

        public static bool IsDigit(char value)
        {
            return (value >= '0') && (value <= '9');
        }

        private static readonly char[] whitespaceChars = new[] {
            '\u0020', '\u000D', '\u000A'
        };

        public static bool IsWhitespace(char @char)
        {
            return ArmStringValidator.whitespaceChars.Contains(@char);
        }

    }

}
