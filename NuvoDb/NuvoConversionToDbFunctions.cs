using System.Linq.Expressions;

namespace NuvoDb;

public static class NuvoConversionToDbFunctions
{
    public static readonly Expression<Func<string, string?>> StringToDb = x => x.Replace("\'", "\'");
    public static readonly Expression<Func<long, string?>> LongToDb = x => StringToDb.Compile().Invoke(x.ToString());
    public static readonly Expression<Func<long?, string?>> NullableLongToDb = x => x.HasValue ? LongToDb.Compile().Invoke(x.Value) : "null";
    public static readonly Expression<Func<int, string?>> IntToDb = x => StringToDb.Compile().Invoke(x.ToString());
    public static Expression<Func<int?, string?>> NullableIntToDb => x => x.HasValue ? IntToDb.Compile().Invoke(x.Value) : "null";
}
