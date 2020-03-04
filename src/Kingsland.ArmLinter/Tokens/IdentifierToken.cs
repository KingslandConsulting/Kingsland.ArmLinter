using Kingsland.ParseFx.Lexing;
using Kingsland.ParseFx.Lexing.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class IdentifierToken : Token
    {

        public IdentifierToken(SourceExtent extent, string name)
            : base(extent)
        {
            this.Name = name;
        }

        public string Name
        {
            get;
            private set;
        }

        public static bool AreEqual(IdentifierToken obj1, IdentifierToken obj2)
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
                return obj1.Extent.IsEqualTo(obj2.Extent) &&
                       (obj1.Name == obj2.Name);
            }
        }

    }

}
