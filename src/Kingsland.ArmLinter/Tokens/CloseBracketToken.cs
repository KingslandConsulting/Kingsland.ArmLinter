using Kingsland.ParseFx.Syntax;
using Kingsland.ParseFx.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class CloseBracketToken : SyntaxToken
    {

        public CloseBracketToken()
            : this(SourceExtent.Empty)
        {
        }

        public CloseBracketToken(SourceExtent extent)
            : base(extent)
        {
        }

    }

}
