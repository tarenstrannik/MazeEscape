using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] protected Transform m_characterToFollow;

    public Transform CharacterToFollow
    {
        set 
        {
            m_characterToFollow = value;
        }
    }

    [SerializeField] protected Vector3 m_deltaPosition;

    public Vector3 DelatPosition
    {
        set
        {
            m_deltaPosition = value;
        }
    }

    protected Slider m_characterHealth;
    protected virtual void Awake()
    {
        m_characterHealth = GetComponentInChildren<Slider>();
      
    }

    protected virtual void Start()
    {
        SetInitialHealthValueFromCharacter(m_characterToFollow.GetComponent<CharacterController>());
    }
    public void SetInitialHealthValueFromCharacter(CharacterController characterController)
    {
       
        if (m_characterHealth != null)
        {
            m_characterHealth.maxValue = characterController.MaxCharacterHealth;
            m_characterHealth.value = characterController.CharacterHealth;
        }
       
    }

    protected virtual void Update()
    {
        transform.position = m_characterToFollow.position+ m_deltaPosition;
    }

    public virtual void UpdateHealth(float health)
    {
        if(m_characterHealth!=null)
        {
            m_characterHealth.value = health;
        }
    }
}
