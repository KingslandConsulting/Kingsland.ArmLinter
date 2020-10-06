using Kingsland.ArmLinter;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kingsland.ArmValidator
{

    class Program
    {

        private const string JsonSchemaDeploymentTemplate = "https://schema.management.azure.com/schemas/2015-01-01/deploymentTemplate.json#";
        private const string JsonSchemaDeploymentParameters = "https://schema.management.azure.com/schemas/2015-01-01/deploymentParameters.json#";

        static void Main()
        {

            //var armDir = "C:\\src\\github\\Azure\\azure-quickstart-templates";
            var armDir = "C:\\src\\github\\mikeclayton\\azure-quickstart-templates";
            var armFiles = Directory.GetFiles(armDir, "*.json", SearchOption.AllDirectories);

            var armLexer = ArmExpressionLexer.Create();

            foreach (var armFile in armFiles)
            {

                var armText = File.ReadAllText(armFile);
                var armJson = JToken.Parse(armText);

                var schema = armJson.Value<string>("$schema");
                switch (schema)
                {
                    case Program.JsonSchemaDeploymentTemplate:
                        break;
                    case Program.JsonSchemaDeploymentParameters:
                        continue;
                    default:
                        continue;
                }

                var armTokens = Program.VisitTokens(armJson)
                                       .Where(t => t.Type == JTokenType.String)
                                       .Select(t => (JValue)t)
                                       .Where(p => ArmStringValidator.IsArmExpression(p.Value<string>()) )
                                       .ToList();

                foreach (var armToken in armTokens)
                {
                    var armExpression = armToken.Value<string>();

                    // test cases for unit tests (eventaully)
                    //armExpression = "[concat('storage)]";  // mismatched quotes
                    //armExpression = "[concat('storage']";  // mismatched parens
                    //armExpression = "[concat()'storage']"; // unexpected token sequences

                    if (armExpression.StartsWith("[", StringComparison.InvariantCulture))
                    {
                        armExpression = armExpression.Substring(1);
                    }
                    if (armExpression.EndsWith("]", StringComparison.InvariantCulture))
                    {
                        armExpression = armExpression[0..^1];
                    }
                    Console.WriteLine(armExpression);
                    var tokens = armLexer.Lex(armExpression);
                    var ast = ArmExpressionParser.Parse(tokens);
                    foreach (var token in tokens)
                    {
                        Console.WriteLine($"    {token.Extent.Text}");
                    }
                }

            }

        }

        private static IEnumerable<JToken> VisitTokens(JToken value)
        {
            var stack = new Stack<JToken>(
                new List<JToken> { value }
            );
            while (stack.Count > 0)
            {
                var top = stack.Pop();
                switch (top.Type)
                {
                    case JTokenType.Array:
                        yield return top;
                        foreach (var item in ((JArray)top).Values().Reverse())
                        {
                            stack.Push(item);
                        }
                        break;
                    case JTokenType.Object:
                        yield return top;
                        foreach (var property in ((JObject)top).Properties().Reverse())
                        {
                            stack.Push(property);
                        }
                        break;
                    case JTokenType.Property:
                        stack.Push(((JProperty)top).Value);
                        break;
                    case JTokenType.Boolean:
                    case JTokenType.Date:
                    case JTokenType.Float:
                    case JTokenType.Integer:
                    case JTokenType.Null:
                    case JTokenType.String:
                        yield return (JValue)top;
                        break;
                    default:
                        throw new InvalidOperationException($"unhandled type '{top.Type.ToString()}'");
                };
            }
        }

    }

}
