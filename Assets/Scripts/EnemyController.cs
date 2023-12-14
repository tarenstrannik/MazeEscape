using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
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
    [SerializeField] private LayerMask m_whatToIncludeInLinecast;
    [SerializeField] private float m_visibilityDistance = 5f;
    [SerializeField] private float m_visibilityAngle= 90f;

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
        CheckIfCanSeePlayer();
        base.Update();
    }

    private void CheckIfCanSeePlayer()
    {
       if (!m_player.GetComponent<PlayerController>().IsDead)
       {
        if(Vector3.Distance(transform.position, m_player.transform.position)<=m_visibilityDistance)
            {
                
                if(Vector3.Angle(transform.forward.normalized, (m_player.transform.position - transform.position).normalized) <= m_visibilityAngle)
                {
                    if (Physics.Linecast(transform.position, m_player.transform.position, out var hitInfo, m_whatToIncludeInLinecast))
                    {
                        if(hitInfo.collider.gameObject== m_player)
                        {
                            m_characterMove.GoToPoint(Player.transform.position);
                        }
                    }
                }
            }
            
       };
    }

}
