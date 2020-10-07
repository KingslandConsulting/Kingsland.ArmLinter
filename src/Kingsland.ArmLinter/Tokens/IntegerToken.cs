using Kingsland.ParseFx.Syntax;
using Kingsland.ParseFx.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class IntegerToken : SyntaxToken
    {

        public IntegerToken(int value)
            : this(SourceExtent.Empty, value)
        {
        }

        public IntegerToken(SourceExtent extent, int value)
            : base(extent)
        {
            this.Value = value;
        }

        public int Value
        {
            get;
            private set;
        }

    }

}
