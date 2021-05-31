using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSoundCtrl : MonoBehaviour
{
    AudioSource BGM;

    // Start is called before the first frame update
    void Start()
    {
        BGM = GetComponent<AudioSource>();
        if (BGM == null)
            Debug.LogError("There's no BGM Audio Source.");
        SetBGMVolume();

    }

    public void SetBGMVolume()
    {
        BGM.volume = DataController.Instance.gameData.BGMVolume;

    }

}
