using System.Linq.Expressions;

namespace NuvoDb;

public class NuvoPropertyMap<TModel, TValue> : INuvoPropertyMap
{
    public string ModelName { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public string ColumnName { get; set; } = string.Empty;
    public string ColumnType { get; set; } = string.Empty;
    public bool ColumnRequired { get; set; }
    private Func<TModel, TValue>? _getter;
    private Action<TModel, TValue>? _setter;

    public void SetPropertyGetter(Expression<Func<TModel, TValue>> getter)
    {
        _getter = getter.Compile();
    }

    public void SetPropertySetter(Expression<Action<TModel, TValue>> setter)
    {
        _setter = setter.Compile();
    }

    public string GetModelName()
    {
        return ModelName;
    }

    public string GetPropertyName()
    {
        return PropertyName;
    }

    public string GetColumnName()
    {
        return ColumnName;
    }

    public string GetColumnType()
    {
        return ColumnType;
    }

    public bool GetColumnRequired()
    {
        return ColumnRequired;
    }

    public object? GetValue(object? model)
    {
        if (model == null || _getter == null) return null;
        return _getter((TModel)model);
    }

    public void SetValue(object? model, object? value)
    {
        if (model == null || value == null || _setter == null) return;
        _setter((TModel)model, (TValue)value);
    }
}
