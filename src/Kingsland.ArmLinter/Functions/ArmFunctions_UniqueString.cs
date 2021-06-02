using System;

namespace Kingsland.ArmLinter.Functions
{

    /// <summary>
    /// Implementation of built-in ARM Template functions.
    /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string
    /// </summary>
    public static partial class ArmFunctions
    {

        #region UniqueString

        /// <summary>
        /// Creates a deterministic hash string based on the values provided as parameters.
        /// </summary>
        /// <returns>A string containing 13 characters.</returns>
        /// <param name="baseString">The value used in the hash function to create a unique string.</param>
        /// <param name="args">You can add as many strings as needed to create the value that specifies the level of uniqueness.</param>
        /// <remarks>
        /// This function is helpful when you need to create a unique name for a resource.
        /// You provide parameter values that limit the scope of uniqueness for the result.
        /// You can specify whether the name is unique down to subscription, resource group,
        /// or deployment.
        ///
        /// The returned value isn't a random string, but rather the result of a hash function.
        /// The returned value is 13 characters long. It isn't globally unique. You may want to
        /// combine the value with a prefix from your naming convention to create a name that
        /// is meaningful.The following example shows the format of the returned value. The
        /// actual value varies by the provided parameters.
        ///
        /// See https://docs.microsoft.com/en-us/azure/azure-resource-manager/templates/template-functions-string#uniquestring
        /// </remarks>
        public static string UniqueString(string baseString, params string[] args)
        {

            // applies a currently-unknown deterministic hash algorithm to turn the
            // arguments into a predictable 13-character string. the same input always
            // produces the same hash value.

            // the algorithm can be executed by deploying the following arm template:

            // {
            //   "$schema": "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
            //   "contentVersion": "1.0.0.0",
            //   "resources" : [ ],
            //   "outputs": {
            //     "uniqueString": {
            //       "type": "string",
            //       "value": "[uniqueString('xxx'))]"
            //     }
            //   }
            // }

            // PS> New-AzResourceGroupDeployment -ResourceGroupName "my-resourcegroup" -TemplateFile "C:\temp\template.json"

            // example output:
            //
            //   uniqueString("aaa") => "zavj67f3en4hc"
            //   uniqueString("bbb") => "w5eragfxe33cy"
            //   uniqueString("zzz") => "cryksdzzzawae"
            //   uniqueString("xxx", "yyy", "zzz") => "lghi7k5js7itq"

            // the actual algorithm is not published anywhere, and there's seemingly no information
            // that will allow us to replicate it. here's some links to the limited information that
            // *is* available:
            //
            //   + https://stackoverflow.com/questions/43295720/azure-arm-uniquestring-function-mimic
            //   + https://stackoverflow.com/questions/64119824/reimplement-the-uniquestring-hash-arm-function
            //   + https://docs.microsoft.com/en-gb/archive/blogs/389thoughts/get-uniquestring-generate-unique-id-for-azure-deployments

            if (baseString == null)
            {
                throw new ArgumentNullException(nameof(baseString));
            }

            throw new NotImplementedException();

        }

        #endregion

    }

}
