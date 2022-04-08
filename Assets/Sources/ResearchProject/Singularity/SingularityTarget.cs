using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingularityTarget : MonoBehaviour
{
    [SerializeField] Rigidbody m_rRB = null;
    public Vector3 m_vCurrentGravityDir = Vector3.zero;
    

    void Start()
    {
        SingularityManager.Instance.Registration(this);

        m_rRB = GetComponent<Rigidbody>();
    }

    //Setting the summed influence of singularities to the rigidbody
    public void ApplySingularityForce(Vector3 _force)
    {
        if (m_rRB != null)
        {
            m_vCurrentGravityDir = Physics.gravity + _force;
            m_rRB.AddForce(_force);
        }
    }

    private void OnDestroy()
    {
        SingularityManager.Instance.Deregistration(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + m_vCurrentGravityDir * 0.5f);
    }
}
