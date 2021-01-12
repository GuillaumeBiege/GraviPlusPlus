using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    GameManager m_rGM = default;

    [SerializeField] Text m_rVictoryText = default;


    private void Awake()
    {
        m_rGM = FindObjectOfType<GameManager>();

        if (m_rGM != null)
        {
            m_rGM.ONDemoCompleted += DisplayDemoCompleted;
        }
    }

    public void DisplayDemoCompleted()
    {
        m_rVictoryText.gameObject.SetActive(true);
    }
}
