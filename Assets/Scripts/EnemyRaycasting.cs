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

    [SerializeField] private bool m_isFrontDamageLineFlat;

    public bool IsFrontDamageLineFlat
    {
        set
        {
            m_isFrontDamageLineFlat = value;
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

    [SerializeField] private bool m_isFrontViewLineFlat;

    public bool IsFrontViewLineFlat
    {
        set
        {
            m_isFrontViewLineFlat = value;
        }
    }

    [SerializeField] LayerMask m_raycastMask;

    private int m_damageRayPointsCount;
    private int m_visibilityRayPointsCount;

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
        //calculating number of rays
        m_damageRayPointsCount = Mathf.CeilToInt(2 * m_drawAndDamageAngle / m_deltaAngle) + 1;
        m_forwardLineRenderer.positionCount = m_damageRayPointsCount;

        m_visibilityRayPointsCount = Mathf.CeilToInt(2 * m_visibilityAngle / m_deltaAngle) + 1;
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
        //presize but costly solution. can be simplified by increasing step. or if precision isn/t necesssary replaced by checking distance, angle from forward to the player direction (if is near) and one ray/line cast to check, if player isn't hidden behind the wall
        var startPoint = transform.position;
        //calculating bounds of raycasting
        var leftVector = Quaternion.AngleAxis(m_drawAndDamageAngle, Vector3.up) * transform.forward;
        var rightVector = Quaternion.AngleAxis(-m_drawAndDamageAngle, Vector3.up) * transform.forward;

        m_isTargetInDamageZone = false;

        var curVector = leftVector;
        RaycastHit hit;
        Vector3 point;
        float curDrawAndDamageDistance = m_drawAndDamageDistance;
        //going from left to the right to check all damage zone if there are walls or player in it
        for (var i = 0; i < m_damageRayPointsCount - 1; i++)
        {
            if (Physics.Raycast(startPoint, curVector, out hit, curDrawAndDamageDistance))
            {
                //if no player that it is wall and end of the ray/part of the damage line
                if (hit.collider.gameObject != m_target)
                {
                    point = hit.point;
                }
                else
                {
                    //if hit on player than he should be damaged. but also need to repeat raycast with masking player, to draw beauty damage area
                    m_isTargetInDamageZone = true;
                    if (Physics.Raycast(startPoint, curVector, out hit, curDrawAndDamageDistance, m_raycastMask))
                    {    
                        point = hit.point;
                    }
                    //if no hit drawing at the max distance
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
            //changing distance of cast according to angle to achieve triangle damage zone, if selected in parameters
            if(m_isFrontDamageLineFlat) curDrawAndDamageDistance = m_drawAndDamageDistance * Mathf.Cos(Mathf.Deg2Rad * m_drawAndDamageAngle) / Mathf.Cos(Mathf.Deg2Rad * (m_drawAndDamageAngle - (i+1) * m_deltaAngle));

        }
        //last ray drawing not in cycle but precisely along right border in case if last step will be over the right bound
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
        
        //drawing line
        m_forwardLineRenderer.SetPosition(m_damageRayPointsCount - 1, point);
        m_leftLineRenderer.SetPosition(0, startPoint);
        m_leftLineRenderer.SetPosition(1, m_forwardLineRenderer.GetPosition(0));

        m_rightLineRenderer.SetPosition(0, startPoint);
        m_rightLineRenderer.SetPosition(1, m_forwardLineRenderer.GetPosition(m_damageRayPointsCount - 1));

    }

    
    //the same principle to check if player can be seen
    public GameObject CheckIfCanSeePlayer()
    {
        if (!m_target.GetComponent<PlayerController>().IsDead)
        {
            if (Vector3.Distance(transform.position, m_target.transform.position) <= m_visibilityDistance)
            {

                var startPoint = transform.position;

                var leftVector = Quaternion.AngleAxis(m_visibilityAngle, Vector3.up) * transform.forward;
                var rightVector = Quaternion.AngleAxis(-m_visibilityAngle, Vector3.up) * transform.forward;

                var curVector = leftVector;
                RaycastHit hit;

                float curVisibilityDistance = m_visibilityDistance;
                for (var i = 0; i < m_visibilityRayPointsCount - 1; i++)
                {
                    if (Physics.Raycast(startPoint, curVector, out hit, curVisibilityDistance))
                    {
                        
                        if (hit.collider.gameObject == m_target)
                        {
                            return m_target;
                        }
                    }
                    curVector = Quaternion.AngleAxis(-m_deltaAngle, Vector3.up) * curVector;
                    if(m_isFrontViewLineFlat) curVisibilityDistance = m_visibilityDistance * Mathf.Cos(Mathf.Deg2Rad * m_drawAndDamageAngle) / Mathf.Cos(Mathf.Deg2Rad * (m_drawAndDamageAngle - (i + 1) * m_deltaAngle));
                }
                if (Physics.Raycast(startPoint, rightVector, out hit, m_visibilityDistance))
                {
                    if (hit.collider.gameObject == m_target)
                    {
                        return m_target;
                    }
                }
            }
        };

        return null;
    }
}
