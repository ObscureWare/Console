namespace ObscureWare.Console.Commands.Engine.Internals
{
    using System;
    using System.Linq;

    using ObscureWare.Console.Commands.Interfaces;
    using ObscureWare.Console.Commands.Interfaces.Model;

    internal class ConsoleCommandBuilder
    {
        /// <summary>
        /// Validates command definitions and creates instance
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns>Tuple: commandName, CommandModel, Command instance</returns>
        public Tuple<CommandModelBuilder, IConsoleCommand> ValidateAndBuildCommand(Type commandType)
        {
            // 1. Verify command type
            if (commandType == null)
                throw new ArgumentNullException(nameof(commandType));
            if (commandType.IsAbstract || commandType.IsInterface)
            {
                return null; // ignore abstract layer...
            };
            if (!typeof(IConsoleCommand).IsAssignableFrom(commandType))
                throw new ArgumentException($"Command type must implement {nameof(IConsoleCommand)} interface.", nameof(commandType));

            // 1a. Model attribute check
            var modelAtt =
                (CommandModelAttribute)
                commandType.GetCustomAttributes(typeof(CommandModelAttribute), inherit: true).FirstOrDefault();
            if (modelAtt == null)
                throw new ArgumentException($"Command type must be decorated with {nameof(CommandModelAttribute)}.", nameof(commandType));

            // 2. Verify that code of the command doe snot directly call System.Console type, which might be a very common error
            this.VerifyCodeForReference(typeof(Console));

            // 3. Build / Verify Command's model type
            var mBuilder = this.ValidateModel(modelAtt.ModelType, commandType);

            // 4. Try creating instance of the command.
            IConsoleCommand instance;
            try
            {
                instance = (IConsoleCommand)Activator.CreateInstance(commandType);
            }
            catch (Exception iex)
            {
                throw new InvalidOperationException($"Could not create instance of the command {commandType.FullName} using default activator. Make sure it exposes public, parameterless constructor.", iex);
            }

            return new Tuple<CommandModelBuilder, IConsoleCommand>(mBuilder, instance);
        }

        private void VerifyCodeForReference(Type type)
        {
            //foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static) )
            //{
            //    if (methodInfo.IsAbstract)
            //    {
            //        continue;
            //    }

            //    var body = methodInfo.GetMethodBody();

            //    // TODO: implement somewhere in time
            //}
        }

        private CommandModelBuilder ValidateModel(Type modelType, Type commandType)
        {
            if (modelType == null)
                throw new ArgumentException($"Model type of {nameof(CommandModelAttribute)} cannot be null.", nameof(modelType));
            if (modelType.IsAbstract || modelType.IsInterface)
                throw new ArgumentException($"Model type cannot be abstract or interface.", nameof(modelType));
            if (!typeof(CommandModel).IsAssignableFrom(modelType))
                throw new ArgumentException($"Model type must inherit from {nameof(CommandModel)}.", nameof(modelType));

            // 2a. Model reverse pointer attribute check
            var reverseTypeAtt = (CommandModelForAttribute)
                modelType.GetCustomAttributes(typeof(CommandModelForAttribute), inherit: true).FirstOrDefault();
            if (reverseTypeAtt == null)
                throw new ArgumentException(
                    $"Model type must be decorated with {nameof(CommandModelForAttribute)}.", nameof(modelType));
            if (reverseTypeAtt.ModelledCommandType != commandType)
                throw new ArgumentException(
                    $"Model type attribute {nameof(CommandModelForAttribute)} must point back to the command type.",
                    nameof(modelType));

            // 2b. Model, command name attribute check
            var modelNameAtt =
                (CommandNameAttribute)
                modelType.GetCustomAttributes(typeof(CommandNameAttribute), inherit: true).FirstOrDefault();
            if (modelNameAtt == null)
                throw new ArgumentException($"Model type must be decorated with {nameof(CommandNameAttribute)}.", nameof(modelType));
            if (string.IsNullOrWhiteSpace(modelNameAtt.CommandName))
                throw new ArgumentException($"Model type must be decorated with {nameof(CommandNameAttribute)}.", nameof(modelType));

            return  new CommandModelBuilder(modelType, modelNameAtt.CommandName);
        }
    }
}