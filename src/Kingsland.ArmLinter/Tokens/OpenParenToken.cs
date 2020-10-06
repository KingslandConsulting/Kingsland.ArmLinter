using Kingsland.ParseFx.Syntax;
using Kingsland.ParseFx.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class OpenParenToken : SyntaxToken
    {

        public OpenParenToken()
            : this(SourceExtent.Empty)
        {
        }

        public OpenParenToken(SourceExtent extent)
            : base(extent)
        {
        }

    }

}
