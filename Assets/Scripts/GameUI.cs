using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject m_gameOverScreen;
    [SerializeField] private GameObject m_gameWinScreen;
    public void ShowGameOverScreen()
    {
        m_gameOverScreen.SetActive(true);
    }
    public void ShowGameWinScreen()
    {
        m_gameWinScreen.SetActive(true);
    }
}
