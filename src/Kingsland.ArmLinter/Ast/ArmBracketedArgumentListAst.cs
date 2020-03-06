using Kingsland.ArmLinter.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Kingsland.ArmLinter.Ast
{

    public sealed class ArmBracketedArgumentListAst : ArmSyntaxNodeAst
    {

        /// <summary>
        /// argument_list = "[" [ argument *( "," argument ) ] "]"
        /// argument      = expression
        /// </summary>
        /// <param name="openBracket"></param>
        /// <param name="arguments"></param>
        /// <param name="closeBracket"></param>
        public ArmBracketedArgumentListAst(OpenBracketToken openBracket, IList<ArmExpressionAst> arguments, CloseBracketToken closeBracket)
        {
            this.OpenBracket = openBracket ?? throw new ArgumentNullException(nameof(openBracket));
            this.ArgumentList = new ReadOnlyCollection<ArmExpressionAst>(
                arguments ?? throw new ArgumentNullException(nameof(arguments))
            );
            this.CloseBracket = closeBracket ?? throw new ArgumentNullException(nameof(closeBracket));
        }

        public OpenBracketToken OpenBracket
        {
            get;
            private set;
        }

        public ReadOnlyCollection<ArmExpressionAst> ArgumentList
        {
            get;
            private set;
        }

        public CloseBracketToken CloseBracket
        {
            get;
            private set;
        }

        public override string ToArmText()
        {
            var result = new StringBuilder();
            result.Append('[');
            for (var i = 0; i < this.ArgumentList.Count; i++)
            {
                if (i > 0) { result.Append(", "); }
                result.Append(this.ArgumentList[i].ToArmText());
            }
            result.Append(']');
            return result.ToString();
        }

    }

}
