namespace ObscureWare.Console.Operations.Interfaces.Tables
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Shared;

    /// <summary>
    /// DataTable object to store "dynamic" objects together with vectorizing function(s)
    /// </summary>
    /// <typeparam name="T">Underlying object</typeparam>
    public class DynamicDataTable<T> : IDataTable<T>
        //where T : INotifyPropertyChanged
    {
        private readonly Func<T, string[]> _vectorizingFunction;

        // For now - assuming that DataTable stores dynamic data, yet presents only snapshots of current state.

        private readonly List<T> _data = new List<T>();

        /// <summary>
        /// Initializes new instance of <see cref="DataTable{T}"/>
        /// </summary>
        /// <param name="vectorizingFunction">Function callback that will be used to generate cells from object</param>
        /// <param name="columns">Initial set of columns that table will contain.</param>
        public DynamicDataTable(
            Func<T, string[]> vectorizingFunction,
            params ColumnInfo[] columns)
        {
            if (vectorizingFunction == null)
            {
                throw new ArgumentNullException(nameof(vectorizingFunction));
            }

            this._vectorizingFunction = vectorizingFunction;
            this.Columns = columns.ToDictionary(c => c.Header, c => c);
        }

        /// <summary>
        /// Gets columns collection
        /// </summary>
        public Dictionary<string, ColumnInfo> Columns { get; }

        /// <summary>
        /// Adds new row to the table
        /// </summary>
        /// <param name="src">Underlying object of the row</param>
        public void AddRow(T src)
        {
            if (EqualityComparer<T>.Default.Equals(src, default(T)))
            {
                throw new ArgumentNullException(nameof(src));
            }

            string[] rowValues = this._vectorizingFunction(src);

            if (rowValues.SelectMany(row => row.ToCharArray()).Any(ch => ch.IsSystemChar()))
            {
                throw new ArgumentException("Row values cannot contain special characters. Clean data before adding it to the table.", nameof(rowValues));
            }

            this._data.Add(src);
        }

        public IEnumerable<string[]> GetRows()
        {
            return this._data.Select(i => this._vectorizingFunction(i));
        }

        /// <summary>
        /// Finds first value that is identified by value stored in the first column or NULL
        /// </summary>
        /// <param name="aIdentifier">Artificial Idx identifier to be used for searching in the first column</param>
        /// <returns></returns>
        public T GetUnderlyingValue(string aIdentifier)
        {
            return this._data.FirstOrDefault(i => this._vectorizingFunction(i).First().Equals(aIdentifier, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Finds first value that matches given predicate filtering function or NULL
        /// </summary>
        /// <param name="identifier">Identifier to be found in stored objects</param>
        /// <param name="matchingFunc">Matching function. When it returns true for given object, that row will be returned.</param>
        /// <returns>First found matching object or NULL</returns>
        public T FindValueWhere(string identifier, Func<T, string, bool> matchingFunc)
        {
            return this._data.FirstOrDefault(i => matchingFunc(i, identifier));
        }

        /// <summary>
        /// Builds indexed table, that provides extra Idx column to be used with cached results
        /// </summary>
        /// <typeparam name="TKey">Type of data stored in the table</typeparam>
        /// <param name="header">Column headers</param>
        /// <param name="dataSource">Rows data source</param>
        /// <param name="vectorizingFunction">Row generating callback function</param>
        /// <returns>Artificial table object</returns>
        public static IDataTable<TKey> BuildIndexedTable<TKey>(string[] header, IEnumerable<TKey> dataSource,
            Func<TKey, string[]> vectorizingFunction)
        {
            uint scopedIndexer = 1;
            DynamicDataTable<TKey> table = new DynamicDataTable<TKey>(
                (src) => new[] { (scopedIndexer++).ToAlphaEnum() + '.'}.Concat(vectorizingFunction.Invoke(src)).ToArray(), // decorating fn
                new[] { "Idx" }.Concat(header).Select(head => new ColumnInfo(head)).ToArray());

            foreach (TKey src in dataSource)
            {
                table.AddRow(src);
            }

            return table;
        }
    }
}