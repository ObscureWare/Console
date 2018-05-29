namespace ObscureWare.Console.Root.Desktop
{
    // Copyright (C) Microsoft.  All rights reserved.
    // Licensed under the terms described in the LICENSE file in the root of this project.
    //

    // Based on e-MIT-ed source from https://github.com/Microsoft/console/blob/master/tools/ColorTool/ColorTool/ISchemeParser.cs
    // Adapted to be used and reuse ObscureWare's Console library

    interface ISchemeParser
    {
        string Name { get; }

        ColorScheme ParseScheme(string schemeName, bool reportErrors = true);
    }
}