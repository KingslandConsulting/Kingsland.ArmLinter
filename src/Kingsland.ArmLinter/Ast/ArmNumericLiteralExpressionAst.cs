using Kingsland.ArmLinter.Tokens;
using Kingsland.ParseFx.Lexing;
using System;

namespace Kingsland.ArmLinter.Ast
{

    public sealed class ArmNumericLiteralExpressionAst : ArmExpressionAst
    {

        /// <summary>
        /// </summary>
        /// <param name="token"></param>
        public ArmNumericLiteralExpressionAst(Token token)
        {
            this.Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public Token Token
        {
            get;
            private set;
        }

    }

}
