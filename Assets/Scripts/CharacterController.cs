using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CharacterController : MonoBehaviour,IDamageble,ICanDie
{
   

    [SerializeField] protected float m_maxPersonHealth = 10f;

    private float m_personHealth;
    public float PersonHealth { 
        get
        {
            return m_personHealth;
        }
        protected set
        {
            m_personHealth = Mathf.Clamp(value, 0f, m_maxPersonHealth); ;
        } 
    }

    protected bool m_isDead = false;
    public bool IsDead { 
        get
        {
            return m_isDead;
        }
    } 

    protected MoveController m_characterMove;

    // Start is called before the first frame update
    public virtual void ReceiveDamage(float damage)
    {
        m_personHealth -= damage;
        if (m_personHealth <= 0 && !m_isDead)
        {
            m_isDead = true;

        }
    }
    protected virtual void Awake()
    {
        m_personHealth = m_maxPersonHealth;
        m_characterMove = GetComponent<MoveController>();

    }
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
       
    }

}
