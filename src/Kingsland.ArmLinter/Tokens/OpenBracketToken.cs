﻿using Kingsland.ParseFx.Syntax;
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

        public static bool AreEqual(OpenBracketToken obj1, OpenBracketToken obj2)
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
