﻿using Kingsland.ArmLinter.Tokens;
using Kingsland.ParseFx.Lexing;
using System;

namespace Kingsland.ArmLinter.Ast
{

    public sealed class ArmMemberAccessExpressionAst : ArmExpressionAst
    {

        /// <summary>
        /// member_access = expression operator member_name
        /// operator      = "."
        /// member_name   = identifier
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="name"></param>
        public ArmMemberAccessExpressionAst(ArmExpressionAst expression, Token operatorToken, IdentifierToken name)
        {
            this.Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            this.OperatorToken = operatorToken ?? throw new ArgumentNullException(nameof(operatorToken));
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public ArmExpressionAst Expression
        {
            get;
            private set;
        }

        public Token OperatorToken
        {
            get;
            private set;
        }

        public IdentifierToken Name
        {
            get;
            private set;
        }

    }

}
