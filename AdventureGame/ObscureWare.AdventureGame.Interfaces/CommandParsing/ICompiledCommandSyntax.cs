namespace ObscureWare.AdventureGame.Interfaces.CommandParsing
{
    using Parsing;

    public interface ICompiledCommandSyntax
    {
        /// <summary>
        /// This is rough match of keyword and number of arguments (except white-words)
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        bool Matches(params string[] args);

        /// <summary>
        /// Detailed analysis of arguments, including objects resolution and verification of other aspects of the command
        /// </summary>
        /// <param name="world"></param>
        /// <param name="args"></param>
        ExecutionObject TryBuildExecution(IGameWorld world, params string[] args);

        /// <summary>
        /// Index of keyword in the arguments of this syntax version
        /// </summary>
        int KeywordIndex { get; }
    }
}