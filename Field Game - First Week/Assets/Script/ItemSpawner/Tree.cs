using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : ItemSpawner
{
    [SerializeField] GameObject fruitPrefab = null;

    [SerializeField] float SPAWNTIMEFRUIT = 10.0f; //사과 다시 떨어지는 시간
    private float stepTime;

    private bool isAvailableSpawn; //사과 다시 열였는지

    private void Start()
    {
        itemPrefab = fruitPrefab;
        stepTime = 0.0f;
        isAvailableSpawn = true;
    }

    private void Update()
    {
        if (!isAvailableSpawn)
            stepTime += Time.deltaTime;

        if (stepTime > SPAWNTIMEFRUIT)
        {
            isAvailableSpawn = true;
        }
    }

    public void hit()
    {
        if (isAvailableSpawn)
        {
            spawnItem();
            isAvailableSpawn = false;
            stepTime = 0.0f;
        }

    }
}