using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBehaviour : MonoBehaviour
{
    [SerializeField] GameManager m_rGM = default;

    private void Awake()
    {
        m_rGM = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null && m_rGM != null)
        {
            m_rGM.CompleteDemo();

            Destroy(gameObject);
        }
    }
}
