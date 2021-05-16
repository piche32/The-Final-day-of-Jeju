using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBT : MonoBehaviour
{
    [SerializeField] GameObject tutorial1 = null;
    [SerializeField] GameObject tutorial2 = null;
    // Start is called before the first frame update
  
    public void nextTutorial()
    {
        tutorial1.SetActive(false);
        tutorial2.SetActive(true);
    }
}
