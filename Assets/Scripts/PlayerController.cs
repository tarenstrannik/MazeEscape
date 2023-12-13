using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows;
using UnityEngine.InputSystem;
[RequireComponent(typeof(AudioSource))]

public class PlayerController : CharacterController, ITargetForEnemy
{
    

    private AudioSource m_playerAudioSource;

    [SerializeField] private AudioClip m_damageAudio;
    [SerializeField] private AudioClip m_deathAudio;
    [SerializeField] private Canvas uiDisplayObject;

    

    
    protected override void Awake()
    {
        base.Awake();
       
    }
    protected override void Start()
    {
        base.Start();
        m_playerAudioSource = GetComponent<AudioSource>();

       
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
       

    }


    private void OnMove(InputValue value)
    {
        if(!IsDead)
        {
            m_characterMove.SendMessage("CharacterMovement", value.Get<Vector2>());
        }
    }
    private void OnMove(Vector2 direction)
    {
        if (!IsDead)
        {
            m_characterMove.SendMessage("CharacterMovement", direction);
        }
    }



    public override void ReceiveDamage(float damage)
    {
        float prevHealth = PersonHealth;
        base.ReceiveDamage(damage);
        if (Mathf.Ceil(prevHealth) != Mathf.Ceil(PersonHealth))
            //uiDisplay.SendMessage("UpdateHealth", Mathf.Ceil(PersonHealth));
        if (!IsDead)
        {
            if (damage > 0)
            {
                if(m_damageAudio!=null) m_playerAudioSource.PlayOneShot(m_damageAudio);
            }
            else
            {
                
            }
            
        }
        else if (IsDead )
        {
            if(m_deathAudio!=null) m_playerAudioSource.PlayOneShot(m_deathAudio);

        }
    }






}
