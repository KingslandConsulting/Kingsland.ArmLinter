Kingsland.ArmLinter
===================

Overview
--------

Kingsland.ArmLinter is a C# library for parsing and validating the contents of Azure ARM Template files.

The motiviation for this library was too many late nights committing an ARM template with an embedded expression along the lines of:

```
"name": "[concat('storage', uniqueString(resourceGroup().id))]"
```

and pushing it through my build and deployment pipeline only to find out 5 minutes later that I'd missed a ')' somewhere in the expression, or misspelled a built-in function name.

Lexing
------

The project can be used to lex and parse all of the expressions in a deployment template (see the ```Kingsland.ArmValidator``` project for an example).

You can currently detect the following types of errors:

+ ```[concat('storage)]``` - mismatched quotes in strings
+ ```[concat('storage']``` - mismatched brackets:
+ ```[concat()'storage']``` - unexpected token sequences

At present, it won't validate the argument signature of functions - e.g.

+ ```toLower('value1', 'value2')``` - too many arguments
+ ```toLower(100)``` - incorrect argument type

Evaluating
----------

The library also supports evaluating a very limited subset of ARM Template functions inside expression strings - for example:

```csharp
ArmExpressionEvaluator.Evaluate("concat('hello', 'brave', 'new', 'world')");
// returns "hellobravenewworld"

ArmExpressionEvaluator.Evaluate("concat('hello', concat('brave', 'new'), 'world')");
// also returns "hellobravenewworld"

ArmExpressionEvaluator.Evaluate("base64('one, two, three')");
// returns "b25lLCB0d28sIHRocmVl"

ArmExpressionEvaluator.Evaluate("toUpper('One Two Three')");
// returns "ONE TWO THREE"

ArmExpressionEvaluator.Evaluate("concat(toLower('ONE'), '-', toUpper('two'), '-', base64('three'))");
// returns "one-TWO-dGhyZWU="

```
Note - there's only a few functions that currently work:

* ```concat```
* ```base64```
* ```base64ToString```
* ```toLower```
* ```toUpper```

but the code is structured to allow easy addition of new functions, so post an issue (or send a PR) if there's one you'd like to be supported.

Roadmap
-------

This project still currently a bit of a work in progress - here's the things it still needs...

| Target             |   Progress  |
| ------------------ | ----------- |
| Write the lexer    |    Done!    |
| Write the parser   |    Done!    |
| Write an evaluator | In Progress |
| Write an analyzer  | Not Started |
