using System;

namespace Kingsland.ArmLinter.Ast
{

    public sealed class ArmElementExpressionAst : ArmExpressionAst
    {

        /// <summary>
        /// invocation    = expression argument_list
        /// argument_list = "(" [ argument *( "," argument ) ] ")"
        /// argument      = expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="argumentList"></param>
        public ArmElementExpressionAst(ArmExpressionAst expression, ArmBracketedArgumentListAst argumentList)
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

    }

}
