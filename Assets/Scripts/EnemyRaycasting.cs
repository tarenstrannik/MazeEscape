using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyRaycasting : RaycastingController
{
    private EnemyController m_enemyController;

   

   [SerializeField] private float m_drawAndDamageAngle;

    public float DrawAndDamageAngle
    {
        set
        {
            m_drawAndDamageAngle = value;
        }
    }

    [SerializeField] private float m_deltaAngle;

    public float DeltaAngle
    {
        set
        {
            m_deltaAngle = value;
        }
    }

    [SerializeField] private float m_drawAndDamageDistance=5f;

    public float DrawAndDamageDistance
    {
        set
        {
            m_drawAndDamageDistance = value;
        }
    }

    [SerializeField] private float m_visibilityDistance = 5f;
    public float VisibilityDistance
    {
        set
        {
            m_visibilityDistance = value;
        }
    }

    [SerializeField] private float m_visibilityAngle;

    public float VisibilityAngle
    {
        set
        {
            m_visibilityAngle = value;
        }
    }

    [SerializeField] LayerMask m_raycastMask;

    private int m_rayPointsCount;

    private GameObject m_target;

    public GameObject Target
    {
        set
        {
            m_target = value;
        }
    }

    private bool m_isTargetInDamageZone = false;


    protected override void Awake()
    {
        base.Awake();
        m_enemyController=GetComponent<EnemyController>();

    }

    

    protected override void Start()
    {
        base.Start();

        m_rayPointsCount = Mathf.CeilToInt(2 * m_drawAndDamageAngle / m_deltaAngle) + 1;
        m_forwardLineRenderer.positionCount = m_rayPointsCount;
    }

    // Update is called once per frame
    protected override void Update()
    {
        DrawDamageZone();
        if (m_isTargetInDamageZone)
            m_enemyController.GiveDamage(m_target);

    }

    private void DrawDamageZone()
    {
        
        var startPoint = transform.position;
        
        var leftVector = Quaternion.AngleAxis(m_drawAndDamageAngle, Vector3.up) * transform.forward;
        var rightVector = Quaternion.AngleAxis(-m_drawAndDamageAngle, Vector3.up) * transform.forward;

        m_isTargetInDamageZone = false;

        var curVector = leftVector;
        RaycastHit hit;
        Vector3 point;
        float curDrawAndDamageDistance = m_drawAndDamageDistance;
        for (var i = 0; i < m_rayPointsCount - 1; i++)
        {
            if (Physics.Raycast(startPoint, curVector, out hit, curDrawAndDamageDistance))
            {

                if (hit.collider.gameObject != m_target)
                {
                    point = hit.point;
                }
                else
                {
                    m_isTargetInDamageZone = true;
                    if (Physics.Raycast(startPoint, curVector, out hit, curDrawAndDamageDistance, m_raycastMask))
                    {    
                        point = hit.point;
                    }
                    else
                    {
                        point = transform.position + curVector * curDrawAndDamageDistance;
                    };
                }
            }
            else
            {
                point = transform.position + curVector * curDrawAndDamageDistance;
            };
            m_forwardLineRenderer.SetPosition(i, point);
            curVector = Quaternion.AngleAxis(-m_deltaAngle, Vector3.up) * curVector;
            curDrawAndDamageDistance = m_drawAndDamageDistance * Mathf.Cos(Mathf.Deg2Rad * m_drawAndDamageAngle) / Mathf.Cos(Mathf.Deg2Rad * (m_drawAndDamageAngle - (i+1) * m_deltaAngle));

        }

        if (Physics.Raycast(startPoint, rightVector, out hit, m_drawAndDamageDistance))
        {

            if (hit.collider.gameObject != m_target)
            {
                point = hit.point;
            }
            else
            {
                m_isTargetInDamageZone = true;
                if (Physics.Raycast(startPoint, rightVector, out hit, m_drawAndDamageDistance, m_raycastMask))
                {
                    point = hit.point;
                }
                else
                {
                    point = transform.position + rightVector * m_drawAndDamageDistance;
                };
            }
        }
        else
        {
            point = transform.position + rightVector * curDrawAndDamageDistance;
        };
        

        m_forwardLineRenderer.SetPosition(m_rayPointsCount-1, point);
        m_leftLineRenderer.SetPosition(0, startPoint);
        m_leftLineRenderer.SetPosition(1, m_forwardLineRenderer.GetPosition(0));

        m_rightLineRenderer.SetPosition(0, startPoint);
        m_rightLineRenderer.SetPosition(1, m_forwardLineRenderer.GetPosition(m_rayPointsCount - 1));

    }

    

    public GameObject CheckIfCanSeePlayer()
    {
        if (!m_target.GetComponent<PlayerController>().IsDead)
        {
            if (Vector3.Distance(transform.position, m_target.transform.position) <= m_visibilityDistance)
            {

                if (Vector3.Angle(transform.forward.normalized, (m_target.transform.position - transform.position).normalized) <= m_visibilityAngle)
                {
                    if (Physics.Linecast(transform.position, m_target.transform.position, out var hitInfo))
                    {
                        if (hitInfo.collider.gameObject == m_target)
                        {
                            return m_target;
                        }
                    }
                }
            }

        };

        return null;
    }
}
