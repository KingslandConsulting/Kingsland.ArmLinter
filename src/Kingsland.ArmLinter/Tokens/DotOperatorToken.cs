using Kingsland.ParseFx.Syntax;
using Kingsland.ParseFx.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class DotOperatorToken : SyntaxToken
    {

        public DotOperatorToken()
            : this(SourceExtent.Empty)
        {
        }

        public DotOperatorToken(SourceExtent extent)
            : base(extent)
        {
        }

    }

}
