using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Parameters", menuName = "Game Parameters")]
public class GameParameters : ScriptableObject
{
 
    // player
    
    public Vector3 m_playerUIDelta = new Vector3(0, 1, 1);

    public float m_playerHealth = 10f;
    public float m_playerSpeed = 10f;
    public float m_playerRotationSpeed = 20f;

    //enemy
  

    public float m_enemyHealth = 10f;
    public float m_enemySpeed = 5f;
    public float m_enemyRotationSpeed = 120f;

    public float m_enemyVisibilityDistance = 10f;
    [Tooltip("To left and right from forward")]
    public float m_enemyVisibilityAngle = 45f;

    public float m_enemyDamage = 1f;
    public float m_enemyDamageDelay = 1f;

    public int m_minEnemyCount = 1;
    public int m_maxEnemyCount = 5;
    
    [Range(2,100)]
    public int m_enemyWaypointsMinNumber = 2;
    [Range(2, 100)]
    public int m_enemyWaypointsMaxNumber = 4;

    //level
 
    [Range (2,9)]
    public int m_xLabyrinthSize = 9;
    [Range(2, 5)]
    public int m_zLabyrinthSize = 5;

    private void OnValidate()
    {
        if (m_enemyWaypointsMinNumber > m_enemyWaypointsMaxNumber) m_enemyWaypointsMaxNumber = m_enemyWaypointsMinNumber;


        var allCellsNumber = m_xLabyrinthSize * m_zLabyrinthSize;
        
        m_minEnemyCount = m_minEnemyCount < allCellsNumber - 2  ? m_minEnemyCount : allCellsNumber - 2 ;
        m_maxEnemyCount = m_maxEnemyCount < allCellsNumber - 2  ? m_maxEnemyCount : allCellsNumber - 2 ;


        if (m_minEnemyCount > m_maxEnemyCount) m_maxEnemyCount = m_minEnemyCount;

       

    }
}
