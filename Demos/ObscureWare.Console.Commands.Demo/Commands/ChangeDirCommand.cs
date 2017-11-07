namespace ObscureWare.Console.Commands.Demo.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    [CommandModel(typeof(ChangeDirCommandModel))]
    public class ChangeDirCommand : IConsoleCommand, ICommandAutoCompletion
    {
        /// <inheritdoc />
        public void Execute(object contextObject, ICommandOutput output, object runtimeModel)
        {
            ChangeDirCommandModel model = runtimeModel as ChangeDirCommandModel;
            switch (model?.Target?.Trim())
            {
                case null:
                case "":
                case ".":
                    {
                        break; // remain;
                    }
                case "..":
                    {
                        DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
                        if (dir.FullName != dir.Root.FullName)
                        {
                            Environment.CurrentDirectory = dir.Parent?.FullName ?? dir.FullName; // stay
                        }
                        break;
                    }
                case "\\":
                    {
                        DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);
                        Environment.CurrentDirectory = dir.Root.FullName;
                        break;
                    }
                default:
                    {
                        string path = model.Target.Trim();
                        if (!Path.IsPathRooted(path))
                        {
                            Environment.CurrentDirectory = new DirectoryInfo(path).FullName;
                        }
                        else
                        {
                            path = Path.Combine(Environment.CurrentDirectory, path);
                            Environment.CurrentDirectory = new DirectoryInfo(path).FullName;
                        }
                        break;
                    }

                    //(contextObject as ConsoleContext).
            }
        }

        /// <inheritdoc />
        public IEnumerable<string> AutoCompleteCommand(object contextObject, CommandModel runtimeModel, string targetPropertyName, string matchText)
        {
            ChangeDirCommandModel model = runtimeModel as ChangeDirCommandModel;
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            switch (targetPropertyName)
            {
                case nameof(ChangeDirCommandModel.Target):
                    {
                        try
                        {
                            string rootFolder = Environment.CurrentDirectory;

                            // TODO: implement
                            // 1. check current folder
                            // 2. check if current + match exists
                            // 2a. if Yes - scan sub-folders and return
                            // 2b. if Not and does not end with directory delimiter - cut till last delimiter and use as path and remainder as StartsWith() filter
                        }
                        catch (Exception)
                        {
                            yield break; // just ignore invalid characters / or paths
                        }

                        break;
                    }
            }

            yield break;
        }
    }
}