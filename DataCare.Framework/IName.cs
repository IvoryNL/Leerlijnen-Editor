namespace DataCare.Framework
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq;

    [ContractClass(typeof(INameContract))]
    public interface IName
    {
        string Name { get; }
    }

    [ContractClassFor(typeof(IName))]
    public abstract class INameContract : IName
    {
        public string Name
        {
            get
            {
                Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

                throw new System.NotImplementedException();
            }
        }
    }
}