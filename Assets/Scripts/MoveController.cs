using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MoveController : MonoBehaviour
{
    protected Rigidbody m_personRb;

    [SerializeField] protected float m_speed = 5f;
    [SerializeField] protected float m_rotationSpeed = 20f;

    protected Vector2 m_movementDirection = new Vector2(0, 0);
    
    void OnEnable()
    {
        m_personRb = GetComponent<Rigidbody>();
    }
    private void CharacterMovement(Vector2 value)
    {
        m_movementDirection = value;
    }
    protected virtual void Awake()
    {

    }
    protected virtual void FixedUpdate()
    {
        MoveAtDirection();
        RotateAtDirection();
    }

    protected virtual void MoveAtDirection()
    {
        m_personRb.velocity = Vector3.forward * m_speed * m_movementDirection.y + Vector3.right * m_speed * m_movementDirection.x;
    }

    protected virtual void RotateAtDirection()
    {
        var curDirection = new Vector3(m_movementDirection.normalized.x, 0, m_movementDirection.normalized.y);
        if (curDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(curDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);
        }
    }
}
