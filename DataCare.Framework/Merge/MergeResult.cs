namespace DataCare.Framework.Merge
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FSharpx;
    using Microsoft.FSharp.Core;

    public enum MergeResolution
    {
        Equal, Yours, Theirs, New
    }

    [Flags]
    public enum MergeResolutionChoice
    {
        KeepConflict = 1,
        ResolveConflict = 2,
        ChooseProposal = 4,
        ChooseYours = 8,
        ChooseTheirs = 16,
        ChooseAncestor = 32
    }

    public static class MergeResolutionHelpers
    {
        public static MergeResolution Minimum(MergeResolution x, MergeResolution y)
        {
            return x == y || y == MergeResolution.Equal
                ? x
                : x == MergeResolution.Equal
                    ? y
                    : MergeResolution.New;
        }

        public static MergeResolution GetResolutionMinimum(IEnumerable<IMergeResult> mergeresults)
        {
            return mergeresults.Select(mergeresult => mergeresult.Resolution).Aggregate(Minimum);
        }

        public static IMergeResult<T> ResolveWithResolutionChoice<T>(T proposal, T yours, T theirs, T ancestor, MergeResolutionChoice resolutionChoice)
        {
            if (resolutionChoice.HasFlag(MergeResolutionChoice.ResolveConflict))
            {
                return MergeResult<T>.IsSuccess(proposal, yours, theirs, ancestor);
            }
            else
            {
                return MergeResult<T>.IsConflict(proposal, yours, theirs, ancestor);
            }
        }

        public static IMergeResult<T> Resolve<T>(T yours, T theirs, T ancestor, Func<T, IDictionary<string, dynamic>, object[]> getConstructorArguments, IDictionary<string, IMergeResult> mergeresults, MergeResolutionChoice resolutionOption)
        {
            MergeResolution mergeResolution = MergeResolutionHelpers.GetResolutionMinimum(mergeresults.Values);

            switch (mergeResolution)
            {
                case MergeResolution.Equal:
                    return MergeResult<T>.IsEqual(yours, theirs, ancestor);

                case MergeResolution.Theirs:
                    return MergeResult<T>.IsTheirs(yours, theirs, ancestor);
            }

            if (mergeresults.Any(result => result.Value.Conflict))
            {
                IDictionary<string, dynamic> parameterResults = null;

                if (resolutionOption.HasFlag(MergeResolutionChoice.ChooseYours))
                {
                    // is yours hier kiezen (geen conflict) niet gevaarlijk? de audit trail van theirs (server) raak je kwijt
                    // en TBH lijkt hier nu niet tegen te kunnen omdat administratieve locatie van klant dan niet meer gevonden kan worden.
                    parameterResults = mergeresults
                        .Select(kvp => new { kvp.Key, Value = kvp.Value.Conflict ? kvp.Value.Yours : kvp.Value.Result })
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
                else if (resolutionOption.HasFlag(MergeResolutionChoice.ChooseTheirs))
                {
                    parameterResults = mergeresults
                        .Select(kvp => new { kvp.Key, Value = kvp.Value.Conflict ? kvp.Value.Theirs : kvp.Value.Result })
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
                else if (resolutionOption.HasFlag(MergeResolutionChoice.ChooseAncestor))
                {
                    parameterResults = mergeresults
                        .Select(kvp => new { kvp.Key, Value = kvp.Value.Conflict ? kvp.Value.Ancestor : kvp.Value.Result })
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }
                else
                {
                    parameterResults = mergeresults
                        .Select(kvp => new { kvp.Key, Value = kvp.Value.Result })
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }

                if (resolutionOption.HasFlag(MergeResolutionChoice.ResolveConflict))
                {
                    return MergeResult<T>.IsSuccess((T)Activator.CreateInstance(typeof(T), getConstructorArguments(theirs, parameterResults)), yours, theirs, ancestor);
                }
                else
                {
                    return MergeResult<T>.IsConflict((T)Activator.CreateInstance(typeof(T), getConstructorArguments(theirs, parameterResults)), yours, theirs, ancestor);
                }
            }
            else
            {
                IDictionary<string, dynamic> parameterResults = mergeresults
                    .Select(kvp => new { kvp.Key, Value = kvp.Value.Result })
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                return MergeResult<T>.IsSuccess((T)Activator.CreateInstance(typeof(T), getConstructorArguments(theirs, parameterResults)), yours, theirs, ancestor);
            }
        }
    }

    public abstract class MergeResult { }

    public class MergeResult<T> : MergeResult, IMergeResult<T>
    {
        private MergeResult(T result)
        {
            Conflict = false;
            Result = result;
            Resolution = MergeResolution.New;
        }

        private MergeResult(T result, T yours, T theirs, T ancestor) :
            this(result, yours, theirs, ancestor, true, MergeResolution.New)
        { }

        private MergeResult(T result, T yours, T theirs, T ancestor, bool conflict, MergeResolution resolution)
        {
            Conflict = conflict;
            Result = result;
            Yours = yours;
            Theirs = theirs;
            Ancestor = ancestor;
            Resolution = resolution;
        }

        public static MergeResult<T> IsEqual(T yours, T theirs, T ancestor)
        {
            var result = new MergeResult<T>(yours, yours, theirs, ancestor);
            result.Conflict = false;
            result.Resolution = MergeResolution.Equal;
            return result;
        }

        public static MergeResult<T> IsYours(T yours, T theirs, T ancestor)
        {
            var result = new MergeResult<T>(yours, yours, theirs, ancestor);
            result.Conflict = false;
            result.Resolution = MergeResolution.Yours;
            return result;
        }

        public static MergeResult<T> IsTheirs(T yours, T theirs, T ancestor)
        {
            var result = new MergeResult<T>(theirs, yours, theirs, ancestor);
            result.Conflict = false;
            result.Resolution = MergeResolution.Theirs;
            return result;
        }

        public static MergeResult<T> IsConflict(T result, T yours, T theirs, T ancestor)
        {
            return new MergeResult<T>(result, yours, theirs, ancestor);
        }

        public static MergeResult<T> IsSuccess(T result, T yours, T theirs, T ancestor)
        {
            return new MergeResult<T>(result, yours, theirs, ancestor, false, MergeResolution.New);
        }

        public static MergeResult<FSharpOption<T>> ToFSharpOption(IMergeResult<T> result)
        {
            return new MergeResult<FSharpOption<T>>(result.Result.ToFSharpOption(), result.Yours.ToFSharpOption(), result.Theirs.ToFSharpOption(), result.Ancestor.ToFSharpOption(), result.Conflict, result.Resolution);
        }

        public bool Conflict { get; private set; }
        public T Result { get; private set; }
        public T Yours { get; private set; }
        public T Theirs { get; private set; }
        public T Ancestor { get; private set; }
        object IMergeResult.Result { get { return Result; } }
        object IMergeResult.Yours { get { return Yours; } }
        object IMergeResult.Theirs { get { return Theirs; } }
        object IMergeResult.Ancestor { get { return Ancestor; } }

        public MergeResolution Resolution { get; private set; }
    }

    public interface IMergeResult
    {
        bool Conflict { get; }
        MergeResolution Resolution { get; }
        object Result { get; }
        object Yours { get; }
        object Theirs { get; }
        object Ancestor { get; }
    }

    public interface IMergeResult<out T> : IMergeResult
    {
        new T Result { get; }
        new T Yours { get; }
        new T Theirs { get; }
        new T Ancestor { get; }
    }
}