using Kingsland.ParseFx.Syntax;
using Kingsland.ParseFx.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class WhitespaceToken : SyntaxToken
    {

        public WhitespaceToken(string value)
            : this(SourceExtent.Empty, value)
        {
        }

        public WhitespaceToken(SourceExtent extent, string value)
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
