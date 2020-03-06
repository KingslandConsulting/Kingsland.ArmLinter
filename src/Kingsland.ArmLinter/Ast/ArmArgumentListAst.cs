using Kingsland.ArmLinter.Tokens;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Kingsland.ArmLinter.Ast
{

    public sealed class ArmArgumentListAst : ArmSyntaxNodeAst
    {

        /// <summary>
        /// argument_list = "(" [ argument *( "," argument ) ] ")"
        /// argument      = expression
        /// </summary>
        /// <param name="openParen"></param>
        /// <param name="arguments"></param>
        /// <param name="closeParen"></param>
        public ArmArgumentListAst(OpenParenToken openParen, IList<ArmExpressionAst> arguments, CloseParenToken closeParen)
        {
            this.OpenParen = openParen ?? throw new ArgumentNullException(nameof(openParen));
            this.ArgumentList = new ReadOnlyCollection<ArmExpressionAst>(
                arguments ?? throw new ArgumentNullException(nameof(arguments))
            );
            this.CloseParen = closeParen ?? throw new ArgumentNullException(nameof(closeParen));
        }

        public OpenParenToken OpenParen
        {
            get;
            private set;
        }

        public ReadOnlyCollection<ArmExpressionAst> ArgumentList
        {
            get;
            private set;
        }

        public CloseParenToken CloseParen
        {
            get;
            private set;
        }

        public override string ToArmText()
        {
            var result = new StringBuilder();
            result.Append('(');
            for (var i = 0; i < this.ArgumentList.Count; i++)
            {
                if (i > 0) { result.Append(", "); }
                result.Append(this.ArgumentList[i].ToArmText());
            }
            result.Append(')');
            return result.ToString();
        }

    }

}
