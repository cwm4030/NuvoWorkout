using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace NuvoDb;

public class NuvoModelMap(string modelName, string schemaName)
{
    public string ModelName { get; set; } = modelName;
    public string SchemaName { get; set; } = schemaName;
    public HashSet<string> ModelProperties => [.. _propertyMaps.Keys.Where(k => k.StartsWith(s_property)).Select(k => k.Replace(s_property, string.Empty))];
    public HashSet<string> TableColumns => [.. _propertyMaps.Keys.Where(k => k.StartsWith(s_column)).Select(k => k.Replace(s_column, string.Empty))];
    private static readonly string s_property = "property ";
    private static readonly string s_column = "column ";
    private readonly ConcurrentDictionary<string, INuvoPropertyMap> _propertyMaps = [];

    public void DefinePropertyMap<TModel, TValue>(string propertyName, string columnName, string columnType, bool columnRequired = false)
    {
        var propertyMap = new NuvoPropertyMap<TModel, TValue>
        {
            ModelName = ModelName,
            PropertyName = propertyName,
            ColumnName = columnName,
            ColumnType = columnType,
            ColumnRequired = columnRequired
        };

        var propertyInfo = typeof(TModel).GetProperty(propertyName);
        if (propertyInfo != null || propertyInfo?.PropertyType != null)
        {
            var instanceParameter = Expression.Parameter(typeof(TModel), "model");
            var valueParameter = Expression.Parameter(propertyInfo.PropertyType, "value");
            var propertyAccess = Expression.Property(instanceParameter, propertyInfo);
            var assignExpression = Expression.Assign(propertyAccess, valueParameter);
            var getter = Expression.Lambda<Func<TModel, TValue>>(propertyAccess, instanceParameter);
            var setter = Expression.Lambda<Action<TModel, TValue>>(assignExpression, instanceParameter, valueParameter);
            propertyMap.SetPropertyGetter(getter);
            propertyMap.SetPropertySetter(setter);
        }
        AddPropertyMap(propertyMap);
    }

    public void AddPropertyMap(INuvoPropertyMap propertyMap)
    {
        _propertyMaps.TryAdd(s_property + propertyMap.GetPropertyName(), propertyMap);
        _propertyMaps.TryAdd(s_column + propertyMap.GetColumnName(), propertyMap);
    }

    public INuvoPropertyMap? GetPropertyMapFromPropertyName(string propertyName)
    {
        return _propertyMaps.TryGetValue(s_property + propertyName, out var pm) ? pm : null;
    }

    public INuvoPropertyMap? GetPropertyMapFromColumnName(string columnName)
    {
        return _propertyMaps.TryGetValue(s_column + columnName, out var pm) ? pm : null;
    }
}
