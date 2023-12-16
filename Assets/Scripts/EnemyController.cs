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

    private Coroutine m_damageWaitingCoroutine = null;
    private float m_curDamageTimer = 0f;

    private EnemyRaycasting m_enemyRaycasting;
    
   
    protected override void Awake()
    {
        base.Awake();
        m_enemyRaycasting = GetComponent<EnemyRaycasting>();


    }
    protected override void Start()
    {
        base.Start();
        
    }
    protected override void Update()
    {
        var target = m_enemyRaycasting.CheckIfCanSeePlayer();
        if (target!=null)
            m_characterMove.GoToPoint(target.transform.position);
        base.Update();
    }

    public void GiveDamage(GameObject target)
    {
        if (target.GetComponent<IDamageble>() != null && target.GetComponent<ITargetForEnemy>() != null && target.GetComponent<ICanDie>() != null && !target.GetComponent<ICanDie>().IsDead)
        {
            if (m_damageWaitingCoroutine == null)
            {
                target.GetComponent<IDamageble>().ReceiveDamage(m_enemyDamage);
                m_damageWaitingCoroutine = StartCoroutine(DamageDelay());

            }

        }

    }

    private IEnumerator DamageDelay()
    {
        m_curDamageTimer = m_enemyDamageDelay;
        while (m_curDamageTimer >= 0)
        {
            m_curDamageTimer -= Time.deltaTime;
            yield return null;
        }

        m_damageWaitingCoroutine = null;
    }

}
