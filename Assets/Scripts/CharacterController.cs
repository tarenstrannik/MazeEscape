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
    [SerializeField] private AudioClip m_deathAudio;
    
    
    public UnityEvent m_death;
    
    public UnityEvent<float> m_damageRecieved;

    [SerializeField] protected float m_maxCharacterHealth = 10f;

    public float MaxCharacterHealth
    {
        get
        {
            return m_maxCharacterHealth;
        }
        set
        {
            m_maxCharacterHealth = value;
        }
    }

    private float m_characterHealth;
    public float CharacterHealth { 
        get
        {
            return m_characterHealth;
        }
        set
        {
            m_characterHealth = Mathf.Clamp(value, 0f, m_maxCharacterHealth); ;
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

    
    protected virtual void Awake()
    {
        
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

        CharacterHealth -= damage;
        m_damageRecieved.Invoke(m_characterHealth);
        if (!m_isDead)
        {
            if (CharacterHealth <= 0 )
            {
                m_death.Invoke();

            }
            else
            {
                if (m_damageAudio != null) m_characterAudioSource.PlayOneShot(m_damageAudio);
            }
        }

    }

    public virtual void Die()
    {
        m_isDead = true;
        
        if (m_deathAudio != null) m_characterAudioSource.PlayOneShot(m_deathAudio);
    }


}
