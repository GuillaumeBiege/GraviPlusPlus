using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingularityPunctualComponent : SingularityComponent
{
    public override Vector3 CalcForce(Vector3 _targetPos)
    {
        Vector3 force = CalcAxialVector(_targetPos, transform.position);
        return force * GenerateForce(transform.position, _targetPos);
    }

    private void OnDrawGizmos()
    {
        if (m_bIsPositive)
        {
            Gizmos.color = m_cColorPositive;
        }
        else
        {
            Gizmos.color = m_cColorNegative;
        }

        Gizmos.DrawWireSphere(transform.position, m_fRangeMin);
        Gizmos.DrawWireSphere(transform.position, m_fRangeMax);

        Gizmos.color = m_cColorSource;
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}
