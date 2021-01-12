using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingularityPlanarComponent : SingularityComponent
{
    [SerializeField] GameObject m_rSourceB = null;
    [SerializeField] GameObject m_rSourceC = null;

    bool CheckIfWithinSegement(Vector3 _posProjected, Vector3 _axialVector)
    {
        Vector3 SourceToProj = _posProjected - transform.position;
        Vector3 TargetToProj = _posProjected - m_rSourceB.transform.position;

        if (Vector3.Dot(SourceToProj, _axialVector) < 0)
        {
            return false;
        }
        else if (Vector3.Dot(TargetToProj, -_axialVector) < 0)
        {
            return false;
        }


        return true;
    }

    Vector3 CalcProjection(Vector3 _targetPos, Vector3 _axialVectorU, Vector3 _axialVectorV)
    {
        Vector3 PosAToTarget = _targetPos - transform.position;
        float dotResult = Vector3.Dot(_axialVectorU, PosAToTarget);
        Vector3 PosProjected_AB = (_axialVectorU * dotResult);


        dotResult = Vector3.Dot(_axialVectorV, PosAToTarget);
        Vector3 PosProjected_AC = (_axialVectorV * dotResult);

        return PosProjected_AB + PosProjected_AC + transform.position;
    }

    public override Vector3 CalcForce(Vector3 _targetPos)
    {
        Vector3 axialVectorU = CalcAxialVector(transform.position, m_rSourceB.transform.position);
        Vector3 axialVectorV = CalcAxialVector(transform.position, m_rSourceC.transform.position);

        Vector3 posProjected = CalcProjection(_targetPos, axialVectorU, axialVectorV);

        Vector3 force = CalcAxialVector(_targetPos, posProjected);
        return force * GenerateForce(posProjected, _targetPos);
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
        Gizmos.DrawLine(transform.position, m_rSourceB.transform.position);
        Gizmos.DrawLine(transform.position, m_rSourceC.transform.position);
        Gizmos.DrawLine(m_rSourceB.transform.position, m_rSourceC.transform.position);
        Vector3 posD = (m_rSourceB.transform.position - transform.position) + (m_rSourceC.transform.position - transform.position) + transform.position;
        Gizmos.DrawLine(m_rSourceB.transform.position, posD);
        Gizmos.DrawLine(m_rSourceC.transform.position, posD);
        Gizmos.DrawLine(transform.position, posD);

        Gizmos.color = m_cColorSource;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.DrawSphere(m_rSourceB.transform.position, 0.25f);
        Gizmos.DrawSphere(m_rSourceC.transform.position, 0.25f);
        Gizmos.DrawSphere(posD, 0.25f);
    }
}
