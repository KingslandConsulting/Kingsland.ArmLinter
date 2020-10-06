using Kingsland.ParseFx.Syntax;
using Kingsland.ParseFx.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class CloseParenToken : SyntaxToken
    {

        public CloseParenToken()
            : this(SourceExtent.Empty)
        {
        }

        public CloseParenToken(SourceExtent extent)
            : base(extent)
        {
        }

    }

}
