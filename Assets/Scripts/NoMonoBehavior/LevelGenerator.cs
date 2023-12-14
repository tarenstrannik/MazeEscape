using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator
{
    public void GenerateLevel(GameObject[] wallPrefabs, GameObject finishPrefab, GameManager gameManager)
    {
        GameObject.Instantiate(wallPrefabs[0], new Vector3(-2.79f, 0.36f, 0), wallPrefabs[0].transform.rotation);

        var finish=GameObject.Instantiate(finishPrefab,new Vector3(3.09f, finishPrefab.transform.position.y,2.74f), finishPrefab.transform.rotation).GetComponent<Finish>();

        finish.m_gameWin.AddListener(gameManager.GameWin);
    }
}
