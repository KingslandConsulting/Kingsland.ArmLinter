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

        public static bool AreEqual(CommaToken obj1, CommaToken obj2)
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
