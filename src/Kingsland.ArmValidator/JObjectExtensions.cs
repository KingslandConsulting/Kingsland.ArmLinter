using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Kingsland.ArmValidator
{

    internal static class JObjectExtensions
    {

        public static IEnumerable<JToken> BreadthFirst(this JObject obj)
        {
            var unprocessed = new Stack<JToken>(obj.Children());
            while (unprocessed.Count > 0)
            {
                var token = unprocessed.Pop();
                yield return token;
                foreach (var child in token)
                {
                    unprocessed.Push(child);
                }
            }
        }

    }

}
