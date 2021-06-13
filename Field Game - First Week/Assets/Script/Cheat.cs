using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheat : MonoBehaviour
{
    [SerializeField]GameStatus gameStatus = null;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            gameStatus.addCapacity(GameStatus.MAX_SHIP_CAPACITY - 1.0f);
        }
    }
}
