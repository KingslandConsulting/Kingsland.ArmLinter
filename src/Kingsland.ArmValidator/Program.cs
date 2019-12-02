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
        static void Main()
        {

            var armDir = "C:\\src\\github\\Azure\\azure-quickstart-templates";
            var armFiles = Directory.GetFiles(armDir, "*.json", SearchOption.AllDirectories);

            foreach (var armFile in armFiles)
            {

                var armText = File.ReadAllText(armFile);
                var armJson = JToken.Parse(armText);
                var armTokens = Program.VisitTokens(armJson)
                                       .Where(t => t.Type == JTokenType.String)
                                       .Select(t => (JValue)t)
                                       .Where(p => ArmStringValidator.IsArmExpression(p.Value<string>()) )
                                       .ToList();

                foreach (var armToken in armTokens)
                {
                    var text = armToken.Value<string>();
                    Console.WriteLine(text);
                    var tokens = ArmExpressionLexer.Lex(text);
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
