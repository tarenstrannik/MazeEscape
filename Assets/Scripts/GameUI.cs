using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject m_gameOverScreen;
    [SerializeField] private GameObject m_gameWinScreen;
    [SerializeField] private GameObject m_gamePauseScreen;

    [SerializeField] private GameObject m_inGameUI;
    public void ShowGameOverScreen()
    {
        m_gameOverScreen.SetActive(true);
        m_inGameUI.SetActive(false);
    }
    public void ShowGameWinScreen()
    {
        m_gameWinScreen.SetActive(true);
        m_inGameUI.SetActive(false);
    }

    public void TogglePauseScreen()
    {
        m_gamePauseScreen.SetActive(!m_gamePauseScreen.activeSelf);
        m_inGameUI.SetActive(!m_inGameUI.activeSelf);
    }
}
