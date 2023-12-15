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
        set
        {
            m_enemyDamage = value;
        }
    }
    [SerializeField] private float m_enemyDamageDelay = 1f;
    public float EnemyDamageDelay
    {
        get
        {
            return m_enemyDamageDelay;
        }
        set
        {
            m_enemyDamageDelay = value;
        }
    }
    [SerializeField] private GameObject m_target;
    public GameObject Target
    {
        get
        {
            return m_target;
        }
        set
        {
            m_target = value;
        }
    }
    [SerializeField] private LayerMask m_whatToIncludeInLinecast;
    [SerializeField] private float m_visibilityDistance = 5f;
    public float VisibilityDistance
    {
        set
        {
            m_visibilityDistance = value;
        }
    }
    [SerializeField] private float m_visibilityAngle= 45f;

    public float VisibilityAngle
    {
        set
        {
            m_visibilityAngle = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        
    }
    protected override void Start()
    {
        base.Start();
        
    }
    protected override void Update()
    {
        CheckIfCanSeePlayer();
        base.Update();
    }

    private void CheckIfCanSeePlayer()
    {
       if (!m_target.GetComponent<PlayerController>().IsDead)
       {
        if(Vector3.Distance(transform.position, m_target.transform.position)<=m_visibilityDistance)
            {
                
                if(Vector3.Angle(transform.forward.normalized, (m_target.transform.position - transform.position).normalized) <= m_visibilityAngle)
                {
                    if (Physics.Linecast(transform.position, m_target.transform.position, out var hitInfo, m_whatToIncludeInLinecast))
                    {
                        if(hitInfo.collider.gameObject== m_target)
                        {
                            m_characterMove.GoToPoint(m_target.transform.position);
                        }
                    }
                }
            }
            
       };
    }

}
