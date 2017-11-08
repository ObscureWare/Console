namespace ObscureWare.Console.Operations.Interfaces.Tables
{
    using System.Collections.Generic;

    /// <summary>
    /// Core functionality for a DataTable collection.
    /// </summary>
    /// <typeparam name="T">Stored objects type</typeparam>
    public interface IDataTable<T>
    {
        /// <summary>
        /// Gets columns collection
        /// </summary>
        Dictionary<string, ColumnInfo> Columns { get; }

        IEnumerable<string[]> GetRows();

        /// <summary>
        /// Finds first value that is identified by value stored in the first column or NULL
        /// </summary>
        /// <param name="aIdentifier">Artificial Idx identifier to be used for searching in the first column</param>
        /// <returns></returns>
        T GetUnderlyingValue(string aIdentifier);

        /// <summary>
        /// Gets number of rows
        /// </summary>
        int RowCount { get; }
    }
}