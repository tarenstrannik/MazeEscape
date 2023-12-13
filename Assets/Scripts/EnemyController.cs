using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class EnemyController : CharacterController
{

    [SerializeField] private float m_enemyDamage = 1f;
    public float EnemyDamage { 
        get
        {
            return m_enemyDamage;
        } 
        private set
        {
            m_enemyDamage = value;
        }
    }

    private GameObject m_player;
    public GameObject Player
    {
        get
        {
            return m_player;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
    }
    protected override void Start()
    {
        base.Start();
        m_player = GameObject.Find("Player");

        //playerController = GetComponent<EnemyMove>().Player.GetComponent<PlayerController>();
    }
    protected override void Update()
    {
        if (!m_player.GetComponent<PlayerController>().IsDead)
        {
            m_characterMove.SendMessage("NavMeshMove",Player.transform.position);
            
            base.Update();
        }
    }


}
