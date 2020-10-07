using Kingsland.ArmLinter.Tokens;
using Kingsland.ParseFx.Syntax;
using System;

namespace Kingsland.ArmLinter.Ast
{

    public sealed class ArmNumericLiteralExpressionAst : ArmExpressionAst
    {

        /// <summary>
        /// </summary>
        /// <param name="token"></param>
        public ArmNumericLiteralExpressionAst(IntegerToken token)
        {
            this.Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public IntegerToken Token
        {
            get;
            private set;
        }

        public override string ToArmText()
        {
            return this.Token switch
            {
                IntegerToken integerToken => integerToken.Value.ToString(),
                _ => throw new NotImplementedException(),
            };
        }

    }

}
