using Kingsland.ParseFx.Lexing;
using Kingsland.ParseFx.Lexing.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class CloseParenToken : Token
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
