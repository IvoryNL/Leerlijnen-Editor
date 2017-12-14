// -----------------------------------------------------------------------
// <copyright file="IHaveBuilder.cs" company="DataCare BV">
// </copyright>
// -----------------------------------------------------------------------

namespace DataCare.Model
{
    public interface IHaveBuilder<T, out TBuilder>
        where T : IHaveBuilder<T, TBuilder>
        where TBuilder : IAmBuilderFor<T>
    {
        TBuilder ToBuilder();
    }

    public interface IAmBuilderFor<TBuilt>
    {
        TBuilt Build();
    }

    public interface IAmBuilder<TBuilder>
    {
        TBuilder Clone();
    }
}