using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Finish : MonoBehaviour
{
    public UnityEvent m_gameWin;



    private void OnTriggerEnter(Collider other)
    {
        m_gameWin.Invoke();
    }
}
