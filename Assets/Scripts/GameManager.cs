using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameUI m_gameUI;
    private bool m_isGameActive = true;

    // player
    [SerializeField] private GameObject m_playerPrefab;
    [SerializeField] private GameObject m_playerUIPrefab;

    [SerializeField] private Vector3 m_playerUIDelta = new Vector3(0, 1, 1);

    private float m_playerHealth=10f;
    private float m_playerSpeed=10f;
    private float m_playerRotationSpeed = 20f;

    //enemy
    [SerializeField] private GameObject m_enemyPrefab;

    private float m_enemyHealth = 10f;
    private float m_enemySpeed = 5f;
    private float m_enemyRotationSpeed = 120f;

    private float m_enemyVisibilityDistance = 10f;
    private float m_enemyVisibilityAngle=45f;

    private float m_enemyDamage = 1f;
    private float m_enemyDamageDelay = 1f;

    private int m_minEnemyNumber = 1;
    private int m_maxEnemyNumber = 5;
    private int m_enemyWaypointsMaxNumber=4;

    //level
    [SerializeField] GameObject m_cellPrefab;
    [SerializeField] GameObject m_finishPrefab;
    [SerializeField] private NavMeshSurface m_navMeshSurface; 

    [SerializeField] private Finish m_finish;

    private CharacterGenerator m_CharacterGenerator;
    private LevelGenerator m_LevelGenerator;
    private void Awake()
    {
        Time.timeScale = 0;
        //generating level
        m_LevelGenerator = new LevelGenerator();

        m_CharacterGenerator = new CharacterGenerator();

        StartCoroutine(GenerationCoroutine());


    }


    private IEnumerator GenerationCoroutine()
    {
        m_LevelGenerator.GenerateLevel(m_cellPrefab, m_finishPrefab, m_navMeshSurface,this);
        while(!m_LevelGenerator.IsGenerationFinished)
        {
            yield return null;
        }

        //generating player
        var playerCell = m_LevelGenerator.GetFreeRandomCell();

        var player = (PlayerController)m_CharacterGenerator.GenerateCharacter(m_playerPrefab, playerCell.transform.position, m_playerUIPrefab, m_playerUIDelta);

        m_CharacterGenerator.ConfigureCharacter(player, m_playerHealth, m_playerSpeed, m_playerRotationSpeed);
        SubscribeToPlayer(player);

        //generating enemy
        var enemyCount = Random.Range(m_minEnemyNumber, m_maxEnemyNumber + 1);
        for (var i = 0; i < enemyCount; i++)
        {
            var enemyCell = m_LevelGenerator.GetFreeRandomCell();
            var enemy = (EnemyController)m_CharacterGenerator.GenerateCharacter(m_enemyPrefab, enemyCell.transform.position, null, Vector3.zero);
            
            m_CharacterGenerator.ConfigureCharacter(enemy, m_enemyHealth, m_enemySpeed, m_enemyRotationSpeed);

            List<Vector3> enemyWaypoints = new List<Vector3>();
            var randomWaypointsNumber = Random.Range(2, m_enemyWaypointsMaxNumber + 1);
           for(var j=0;j< randomWaypointsNumber; j++)
            {
                enemyWaypoints.Add(m_LevelGenerator.GetWayPointInCell(enemyCell, m_enemyPrefab.transform.localScale.x/2));
               
            }

            m_CharacterGenerator.ConfigureEnemy(enemy, player, m_enemyVisibilityDistance, m_enemyVisibilityAngle, m_enemyDamage, m_enemyDamageDelay, enemyWaypoints);
            //for enemies also need to add list of waypoints give link to level generator to ask for points
        }

        StartGame();

    }
    private void StartGame()
    {
        Time.timeScale = 1;
    }

    public void SubscribeToPlayer(CharacterController character)
    {
        character.m_death.AddListener(GameOver);
        character.m_cancelEvent.AddListener(EscapeButtonAction);


    }


    public void GameOver()
    {
        m_isGameActive = false;
        Time.timeScale = 0;
        m_gameUI.ShowGameOverScreen();
    }
    public void GameWin()
    {
        m_isGameActive = false;
        Time.timeScale = 0;
        m_gameUI.ShowGameWinScreen();
    }

    private void EscapeButtonAction()
    {
        if (m_isGameActive)
        {
            TogglePauseGame();
        }
        else
        {
            ExitGame();
        }
    }
    public void TogglePauseGame()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0; ;
        m_gameUI.TogglePauseScreen();
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
