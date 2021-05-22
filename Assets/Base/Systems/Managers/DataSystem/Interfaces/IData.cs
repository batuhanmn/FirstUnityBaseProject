using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IData
{
    T GetData<T>(string Key);
    void UpdateData<T>(string Key, T Value);
    Type GetDataType(); 
}
