using Kingsland.ParseFx.Syntax;
using Kingsland.ParseFx.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class OpenBracketToken : SyntaxToken
    {

        public OpenBracketToken()
            : this(SourceExtent.Empty)
        {
        }

        public OpenBracketToken(SourceExtent extent)
            : base(extent)
        {
        }

    }

}
