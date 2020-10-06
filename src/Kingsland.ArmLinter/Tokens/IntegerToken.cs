using Kingsland.ParseFx.Syntax;
using Kingsland.ParseFx.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class IntegerToken : SyntaxToken
    {

        public IntegerToken(long value)
            : this(SourceExtent.Empty, value)
        {
        }

        public IntegerToken(SourceExtent extent, long value)
            : base(extent)
        {
            this.Value = value;
        }

        public long Value
        {
            get;
            private set;
        }

    }

}
