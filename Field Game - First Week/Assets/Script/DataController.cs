using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataController : Singleton<DataController>
{
    public string GameDataFileName = ".json"; //이름 변경 절대 X

    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            if(_gameData == null) {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath))
        {
            Debug.Log("불러오기 성공!");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
        }
        else
        {
            MakeNewData();
        }
    }

    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
        Debug.Log("저장 완료");
    }

    public void MakeNewData()
    {
        Debug.Log("새로운 파일 생성");
        _gameData = new GameData();
        
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
