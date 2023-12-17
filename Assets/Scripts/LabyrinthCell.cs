using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthCell : MonoBehaviour
{
    [SerializeField] private bool m_isProceeded = false;
    public bool IsProceeded
    {
        get
        {
            return m_isProceeded;
        }
        private set
        {
            m_isProceeded = value;
        }
    }

    private float m_innerSize = 9f;
    public float InnerSize
    {
        get
        {
            return m_innerSize;
        }
    }

    [SerializeField] private GameObject[] m_walls;
    

    public void Proceed()
    {
        m_isProceeded = true;
    }

    public void DeactivateWall(int index)
    {
       //walls are numerated from 0 to 3 from the top wall clockwise
        m_walls[index].SetActive(false);
    }
    public bool CheckIfWallOpen(int index)
    {
        return !m_walls[index].activeSelf;
    }
}
