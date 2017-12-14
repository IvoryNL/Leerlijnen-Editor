namespace DataCare.Framework.Merge
{
    using System;
    using System.Linq;

    public interface IMerge
    {
    }

    public interface IMerge<T> : IMerge
    {
        IMergeResult<T> Merge(T yours, T theirs, T ancestor, MergeResolutionChoice resolutionChoice);
    }
}