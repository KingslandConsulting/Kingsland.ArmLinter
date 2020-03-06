using Kingsland.ArmLinter.Tokens;
using System;

namespace Kingsland.ArmLinter.Ast
{

    public sealed class ArmStringLiteralExpressionAst : ArmExpressionAst
    {

        /// <summary>
        /// </summary>
        /// <param name="token"></param>
        public ArmStringLiteralExpressionAst(StringLiteralToken token)
        {
            this.Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public StringLiteralToken Token
        {
            get;
            private set;
        }

        public override string ToArmText()
        {
            return $"'{this.Token.Value}'";
        }

    }

}
