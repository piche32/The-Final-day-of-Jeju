using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSetting : MonoBehaviour
{

    [SerializeField] GameObject setting = null;

    [SerializeField] Slider BGMSlider = null;
    [SerializeField] Slider SFXSlider = null;

    TitleSoundCtrl soundCtrl = null;

    public void Start()
    {
        if (setting == null)
            Debug.LogError("There's no Setting.");

        soundCtrl = GameObject.Find("Sound").GetComponent<TitleSoundCtrl>();
        if (soundCtrl == null)
            Debug.LogError("There's no BGM Audio Source.");

        if (BGMSlider == null)
            Debug.LogError("There's no BGM Slider.");

        if (SFXSlider == null)
            Debug.LogError("There's no SFX Slider.");


        BGMSlider.value = DataController.Instance.gameData.BGMVolume;
        SFXSlider.value = DataController.Instance.gameData.SFXVolume;
    }

    public void SettingBGM()
    {
        DataController.Instance.gameData.BGMVolume = BGMSlider.value;
        DataController.Instance.SaveGameData();
        soundCtrl.SetBGMVolume();
    }

    public void SettingSFX()
    {
        DataController.Instance.gameData.SFXVolume = SFXSlider.value;
        DataController.Instance.SaveGameData();
    }

    public void ShowSetting()
    {
        setting.SetActive(true);
    }

    public void QuitSetting()
    {
        setting.SetActive(false);
    }
}
