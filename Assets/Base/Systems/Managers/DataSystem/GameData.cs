using System;
using System.Collections.Generic;

public class GameData : IData
{
    private readonly Dictionary<string, object> _dataCollection = new Dictionary<string, object>();
    public T GetData<T>(string key)
    {
        var data = _dataCollection.ContainsKey(key) ? Json.ConvertFromJson<T>(_dataCollection[key].ToString()) : default(T);
        return data;
    }

    public Type GetDataType()
    {
        return typeof(GameData);
    }

    public void UpdateData<T>(string key, T value)
    {
        if (_dataCollection.ContainsKey(key))
        {
            _dataCollection[key] = Json.ConvertToJson(value);
        }
        else
        {
            _dataCollection.Add(key, value);
        }
    }
}
