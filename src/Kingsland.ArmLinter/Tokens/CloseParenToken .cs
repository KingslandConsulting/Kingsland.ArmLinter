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

        public static bool AreEqual(CloseParenToken obj1, CloseParenToken obj2)
        {
            if ((obj1 == null) && (obj2 == null))
            {
                return true;
            }
            else if ((obj1 == null) || (obj2 == null))
            {
                return false;
            }
            else
            {
                return obj1.Extent.IsEqualTo(obj2.Extent);
            }
        }

    }

}
