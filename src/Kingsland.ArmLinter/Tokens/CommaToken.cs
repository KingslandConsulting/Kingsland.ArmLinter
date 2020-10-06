using Kingsland.ParseFx.Syntax;
using Kingsland.ParseFx.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class CommaToken : SyntaxToken
    {

        public CommaToken()
            : this(SourceExtent.Empty)
        {
        }

        public CommaToken(SourceExtent extent)
            : base(extent)
        {
        }

    }

}
