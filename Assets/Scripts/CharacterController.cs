using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]

public class CharacterController : MonoBehaviour,IDamageble,ICanDie
{
    private AudioSource m_characterAudioSource;

    [SerializeField] private AudioClip m_damageAudio;
    [SerializeField] private AudioClip m_healingAudio;
    [SerializeField] private AudioClip m_deathAudio;
    
    
    public UnityEvent m_death;
    public UnityEvent<float> m_damageRecieved;

    [SerializeField] protected float m_maxPersonHealth = 10f;

    public float MaxPersonHealth
    {
        get
        {
            return m_maxPersonHealth;
        }
        set
        {
            m_maxPersonHealth = value;
        }
    }

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
   
    protected virtual void Awake()
    {
        m_personHealth = m_maxPersonHealth;
        m_characterMove = GetComponent<MoveController>();
        m_characterAudioSource = GetComponent<AudioSource>();
        m_death.AddListener(Die);
        
        
    }
    protected virtual void Start()
    {
        
    }
    protected virtual void Update()
    {

    }
    public virtual void ReceiveDamage(float damage)
    {

        m_personHealth -= damage;
        m_damageRecieved.Invoke(m_personHealth);
        if (m_personHealth <= 0 && !m_isDead)
        {
            m_death.Invoke();
            
        }

        if (!IsDead)
        {
            if (damage > 0)
            {
                if (m_damageAudio != null) m_characterAudioSource.PlayOneShot(m_damageAudio);
            }
            else
            {
                if (m_healingAudio != null) m_characterAudioSource.PlayOneShot(m_healingAudio);
            }

        }
        

    }

    public virtual void Die()
    {
        m_isDead = true;
        //m_characterMove.CharacterMovement(Vector2.zero);
        if (m_deathAudio != null) m_characterAudioSource.PlayOneShot(m_deathAudio);
    }


}
