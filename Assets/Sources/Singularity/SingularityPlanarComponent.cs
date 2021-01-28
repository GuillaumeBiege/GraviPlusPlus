using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingularityPlanarComponent : SingularityComponent
{
    [SerializeField] GameObject m_rSourceB = null;
    [SerializeField] GameObject m_rSourceC = null;
    [SerializeField] Vector3 m_vPosSourceD = Vector3.zero;

    Vector3 PosProjected = Vector3.zero;


    //Check if the projected point is within the quad formed by the four points of the singularity
    bool CheckIfWithinSegement(Vector3 _posProjected, Vector3 _planeNormal)
    {
        //     ABl
        //      |
        //      A-------B-- BCl
        //      |       |
        //      |       |
        //      |       |
        // CAl--C-------D
        //              |
        //             DCl

        //To store the result of each detectection
        bool[] check = new bool[4];

        //Calculate Point D position
        m_vPosSourceD = (m_rSourceB.transform.position - transform.position) + (m_rSourceC.transform.position - transform.position) + transform.position;


        //For AB
        check[0] = DotProductSegementDetection(transform.position, m_rSourceB.transform.position, _planeNormal, _posProjected);

        //For BD
        check[1] = DotProductSegementDetection(m_rSourceB.transform.position, m_vPosSourceD, _planeNormal, _posProjected);

        //For DC
        check[2] = DotProductSegementDetection(m_vPosSourceD, m_rSourceC.transform.position, _planeNormal, _posProjected);

        //For CA
        check[3] = DotProductSegementDetection(m_rSourceC.transform.position, transform.position, _planeNormal, _posProjected);



        if (check[0] && check[1] && check[2] && check[3])
        {
            return true;
        }
        return false;
    }

    bool DotProductSegementDetection(Vector3 _point1, Vector3 _point2, Vector3 _planeNormal, Vector3 _posProjected)
    {
        Vector3 vectorSegement = _point2 - _point1;
        Vector3 vectorSegementLeft = Vector3.Cross(vectorSegement, _planeNormal);
        Vector3 SegementToProjected = _posProjected - _point1;

        return (Vector3.Dot(vectorSegementLeft, SegementToProjected) > 0f) ? false : true;
    }

    /// <summary>
    /// Calculate the projection of the target onto the plane
    /// </summary>
    /// <param name="_targetPos">Position of the influenced object</param>
    /// <param name="_planeNormal">Plane's normal vector</param>
    /// <returns>The projected position as Vector3</returns>
    Vector3 CalcProjection(Vector3 _targetPos, Vector3 _planeNormal)
    {
        Vector3 TargetToPosA = transform.position - _targetPos;

        Vector3 DirectionProjOnNormal = ((Vector3.Dot(TargetToPosA, _planeNormal) / _planeNormal.sqrMagnitude) * _planeNormal.normalized) + _targetPos;

        float k = ((_targetPos.x - transform.position.x) * _planeNormal.x + (_targetPos.y - transform.position.y) * _planeNormal.y + (_targetPos.z - transform.position.z) * _planeNormal.z) 
                  / ((_targetPos.x - DirectionProjOnNormal.x) * _planeNormal.x + (_targetPos.y - DirectionProjOnNormal.y) * _planeNormal.y + (_targetPos.z - DirectionProjOnNormal.z) * _planeNormal.z);

        return (k * (DirectionProjOnNormal - _targetPos)) + _targetPos;
    }

    public override Vector3 CalcForce(Vector3 _targetPos)
    {
        Vector3 axialVectorU = CalcAxialVector(transform.position, m_rSourceB.transform.position);
        Vector3 axialVectorV = CalcAxialVector(transform.position, m_rSourceC.transform.position);
        Vector3 planeNormal = CalcPlaneNormal(axialVectorU, axialVectorV);

        Vector3 posProjected = CalcProjection(_targetPos, planeNormal);

        //Debug//////////
        PosProjected = posProjected;
        /////////////////

        Vector3 force = CalcAxialVector(_targetPos, posProjected);
        return force * GenerateForce(posProjected, _targetPos);
    }

    Vector3 CalcPlaneNormal(Vector3 _axialVectorU, Vector3 _axialVectorV)
    {
        return Vector3.Cross(_axialVectorV, _axialVectorU);
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

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(PosProjected, 0.15f);
    }
}
