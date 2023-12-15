using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliding : CollisionController
{
    // Start is called before the first frame update

    private EnemyController m_enemyController;

    private float m_curDamageTimer = 0f;

    private Coroutine m_damageWaitingCoroutine = null;
    protected override void Awake()
    {
        base.Awake();
        m_enemyController=GetComponent<EnemyController>();

    }


    private void GiveDamage(Collider other)
    {
        if (other.gameObject.GetComponent<IDamageble>() != null && other.gameObject.GetComponent<ITargetForEnemy>() != null && other.gameObject.GetComponent<ICanDie>() != null && !other.gameObject.GetComponent<ICanDie>().IsDead)
        {
            if (m_damageWaitingCoroutine == null)
            {
                other.gameObject.GetComponent<IDamageble>().ReceiveDamage(m_enemyController.EnemyDamage);
                m_damageWaitingCoroutine = StartCoroutine(DamageDelay());

            }

        }
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
       
        base.OnTriggerEnter(other);

        GiveDamage(other);
        
    }
    
    protected override void OnTriggerStay(Collider other)
    {
        
        
        base.OnTriggerStay(other);
        GiveDamage(other);
    }
   
    private IEnumerator DamageDelay()
    {
        m_curDamageTimer = m_enemyController.EnemyDamageDelay;
        while (m_curDamageTimer>=0)
        {
            m_curDamageTimer -= Time.deltaTime;
            yield return null;
        }
        
        m_damageWaitingCoroutine = null;
    }

}
