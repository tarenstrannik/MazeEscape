using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.LightAnchor;

public class EnemyMove : MoveController
{
    private NavMeshAgent m_navMeshAgent;
    [SerializeField] private List<Vector3> m_patrolingPoints = new List<Vector3>();
    public List<Vector3> PatrolingPoints
    {
        set
        {
            m_patrolingPoints = value;
        }
    }
    private Coroutine m_patrolingCoroutine = null;
    private Coroutine m_rotatingCoroutine = null;

    
    private Vector3 m_curWayPoint=Vector3.zero;
    //parameter to check if rotation finished
    [SerializeField] private float m_minDeltaRotation = 0.01f;

    protected override void Awake()
    {
        base.Awake();
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        m_navMeshAgent.speed = m_speed;
        m_navMeshAgent.angularSpeed = m_rotationSpeed;
       
    }
    protected override void Start()
    {
        base.Start();
        if (m_patrolingPoints.Count > 0) Patrol();
    }
    public override void GoToPoint(Vector3 position)
    {
        if(m_rotatingCoroutine!=null) StopCoroutine(m_rotatingCoroutine);
        //moving to point ad in the same time rotationg to make forward direction collinear with moving direction
        NavMeshMove(position);
        RotateAtDirection(new Vector3((position - transform.position).normalized.x, 0, (position - transform.position).normalized.z));
        if (m_patrolingCoroutine == null) m_patrolingCoroutine = StartCoroutine(MovingCoroutine());
    }

    private void NavMeshMove(Vector3 position)
    {
        m_navMeshAgent.SetDestination(position);
    }

    

    public void Patrol()
    {

        //Get next point and Rotate to it
        m_curWayPoint = GetNextPatrolingPoint();
        m_rotatingCoroutine = StartCoroutine(RotatingCoroutine(m_curWayPoint));
    }

    private IEnumerator MovingCoroutine()
    {
        while (true)
        {
            //checking if current movement finished
            if (!m_navMeshAgent.pathPending)
            {
                if (m_navMeshAgent.remainingDistance <= m_navMeshAgent.stoppingDistance)
                {
                    if (!m_navMeshAgent.hasPath || m_navMeshAgent.velocity.sqrMagnitude == 0f)
                    {
                        break;
                    }
                }
            }
            yield return null;
        }
        m_patrolingCoroutine = null;
        //starting patroling cycle again
        Patrol();
    }
    
    private IEnumerator RotatingCoroutine(Vector3 position)
    {
       //rotating to the next patroling point with defined speed and after that initiate moving to it
        while (Vector3.Angle(transform.forward.normalized, new Vector3((position - transform.position).normalized.x, 0, (position - transform.position).normalized.z)) > m_minDeltaRotation)
        {
            
            RotateAtDirection(new Vector3((position - transform.position).normalized.x, 0, (position - transform.position).normalized.z));
            yield return null;
        }
        transform.rotation= Quaternion.LookRotation(new Vector3((position - transform.position).normalized.x, 0, (position - transform.position).normalized.z), Vector3.up);
        NavMeshMove(position);
        if(m_patrolingCoroutine ==null) m_patrolingCoroutine = StartCoroutine(MovingCoroutine());
    }
    private Vector3 GetNextPatrolingPoint()
    {
        //getting point rotating to which  will be the smallest
        var curForward = transform.forward.normalized;
        var curDot = float.NegativeInfinity;
        Vector3 nextPoint = Vector3.zero;
        foreach(Vector3 point in m_patrolingPoints)
        {
            if(point!= m_curWayPoint)
            {
                var direction = Vector3.Normalize(point -transform.position);
                var dot = Vector3.Dot(curForward, direction);
                //if scalar multiplication of vectors is higher, it means they are colinear
                if(dot> curDot)
                {
                    nextPoint = point;
                    curDot = dot;
                }
            }
        }
        return nextPoint;
    }

}
