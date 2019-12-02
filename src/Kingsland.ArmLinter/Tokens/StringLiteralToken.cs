﻿using Kingsland.Lexing;
using Kingsland.Lexing.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class StringLiteralToken : Token
    {

        public StringLiteralToken(SourceExtent extent, string value)
            : base(extent)
        {
            this.Value = value;
        }

        public string Value
        {
            get;
            private set;
        }

        public static bool AreEqual(StringLiteralToken obj1, StringLiteralToken obj2)
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