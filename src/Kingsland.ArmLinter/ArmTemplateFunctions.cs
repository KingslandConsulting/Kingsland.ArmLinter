using Kingsland.ArmLinter.Functions;

namespace Kingsland.ArmLinter
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// see https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions
    /// </summary>
    public static class ArmTemplateFunctions
    {

        public static readonly ArrayFunctions Array = new ArrayFunctions();
        public static readonly StringFunctions String = new StringFunctions();

    }

}
