using System.Collections.Concurrent;
using System.Data.Common;

namespace NuvoDb;

public class NuvoDatabaseMap(string databaseName)
{
    public string DatabaseName { get; set; } = databaseName;
    private static readonly string s_model = "model ";
    private static readonly string s_schema = "schema ";
    private readonly ConcurrentDictionary<string, NuvoModelMap> _modelMaps = [];

    public void DefineModelMap(NuvoModelMap modelMap)
    {
        AddModelMap(modelMap);
    }

    public void AddModelMap(NuvoModelMap modelMap)
    {
        _modelMaps.TryAdd(s_model + modelMap.ModelName, modelMap);
        _modelMaps.TryAdd(s_schema + modelMap.SchemaName, modelMap);
    }

    public NuvoModelMap? GetModelMapFromModelName(string modelName)
    {
        return _modelMaps.TryGetValue(s_model + modelName, out var mm) ? mm : null;
    }

    public NuvoModelMap? GetModelMapFromSchemaName(string schemaName)
    {
        return _modelMaps.TryGetValue(s_schema + schemaName, out var mm) ? mm : null;
    }

    public async Task<List<TModel>> MapModels<TModel>(DbDataReader reader)
    {
        List<TModel> models = [];
        var columnNameIndexes = GetColumnNameIndexes(reader);
        var modelName = typeof(TModel).Name;
        var modelMapping = GetModelMapFromModelName(modelName);
        while (await reader.ReadAsync())
        {
            var model = Activator.CreateInstance<TModel>();
            foreach (var (columnName, index) in columnNameIndexes)
            {
                if (await reader.IsDBNullAsync(index)) continue;
                var propertyMapping = modelMapping?.GetPropertyMapFromColumnName(columnName);
                if (propertyMapping == null) continue;

                var value = propertyMapping?.Cast(reader[columnName]);
                propertyMapping?.SetValue(model, value);
            }
            models.Add(model);
        }
        return models;
    }

    private static List<(string, int)> GetColumnNameIndexes(DbDataReader reader)
    {
        List<(string, int)> columnNameIndexes = [];
        for (var i = 0; i < reader.FieldCount; i++)
            columnNameIndexes.Add((reader.GetName(i), i));
        
        return columnNameIndexes;
    }
}
