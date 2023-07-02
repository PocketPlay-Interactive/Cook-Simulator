using System;
using System.Collections.Generic;

public class GenericDictionary
{
    private Dictionary<string, object> _dict = new Dictionary<string, object>();

    public void Add<T>(string key, T value)
    {
        _dict.Add(key, value);
    }

    public bool Exists<T>(string key)
    {
        return _dict.ContainsKey(key);
    }

    public T GetValue<T>(string key)
    {
        return (T)_dict[key];
    }
}