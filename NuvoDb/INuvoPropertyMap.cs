namespace NuvoDb;

public interface INuvoPropertyMap
{
    public string GetModelName();
    public string GetPropertyName();
    public string GetColumnName();
    public string GetColumnType();
    public bool GetColumnRequired();
    object? GetValue(object model);
    void SetValue(object? model, object? value);
}
