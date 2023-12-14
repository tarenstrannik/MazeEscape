using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Finish : MonoBehaviour
{
    public UnityEvent m_gameWin;

    //temporaly
    [SerializeField] private GameManager m_gameManager;

    private void Start()
    {
        m_gameWin.AddListener(m_gameManager.GameWin);
    }

    private void OnTriggerEnter(Collider other)
    {
        m_gameWin.Invoke();
    }
}
