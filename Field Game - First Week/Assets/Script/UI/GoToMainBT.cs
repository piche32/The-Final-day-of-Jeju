
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainBT : MonoBehaviour
{
    public void GoToMainScene()
    {
        SceneManager.LoadScene("Title(Test)");
    }
}
