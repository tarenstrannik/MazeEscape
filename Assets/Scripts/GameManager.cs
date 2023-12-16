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

    [SerializeField] private Transform m_playerParentGroup;

    //enemy
    [SerializeField] private GameObject m_enemyPrefab;
    [SerializeField] private Transform m_enemyParentGroup;

    //level
    [SerializeField] GameObject m_cellPrefab;
    [SerializeField] GameObject m_finishPrefab;
    [SerializeField] private NavMeshSurface m_navMeshSurface;
    [SerializeField] private Transform m_levelParentGroup;

    private CharacterGenerator m_CharacterGenerator;
    private LevelGenerator m_LevelGenerator;

    [SerializeField] private GameParameters m_gameParams;
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
        m_LevelGenerator.GenerateLevel(m_cellPrefab, m_finishPrefab, m_navMeshSurface, m_gameParams.m_xLabyrinthSize, m_gameParams.m_zLabyrinthSize, m_levelParentGroup);
        while(!m_LevelGenerator.IsGenerationFinished)
        {
            yield return null;
        }
        m_LevelGenerator.GetFinish().m_gameWin.AddListener(GameWin);

        var player = GeneratePlayer();

        GenerateEnemies(player);

        StartGame();

    }

    private PlayerController GeneratePlayer()
    {
        //generating player
        var playerCell = m_LevelGenerator.GetFreeRandomCell();

        var player = (PlayerController)m_CharacterGenerator.GenerateCharacter(m_playerPrefab, playerCell.transform.position, m_playerUIPrefab, m_gameParams.m_playerUIDelta, m_playerParentGroup);

        m_CharacterGenerator.ConfigureCharacter(player, m_gameParams.m_playerHealth, m_gameParams.m_playerSpeed, m_gameParams.m_playerRotationSpeed);
        SubscribeToPlayer(player);

        return player;
        
    }

    private void GenerateEnemies(PlayerController player)
    {
        //generating enemy

        

        var enemyCount = Random.Range(m_gameParams.m_minEnemyCount, m_gameParams.m_maxEnemyCount + 1);
        for (var i = 0; i < enemyCount; i++)
        {
            var enemyCell = m_LevelGenerator.GetFreeRandomCell();
            var enemy = (EnemyController)m_CharacterGenerator.GenerateCharacter(m_enemyPrefab, enemyCell.transform.position, null, Vector3.zero, m_enemyParentGroup);

            m_CharacterGenerator.ConfigureCharacter(enemy, m_gameParams.m_enemyHealth, m_gameParams.m_enemySpeed, m_gameParams.m_enemyRotationSpeed);

            List<Vector3> enemyWaypoints = new List<Vector3>();
            var randomWaypointsNumber = Random.Range(m_gameParams.m_enemyWaypointsMinNumber, m_gameParams.m_enemyWaypointsMaxNumber + 1);
            for (var j = 0; j < randomWaypointsNumber; j++)
            {
                enemyWaypoints.Add(m_LevelGenerator.GetWayPointInCell(enemyCell, m_enemyPrefab.transform.localScale.x / 2));

            }

            m_CharacterGenerator.ConfigureEnemy(enemy, player, m_gameParams.m_enemyVisibilityDistance, m_gameParams.m_enemyVisibilityAngle, m_gameParams.m_enemyDeltaAngle, m_gameParams.m_enemyDrawAndDamageDistance, m_gameParams.m_enemyDrawAndDamageAngle, m_gameParams.m_enemyDamage, m_gameParams.m_enemyDamageDelay, enemyWaypoints);
            
        }
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
