using Kingsland.ParseFx.Syntax;
using Kingsland.ParseFx.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class StringLiteralToken : SyntaxToken
    {

        public StringLiteralToken(string value)
            : this(SourceExtent.Empty, value)
        {
        }

        public StringLiteralToken(SourceExtent extent, string value)
            : base(extent)
        {
            this.Value = value;
        }

        public string Value
        {
            get;
            private set;
        }

    }

}
