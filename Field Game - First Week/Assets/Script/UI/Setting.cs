using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    Slider BGMSlider = null;
    Slider SFXSlider = null;

    [SerializeField]SoundController soundCtrl;

    public void Start()
    {
        BGMSlider = this.transform.Find("BGM").GetComponentInChildren <Slider> ();
        SFXSlider = this.transform.Find("SFX").GetComponentInChildren <Slider> ();

        BGMSlider.value = DataController.Instance.gameData.BGMVolume;
        SFXSlider.value = DataController.Instance.gameData.SFXVolume;
    }

    public void SettingBGM()
    {
        DataController.Instance.gameData.BGMVolume = BGMSlider.value;
        soundCtrl.SetBGMVolume();
        DataController.Instance.SaveGameData();
    }

    public void SettingSFX()
    {
        DataController.Instance.gameData.SFXVolume = SFXSlider.value;
        DataController.Instance.SaveGameData();
        soundCtrl.SetSFXVolume();
    }

}
