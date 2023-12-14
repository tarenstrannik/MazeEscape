using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class CharacterController : MonoBehaviour,IDamageble,ICanDie
{
    private AudioSource m_characterAudioSource;

    [SerializeField] private AudioClip m_damageAudio;
    [SerializeField] private AudioClip m_healingAudio;
    [SerializeField] private AudioClip m_deathAudio;
    [SerializeField] private CharacterUI m_characterUI;

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
   
    protected virtual void Awake()
    {
        m_personHealth = m_maxPersonHealth;
        m_characterMove = GetComponent<MoveController>();
        m_characterAudioSource = GetComponent<AudioSource>();

    }

    public virtual void ReceiveDamage(float damage)
    {
        

        m_personHealth -= damage;
        if (m_personHealth <= 0 && !m_isDead)
        {
            m_isDead = true;
        }

        if (m_characterUI!=null)
            m_characterUI.UpdateHealth(m_personHealth);
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
        else if (IsDead)
        {
            if (m_deathAudio != null) m_characterAudioSource.PlayOneShot(m_deathAudio);

        }

    }


    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
       
    }

}
