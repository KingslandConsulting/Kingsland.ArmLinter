using System;

namespace Kingsland.ArmLinter.Ast
{

    public sealed class ArmInvocationExpressionAst : ArmExpressionAst
    {

        /// <summary>
        /// invocation    = expression argument_list
        /// argument_list = "(" [ argument *( "," argument ) ] ")"
        /// argument      = expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="argumentList"></param>
        public ArmInvocationExpressionAst(ArmExpressionAst expression, ArmArgumentListAst argumentList)
        {
            this.Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            this.ArgumentList = argumentList ?? throw new ArgumentNullException(nameof(argumentList));
        }

        public ArmExpressionAst Expression
        {
            get;
            private set;
        }

        public ArmArgumentListAst ArgumentList
        {
            get;
            private set;
        }

    }

}
