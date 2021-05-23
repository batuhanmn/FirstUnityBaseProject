using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class UIPanel
{
    public string ID;
    public GameObject Panel;
}

public class UIManager : Singleton<UIManager>
{
    public Dictionary<string, GameObject> UICreatedDictionary = new Dictionary<string, GameObject>();
    private Canvas canvas;

    public void ShowPanel(string PanelID, params object[] Variables)
    {
        if (!UICreatedDictionary.ContainsKey(PanelID) || UICreatedDictionary[PanelID] == null)
        {
            GameObject panel = Instantiate(PanelLoad(PanelID).gameObject,canvas.transform);
            UICreatedDictionary[PanelID] = panel;
        }

        UICreatedDictionary[PanelID].transform.SetParent(canvas.transform);
        UICreatedDictionary[PanelID].GetComponent<PanelBase>().ShowPanel(Variables);
        EventSystem.TriggerEvent("OnShowPanel",PanelID);
    }

    public void HidePanel(string PanelID)
    {
        UICreatedDictionary[PanelID].GetComponent<PanelBase>().HidePanel();
        UICreatedDictionary[PanelID].transform.SetParent(transform);
        EventSystem.TriggerEvent("OnHidePanel", PanelID);
    }

    private PanelBase PanelLoad(string PanelID)
    {
        return Resources.Load<PanelBase>("UISystemData/Panels/"+PanelID);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += (a, b) =>
        {
            canvas = GameObject.FindGameObjectWithTag("MainCanvas").GetComponent<Canvas>();
        };
    }

}
