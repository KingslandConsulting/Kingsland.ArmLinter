using Kingsland.ParseFx.Lexing;
using Kingsland.ParseFx.Lexing.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class WhitespaceToken : Token
    {

        public WhitespaceToken(SourceExtent extent)
            : base(extent)
        {
        }

        public static bool AreEqual(WhitespaceToken obj1, WhitespaceToken obj2)
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
