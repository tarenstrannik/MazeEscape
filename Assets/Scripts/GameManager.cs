using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameUI m_gameUI;

    [SerializeField] private GameObject m_playerPrefab;
    [SerializeField] private GameObject m_playerUIPrefab;

    [SerializeField] private Vector3 m_playerUIDelta = new Vector3(0, 1, 1);

    private float m_playerHealth=10f;
    private float m_playerSpeed=5f;
    private float m_playerRotationSpeed = 20f;

    private float m_enemyHealth = 10f;
    private float m_enemySpeed = 3f;
    private float m_enemyRotationSpeed = 120f;

    private float m_enemyVisibilityDistance = 5f;
    private float m_enemyVisibilityAngle=45f;

    private float m_enemyDamage = 1f;
    private float m_enemyDamageDelay = 1f;

    [SerializeField] private GameObject m_enemyPrefab;

    [SerializeField] private Finish m_finish;

    private CharacterGenerator m_CharacterGenerator;

    private void Awake()
    {
        m_CharacterGenerator = new CharacterGenerator();

        //generating player
        var player = (PlayerController)m_CharacterGenerator.GenerateCharacter(m_playerPrefab, new Vector3(5, 1, 5), m_playerUIPrefab, m_playerUIDelta);
        m_CharacterGenerator.ConfigureCharacter(player, m_playerHealth, m_playerSpeed, m_playerRotationSpeed);
        m_CharacterGenerator.ConfigurePlayer(player, this);

        //generating enemy
        var enemy=(EnemyController)m_CharacterGenerator.GenerateCharacter(m_enemyPrefab, new Vector3(-4, 1, -4), null, Vector3.zero);
        m_CharacterGenerator.ConfigureCharacter(enemy, m_enemyHealth, m_enemySpeed, m_enemyRotationSpeed);
        m_CharacterGenerator.ConfigureEnemy(enemy, player, m_enemyVisibilityDistance, m_enemyVisibilityAngle, m_enemyDamage, m_enemyDamageDelay);
        //for enemies also need to add list of waypoints
    }

   



    public void GameOver()
    {
        m_gameUI.ShowGameOverScreen();
    }
    public void GameWin()
    {
        m_gameUI.ShowGameWinScreen();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();

#else
#if UNITY_WEBGL
        SceneManager.LoadScene(0);
#else
        Application.Quit();
#endif
#endif
    }
}
