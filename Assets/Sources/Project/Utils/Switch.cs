using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] bool m_bIsActivated = false;

    public event VoidDelegate ONSwitchActivated;

    [SerializeField] Color m_cColorActivated = Color.green;
    [SerializeField] Color m_cColorDesactivated = Color.red;

    Material m_rMaterial = default;

    private void Awake()
    {
        m_rMaterial = GetComponent<MeshRenderer>().material;

        if (m_rMaterial != null)
        {
            m_rMaterial.SetColor("MainColor", m_cColorDesactivated);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SingularityTarget target = other.gameObject.GetComponent<SingularityTarget>();

        if (target != null && m_bIsActivated == false)
        {
            m_bIsActivated = true;

            if (m_rMaterial != null)
            {
                m_rMaterial.SetColor("MainColor", m_cColorActivated);
            }

            ONSwitchActivated?.Invoke();
        }
    }
}
