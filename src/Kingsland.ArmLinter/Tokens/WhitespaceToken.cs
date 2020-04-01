using Kingsland.ParseFx.Syntax;
using Kingsland.ParseFx.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class WhitespaceToken : SyntaxToken
    {

        public WhitespaceToken(string value)
            : this(SourceExtent.Empty, value)
        {
        }

        public WhitespaceToken(SourceExtent extent, string value)
            : base(extent)
        {
            this.Value = value;
        }

        public string Value
        {
            get;
            private set;
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
