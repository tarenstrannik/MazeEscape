using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyColliding : CollisionController
{
    // Start is called before the first frame update

    private EnemyController m_enemyController;

 

    [SerializeField] private float m_damageDelay = 1f;
    private float m_curDamageTimer = 0f;
    protected override void Awake()
    {
        base.Awake();
        m_enemyController=GetComponent<EnemyController>();

    }


    
    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if(collision.gameObject.GetComponent<IDamageble>()!=null && collision.gameObject.GetComponent<ITargetForEnemy>() != null && collision.gameObject.GetComponent<ICanDie>()!=null && !collision.gameObject.GetComponent<ICanDie>().IsDead)
        {
            GiveDamage(collision.gameObject.GetComponent<IDamageble>());
        }
    }
    private void GiveDamage(IDamageble damageTarget)
    {
        //Bite player
        damageTarget.ReceiveDamage(m_enemyController.EnemyDamage);

        m_curDamageTimer = m_damageDelay;
    }
    private void OnCollisionStay(Collision collision)
    {

        if (collision.gameObject.GetComponent<IDamageble>() != null && collision.gameObject.GetComponent<ITargetForEnemy>() != null && collision.gameObject.GetComponent<ICanDie>() != null && !collision.gameObject.GetComponent<ICanDie>().IsDead)
        {
            m_curDamageTimer -= Time.deltaTime;
            if (m_curDamageTimer <= 0)
            {
                //Bite player
                GiveDamage(collision.gameObject.GetComponent<IDamageble>());
            }

        }
    }


}
