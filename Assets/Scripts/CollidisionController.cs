using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    protected MoveController m_moveController;
    protected CharacterController m_personController;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        m_moveController = GetComponent<MoveController>();
        m_personController = GetComponent<CharacterController>();
        //Debug.Log(personController);
    }



    protected virtual void OnCollisionEnter(Collision collision)
    {
        
    }
}
