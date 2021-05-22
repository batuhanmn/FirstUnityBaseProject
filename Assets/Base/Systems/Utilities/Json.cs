using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Json : Singleton<Json>
{
    public static string ConvertToJson<T>(T value)
    {
        return JsonUtility.ToJson(value);
    }

    public static T ConvertFromJson<T>(string value, Type type)
    {
        return (T) JsonUtility.FromJson(value, type);
    }

}
