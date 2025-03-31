using System.Linq.Expressions;

namespace NuvoWorkoutPersistance;

public interface IPropertyAccessor
{
    public object? Cast(object? value);
    object? GetValue(object model);
    void SetValue(object? model, object? value);
}

public class PropertyAccessor<TModel, TValue>(Expression<Func<TModel, TValue>> getter, Expression<Action<TModel, TValue>> setter) : IPropertyAccessor
{
    private readonly Func<TModel, TValue> _getter = getter.Compile();
    private readonly Action<TModel, TValue> _setter = setter.Compile();

    public object? Cast(object? value)
    {
        if (value == null) return null;
        return (TValue)value;
    }

    public object? GetValue(object model)
    {
        return _getter((TModel)model);
    }

    public void SetValue(object? model, object? value)
    {
        if (model != null && value != null)
            _setter((TModel)model, (TValue)value);
    }
}
