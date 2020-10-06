using Kingsland.ParseFx.Syntax;
using Kingsland.ParseFx.Text;

namespace Kingsland.ArmLinter.Tokens
{

    public sealed class IdentifierToken : SyntaxToken
    {

        public IdentifierToken(string name)
            : this(SourceExtent.Empty, name)
        {
        }

        public IdentifierToken(SourceExtent extent, string name)
            : base(extent)
        {
            this.Name = name;
        }

        public string Name
        {
            get;
            private set;
        }

    }

}
