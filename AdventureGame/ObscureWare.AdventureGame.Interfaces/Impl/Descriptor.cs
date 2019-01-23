namespace ObscureWare.AdventureGame.Interfaces.Impl
{
    using System.Collections.Generic;

    public abstract class Descriptor : IDescriptor
    {
        /// <inheritdoc />
        public string DescriptionCode { get; set; }

        /// <inheritdoc />
        public string BaseName { get; set; }

        /// <inheritdoc />
        public INameBuilder NameBuilder { get; set; }

        /// <inheritdoc />
        public IReadOnlyDictionary<string, string> Descriptions { get; set; }
    }
}