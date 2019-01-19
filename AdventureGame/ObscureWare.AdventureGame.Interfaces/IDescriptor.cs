namespace ObscureWare.AdventureGame.Interfaces
{
    using System.Collections.Generic;

    public interface IDescriptor
    {
        string DescriptionCode { get; }

        string BaseName { get; }

        INameBuilder NameBuilder { get; }

        IReadOnlyDictionary<string, string> Descriptions { get; }
    }
}