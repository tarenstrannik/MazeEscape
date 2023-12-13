using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MoveController
{
    private NavMeshAgent m_navMeshAgent;

    protected override void Awake()
    {
        base.Awake();
        m_navMeshAgent=GetComponent<NavMeshAgent>();
        m_navMeshAgent.speed = m_speed;
        m_navMeshAgent.angularSpeed = m_rotationSpeed;

    }
    public void NavMeshMove(Vector3 position)
    {
        m_navMeshAgent.SetDestination(position);
    }
    
  
    


}
