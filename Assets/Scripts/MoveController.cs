using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MoveController : MonoBehaviour
{
    protected Rigidbody m_personRb;

    [SerializeField] protected float m_speed = 5f;
    public float Speed
    {
        set
        {
            m_speed = value;
        }
    }
    [SerializeField] protected float m_rotationSpeed = 20f;
    public float RotationSpeed
    {
        set
        {
            m_rotationSpeed = value;
        }
    }

    protected Vector2 m_movementDirection = new Vector2(0, 0);
    
    void OnEnable()
    {
        m_personRb = GetComponent<Rigidbody>();
    }
    public void CharacterMovement(Vector2 value)
    {
        m_movementDirection = value;
    }
    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {

    }
    protected virtual void FixedUpdate()
    {
       
        MoveAtDirection();
        var curDirection = new Vector3(m_movementDirection.normalized.x, 0, m_movementDirection.normalized.y);
        RotateAtDirection(curDirection);
    }

    protected virtual void MoveAtDirection()
    {
        m_personRb.velocity = Vector3.forward * m_speed * m_movementDirection.y + Vector3.right * m_speed * m_movementDirection.x;
    }

    protected virtual void RotateAtDirection(Vector3 curDirection)
    {
        
        if (curDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(curDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);
        }
    }

    public virtual void GoToPoint(Vector3 position)
    {

    }
}
