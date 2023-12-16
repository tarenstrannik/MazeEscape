using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingController : MonoBehaviour
{
    protected LineRenderer m_forwardLineRenderer;
    [SerializeField] protected LineRenderer m_leftLineRenderer;
    [SerializeField] protected LineRenderer m_rightLineRenderer;
    // Start is called before the first frame update

    protected virtual void Awake()
    {
        m_forwardLineRenderer = GetComponent<LineRenderer>();
        
    }
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    
}
