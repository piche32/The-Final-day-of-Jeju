using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameObject HowTo = null;
    [SerializeField] GameObject SettingUI = null;
    public void OnEnable()
    {
        HowTo.SetActive(true);
        SettingUI.SetActive(false);
    }

    public void ShowSetting()
    {
        SettingUI.SetActive(true);
        HowTo.SetActive(false);
    }

    public void ShowHowTo()
    {
        HowTo.SetActive(true);
        SettingUI.SetActive(false);
    }

}
