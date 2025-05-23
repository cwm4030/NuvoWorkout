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
        where TModel : new()
    {
        List<TModel> models = [];
        var modelMap = GetModelMapFromModelName(typeof(TModel).Name);
        var columnNameIndexes = GetColumnNameIndexes(reader, modelMap?.TableColumns ?? []);
        while (await reader.ReadAsync())
        {
            var model = new TModel();
            foreach (var (columnName, index) in columnNameIndexes)
            {
                if (await reader.IsDBNullAsync(index)) continue;
                var propertyMap = modelMap?.GetPropertyMapFromColumnName(columnName);
                if (propertyMap == null) continue;
                propertyMap?.SetValue(model, reader[columnName]);
            }
            models.Add(model);
        }
        return models;
    }

    private static List<(string, int)> GetColumnNameIndexes(DbDataReader reader, HashSet<string> mappedColumnNames)
    {
        List<(string, int)> columnNameIndexes = [];
        for (var i = 0; i < reader.FieldCount; i++)
            if (mappedColumnNames.Contains(reader.GetName(i)))
                columnNameIndexes.Add((reader.GetName(i), i));
        
        return columnNameIndexes;
    }
}
