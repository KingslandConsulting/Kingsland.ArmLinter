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

Current Status
--------------

The project can be used to lex and parse all of the expressions in a deployment template (see the ```Kingsland.ArmValidator``` project for an example).

You can currently detect the following types of errors:

+ ```[concat('storage)]``` - mismatched quotes in strings
+ ```[concat('storage']``` - mismatched brackets:
+ ```[concat()'storage']``` - unexpected token sequences

At present, it won't validate the argument signature of functions - e.g.

+ ```toLower('value1', 'value2')``` - too many arguments
+ ```toLower(100)``` - incorrect argument type

Roadmap
-------

This project still currently a bit of a work in progress - here's the things it still needs...

| Target            |   Progress  |
| ----------------- | ----------- |
| Write the lexer   |    Done!    |
| Write the parser  |    Done!    |
| Write an analyzer | Not Started |
