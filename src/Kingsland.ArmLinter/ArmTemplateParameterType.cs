namespace Kingsland.ArmLinter
{

    public static class ArmTemplateParameterType
    {

        // Microsoft.Rest.Azure.CloudException :
        // The request content was invalid and could not be deserialized:
        // 'Error converting value "Null" to type
        // 'Azure.Deployments.Core.Entities.TemplateParameterType'.
        // Path '', line 9, position 24.'.

        //public static readonly string Null = "Null";

        public static readonly string String = "String";
        public static readonly string Array = "Array";
        public static readonly string Bool = "Bool";
        public static readonly string Int = "Int";
        public static readonly string Object = "Object";

        //public static readonly string SecureObject = "secureObject";
        //public static readonly string SecureString = "secureString";

    }
}
