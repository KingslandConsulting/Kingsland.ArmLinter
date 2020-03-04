using Kingsland.ParseFx.Lexing;
using Kingsland.ParseFx.Lexing.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class IntegerToken : Token
    {

        public IntegerToken(SourceExtent extent, long value)
            : base(extent)
        {
            this.Value = value;
        }

        public long Value
        {
            get;
            private set;
        }

        public static bool AreEqual(IntegerToken obj1, IntegerToken obj2)
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
                       (obj1.Value == obj2.Value);
            }
        }

    }

}
