using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownDestroy : MonoBehaviour
{
    [SerializeField] float m_fTimer = 1f;
    [SerializeField] float m_fTimerMax = 10f;

    private void Update()
    {
        if (m_fTimer > 0f)
        {
            m_fTimer -= Time.deltaTime / m_fTimerMax;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
