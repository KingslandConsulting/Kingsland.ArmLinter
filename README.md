Kingsland.ArmLinter
===================

Overview
--------

Kingsland.ArmLinter is a C# library for parsing and validating the contents of Azure ARM Template files.

The motiviation for this library was too many late nights committing an ARM template with an embedded expression along the lines of:

```
"name": "[concat('storage', uniqueString(resourceGroup().id))]"
```

and pushing it through my build and deployment pipeline only to find out 5 minutes later that I'd missed a ')' somwhere in the expression, or misspelled a built-in function name.

Current Status
--------------

The project can be used to lex all of the expressions in a template (see the ```Kingsland.ArmValidator``` project) but it won't yet parse them.

This means, for example, you can currently detect unmatched quotes in strings (e.g. ```'storage```) because the lexer will thrown an exception:

```
"name": "[concat('storage)]"
```

But it won't detect unmatched brackets (e.g. ```('storage'```) unless you analyse the lexer tokens yourself:

```
"name": "[concat('storage']"
```

Roadmap
-------

This project still currently a bit of a work in progress - here's the things it still needs...

| Target           |   Progress  |
| ---------------- | ----------- |
| Write the lexer  |    Done!    |
| Write the parser | Not Started |

(Ultimately I'd love to learn how to make this a Visual Studio Code plugin, but baby steps...)
