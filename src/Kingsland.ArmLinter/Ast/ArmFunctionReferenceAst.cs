using Kingsland.ArmLinter.Tokens;
using System;

namespace Kingsland.ArmLinter.Ast
{

    public sealed class ArmFunctionReferenceAst : ArmExpressionAst
    {

        /// <summary>
        /// function_reference = function_name
        /// function_name      = identifier
        /// </summary>
        /// <param name="name"></param>
        public ArmFunctionReferenceAst(IdentifierToken name)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public IdentifierToken Name
        {
            get;
            private set;
        }

        public override string ToArmText()
        {
            return this.Name.Name;
        }

    }

}