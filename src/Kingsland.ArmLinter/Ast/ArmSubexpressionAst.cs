using System;

namespace Kingsland.ArmLinter.Ast
{

    public sealed class ArmSubexpressionAst : ArmExpressionAst
    {

        public ArmSubexpressionAst(ArmExpressionAst subexpression)
        {
            this.Subexpression = subexpression ?? throw new ArgumentNullException(nameof(subexpression));
        }

        public ArmExpressionAst Subexpression
        {
            get;
            private set;
        }

    }

}
