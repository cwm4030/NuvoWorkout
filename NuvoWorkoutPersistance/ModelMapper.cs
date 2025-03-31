using System.Collections.Concurrent;
using System.Data.Common;
using System.Linq.Expressions;

namespace NuvoWorkoutPersistance;

public class ModelMapper
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, PropertyMapping>> _propertyMappings = [];
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, PropertyMapping>> _columnMappings = [];

    public async Task<List<TModel>> MapModels<TModel>(DbDataReader reader)
    {
        List<TModel> models = [];
        var columnNameIndexes = GetColumnNameIndexes(reader);
        var modelType = typeof(TModel).Name;
        var columnMappings = _columnMappings.TryGetValue(modelType, out var cm) ? cm : [];
        while (await reader.ReadAsync())
        {
            var model = Activator.CreateInstance<TModel>();
            foreach (var (columnName, index) in columnNameIndexes)
            {
                if (reader.IsDBNull(index)) continue;
                var propertyMapping = columnMappings.TryGetValue(columnName, out var pn) ? pn : null;
                if (propertyMapping == null) continue;

                var value = propertyMapping?.PropertyAccessor?.Cast(reader[columnName]);
                propertyMapping?.PropertyAccessor?.SetValue(model, value);
            }
            models.Add(model);
        }
        return models;
    }

    public void AddPropertyMap<TModel, TValue>(string propName, string columnName, string columnType, bool columnRequired = false)
    {
        AddToPropertyMappings<TModel, TValue>(propName, columnName, columnType, columnRequired);
        AddToColumnyMappings<TModel, TValue>(propName, columnName, columnType, columnRequired);
    }

    public static List<(string, int)> GetColumnNameIndexes(DbDataReader reader)
    {
        List<(string, int)> columnNameIndexes = [];
        for (var i = 0; i < reader.FieldCount; i++)
            columnNameIndexes.Add((reader.GetName(i), i));
        
        return columnNameIndexes;
    }

    private void AddToPropertyMappings<TModel, TValue>(string propName, string columnName, string columnType, bool columnRequired)
    {
        var propertyAccessor = GetPropertyAccessor<TModel, TValue>(propName);
        var modelType = typeof(TModel).Name;
        if (_propertyMappings.TryGetValue(modelType, out var mpm))
        {
            if (mpm.TryGetValue(propName, out var pm))
            {
                pm = new PropertyMapping
                {
                    PropertyName = propName,
                    ColumnName = columnName,
                    ColumnType = columnType,
                    ColumnRequired = columnRequired,
                    PropertyAccessor = propertyAccessor
                };
            }
            else
            {
                mpm.TryAdd(propName, new PropertyMapping
                {
                    PropertyName = propName,
                    ColumnName = columnName,
                    ColumnType = columnType,
                    ColumnRequired = columnRequired,
                    PropertyAccessor = propertyAccessor
                });
            }
        }
        else
        {
            var pm = new ConcurrentDictionary<string, PropertyMapping>();
            pm.TryAdd(propName, new PropertyMapping
            {
                PropertyName = propName,
                ColumnName = columnName,
                ColumnType = columnType,
                ColumnRequired = columnRequired,
                PropertyAccessor = propertyAccessor
            });
            _propertyMappings.TryAdd(modelType, pm);
        }
    }

    private void AddToColumnyMappings<TModel, TValue>(string propName, string columnName, string columnType, bool columnRequired)
    {
        var propertyAccessor = GetPropertyAccessor<TModel, TValue>(propName);
        var modelType = typeof(TModel).Name;
        if (_columnMappings.TryGetValue(modelType, out var mcm))
        {
            if (mcm.TryGetValue(columnName, out var pm))
            {
                pm = new PropertyMapping
                {
                    PropertyName = propName,
                    ColumnName = columnName,
                    ColumnType = columnType,
                    ColumnRequired = columnRequired,
                    PropertyAccessor = propertyAccessor
                };
            }
            else
            {
                mcm.TryAdd(columnName, new PropertyMapping
                {
                    PropertyName = propName,
                    ColumnName = columnName,
                    ColumnType = columnType,
                    ColumnRequired = columnRequired,
                    PropertyAccessor = propertyAccessor
                });
            }
        }
        else
        {
            var cm = new ConcurrentDictionary<string, PropertyMapping>();
            cm.TryAdd(columnName, new PropertyMapping
            {
                PropertyName = propName,
                ColumnName = columnName,
                ColumnType = columnType,
                ColumnRequired = columnRequired,
                PropertyAccessor = propertyAccessor
            });
            _columnMappings.TryAdd(modelType, cm);
        }
    }

    private static PropertyAccessor<TModel, TValue>? GetPropertyAccessor<TModel, TValue>(string propName)
    {
        var propertyInfo = typeof(TModel).GetProperty(propName);
        if (propertyInfo == null || propertyInfo?.PropertyType == null) return null;

        var instanceParameter = Expression.Parameter(typeof(TModel), "model");
        var valueParameter = Expression.Parameter(propertyInfo.PropertyType, "value");
        var propertyAccess = Expression.Property(instanceParameter, propertyInfo);
        var assignExpression = Expression.Assign(propertyAccess, valueParameter);
        var getter = Expression.Lambda<Func<TModel, TValue>>(propertyAccess, instanceParameter);
        var setter = Expression.Lambda<Action<TModel, TValue>>(assignExpression, instanceParameter, valueParameter);
        var propertyAccessor = new PropertyAccessor<TModel, TValue>(getter, setter);
        return propertyAccessor;
    }
}

public class PropertyMapping
{
    public string? PropertyName { get; set; }
    public string? ColumnName { get; set; }
    public string? ColumnType { get; set; }
    public bool? ColumnRequired { get; set; }
    public Func<object?, string?>? ToSchemaConversion { get; set; }
    public Func<object?, object?>? FromSchemaConversion { get; set; }
    public IPropertyAccessor? PropertyAccessor { get; set; }
}
