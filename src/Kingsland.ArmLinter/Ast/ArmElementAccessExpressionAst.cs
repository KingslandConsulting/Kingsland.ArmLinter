using System;
using System.Text;

namespace Kingsland.ArmLinter.Ast
{

    public sealed class ArmElementAccessExpressionAst : ArmExpressionAst
    {

        /// <summary>
        /// invocation    = expression argument_list
        /// argument_list = "(" [ argument *( "," argument ) ] ")"
        /// argument      = expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="argumentList"></param>
        public ArmElementAccessExpressionAst(ArmExpressionAst expression, ArmBracketedArgumentListAst argumentList)
        {
            this.Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            this.ArgumentList = argumentList ?? throw new ArgumentNullException(nameof(argumentList));
        }

        public ArmExpressionAst Expression
        {
            get;
            private set;
        }

        public ArmBracketedArgumentListAst ArgumentList
        {
            get;
            private set;
        }

        public override string ToArmText()
        {
            var result = new StringBuilder();
            result.Append(this.Expression.ToArmText());
            result.Append(this.ArgumentList.ToArmText());
            return result.ToString();
        }

    }

}
