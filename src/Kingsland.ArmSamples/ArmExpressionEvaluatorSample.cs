using Kingsland.ArmLinter;
using System;

namespace Kingsland.ArmSamples
{

    public static class ArmExpressionEvaluatorSample
    {

        public static void RunSample()
        {

            //var expression = "concat('hello', concat('brave', 'new'), 'world')";
            var expression = "toLower('HELLO')";
            Console.WriteLine($"expression = '{expression}'");

            var result = ArmExpressionEvaluator.Evaluate(expression);

            Console.WriteLine($"result = '{result}'");

        }

    }

}
