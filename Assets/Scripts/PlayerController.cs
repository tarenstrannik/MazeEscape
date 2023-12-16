using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Windows;
using UnityEngine.InputSystem;


public class PlayerController : CharacterController, ITargetForEnemy
{
       protected override void Awake()
    {
        base.Awake();
       
    }
    protected override void Start()
    {
        base.Start();
  
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
            m_characterMove.CharacterMovement(value.Get<Vector2>());
        }
    }
    private void OnMove(Vector2 direction)
    {
        
        if (!IsDead)
        {
            m_characterMove.CharacterMovement(direction);
        }
    }

    public void OnCancel()
    {
        m_cancelEvent.Invoke();
    }
}
