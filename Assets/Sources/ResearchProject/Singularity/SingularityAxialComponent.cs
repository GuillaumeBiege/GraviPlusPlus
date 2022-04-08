using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingularityAxialComponent : SingularityComponent
{
    public GameObject m_rSourceB = null;


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

    Vector3 CalcProjection(Vector3 _targetPos, Vector3 _axialVector)
    {
        Vector3 PosAToTarget = _targetPos - transform.position;
        float dotResult = Vector3.Dot(_axialVector, PosAToTarget);



        return (_axialVector * dotResult) + transform.position;
    }

    public override Vector3 CalcForce(Vector3 _targetPos)
    {
        Vector3 axialVector = CalcAxialVector(transform.position, m_rSourceB.transform.position);
        Vector3 posProjected = CalcProjection(_targetPos, axialVector);

        if (CheckIfWithinSegement(posProjected, axialVector))
        {
            Vector3 force = CalcAxialVector(_targetPos, posProjected);
            return force * GenerateForce(posProjected, _targetPos);
        }
        else
        {
            return Vector3.zero;
        }
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
        

        Gizmos.color = m_cColorSource;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.DrawSphere(m_rSourceB.transform.position, 0.25f);
    }
}
