using Kingsland.ParseFx.Lexing;
using Kingsland.ParseFx.Lexing.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class CloseBracketToken : Token
    {

        public CloseBracketToken(SourceExtent extent)
            : base(extent)
        {
        }

        public static bool AreEqual(CloseBracketToken obj1, CloseBracketToken obj2)
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
