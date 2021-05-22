using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DataManager : Singleton<DataManager>
{
    private const string LOCATION_KEY = "DataManager.Data";
    private IData data;
    private bool isDirty;
    private bool isLoaded;

    #region DataFunctions

    public void SaveData<T>(string Key, T Value)
    {
        data.UpdateData(Key, Value);
        isDirty = true;
    }
    public T GetData<T>(string Key)
    {
        if(!isLoaded) LoadData();
        return data.GetData<T>(Key);
    }

    public void DeleteData()
    {
        if (CheckHasKey(LOCATION_KEY)) return;
        PlayerPrefs.DeleteKey(LOCATION_KEY);
        isDirty = true;
    }
    #endregion

    #region InitFunctioms
    //DataManager.Instance.SetData(new GameData())
    public void SetData(IData idata)
    {
        data = idata;
    }
    #endregion

    private void LoadData()
    {
        data = Json.ConvertFromJson<IData>(PlayerPrefs.GetString(LOCATION_KEY), data.GetDataType());
        isLoaded = true;
    }

    private bool CheckHasKey(string Key)
    {
        return PlayerPrefs.HasKey(Key);
    }

    private void LateUpdate()
    {
        if (!isDirty) return;
        PlayerPrefs.SetString(LOCATION_KEY, Json.ConvertToJson(data));
        PlayerPrefs.Save();
        isDirty = false;
    }

}
