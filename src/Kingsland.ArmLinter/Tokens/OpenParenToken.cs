﻿using Kingsland.Lexing;
using Kingsland.Lexing.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class OpenParenToken : Token
    {

        public OpenParenToken(SourceExtent extent)
            : base(extent)
        {
        }

        public static bool AreEqual(OpenParenToken obj1, OpenParenToken obj2)
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