using Kingsland.ArmLinter;
using System;

namespace Kingsland.ArmSamples
{

    public static class ArmExpressionEvaluatorSample
    {

        public static void RunSample()
        {

            var expression = "concat('hello', concat('brave', 'new'), 'world')";
            Console.WriteLine($"expression = '{expression}'");

            var ast = ArmExpressionParser.Parse(expression);

            var result = ArmExpressionEvaluator.Evaluate(ast);
            Console.WriteLine($"result = '{result}'");

        }

    }

}
