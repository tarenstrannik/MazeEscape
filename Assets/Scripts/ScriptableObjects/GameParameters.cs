using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Parameters", menuName = "Game Parameters")]
public class GameParameters : ScriptableObject
{

    // player
    [Tooltip("Positioning healthbar according to player")]
    public Vector3 m_playerUIDelta = new Vector3(0, 1, 1);

    public float m_playerHealth = 10f;
    public float m_playerSpeed = 10f;
    public float m_playerRotationSpeed = 20f;

    //enemy
  

    public float m_enemyHealth = 10f;
    public float m_enemySpeed = 5f;
    public float m_enemyRotationSpeed = 120f;
    [Tooltip("How far enemy can see player to start follow")]
    public float m_enemyVisibilityDistance = 10f;
    [Tooltip("To left and right from forward")]
    public float m_enemyVisibilityAngle = 45f;

    [Tooltip("If true looking area is triangle. If false - sector of the sphere")]
    public bool m_isFrontViewLineFlat = true;

    [Tooltip("How far enemy can damage player and how far to draw damage zone")]
    public float m_enemyDrawAndDamageDistance = 5f;
    [Tooltip("To left and right from forward")]
    public float m_enemyDrawAndDamageAngle = 45f;
    [Tooltip("If true damage area is triangle. If false - sector of the sphere")]
    public bool m_isFrontDamageLineFlat = true;

    [Tooltip("Step of Raycasting for scanning damage and view area")]
    public float m_enemyDeltaAngle = 1f;

    public float m_enemyDamage = 1f;
    [Tooltip("Delay between player being damaged")]
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
