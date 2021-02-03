using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingularityPlanarComponent : SingularityComponent
{
    [SerializeField] protected GameObject m_rSourceB = null;
    [SerializeField] protected GameObject m_rSourceC = null;

    [SerializeField] protected Vector3 m_vPosSourceD = Vector3.zero;
    [SerializeField] protected Vector3 m_vPlanarNormal = Vector3.zero;

    protected Vector3 m_vPosProjected = Vector3.zero;


    //Check if the projected point is within the quad formed by the four points of the singularity
    bool CheckIfWithinSegement(Vector3 _posProjected, Vector3 _TargetPos, Vector3 _planeNormal)
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


        //Check if the projected point is within the singularity bondaries
        if (check[0] && check[1] && check[2] && check[3])
        {
            //Check if the target object is above the singularity
            if (Vector3.Dot(_TargetPos- transform.position, _planeNormal) > 0f)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check if the projected position is on the right of a vector formed by two position
    /// </summary>
    /// <param name="_point1">First position of the segement/Vector</param>
    /// <param name="_point2">First position of the segement/Vector</param>
    /// <param name="_planeNormal">Normal of the singularity plane</param>
    /// <param name="_posProjected">Projected point of the target object on the plane</param>
    /// <returns></returns>
    bool DotProductSegementDetection(Vector3 _point1, Vector3 _point2, Vector3 _planeNormal, Vector3 _posProjected)
    {
        Vector3 vectorSegement = _point2 - _point1;
        Vector3 vectorSegementLeft = Vector3.Cross(vectorSegement, _planeNormal);
        Vector3 SegementToProjected = _posProjected - _point1;

        return (Vector3.Dot(vectorSegementLeft, SegementToProjected) < 0f) ? false : true;
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
        CalcPlaneNormal(axialVectorU, axialVectorV);

        m_vPosProjected = CalcProjection(_targetPos, m_vPlanarNormal);


        Vector3 force = CalcAxialVector(_targetPos, m_vPosProjected);

        if (CheckIfWithinSegement(m_vPosProjected, _targetPos, m_vPlanarNormal))
        {
            return force * GenerateForce(m_vPosProjected, _targetPos);
        }

        return Vector3.zero;
    }

    void CalcPlaneNormal(Vector3 _axialVectorU, Vector3 _axialVectorV)
    {
        m_vPlanarNormal = (Vector3.Cross(_axialVectorV, _axialVectorU)).normalized;
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
        //From A to B
        Gizmos.DrawLine(transform.position, m_rSourceB.transform.position);
        //From A to C
        Gizmos.DrawLine(transform.position, m_rSourceC.transform.position);
        //From B to C
        Gizmos.DrawLine(m_rSourceB.transform.position, m_rSourceC.transform.position);

        //Calculating Position of source D
        Vector3 posD = (m_rSourceB.transform.position - transform.position) + (m_rSourceC.transform.position - transform.position) + transform.position;

        //From B to D
        Gizmos.DrawLine(m_rSourceB.transform.position, posD);
        //From C to D
        Gizmos.DrawLine(m_rSourceC.transform.position, posD);
        //From A to D
        Gizmos.DrawLine(transform.position, posD);

        //Draw range influence
        //From A
        Gizmos.DrawLine(transform.position, transform.position + (m_vPlanarNormal * m_fRangeMax));
        //From B
        Gizmos.DrawLine(m_rSourceB.transform.position, m_rSourceB.transform.position + (m_vPlanarNormal * m_fRangeMax));
        //From C
        Gizmos.DrawLine(m_rSourceC.transform.position, m_rSourceC.transform.position + (m_vPlanarNormal * m_fRangeMax));
        //From D
        Gizmos.DrawLine(posD, posD + (m_vPlanarNormal * m_fRangeMax));

        //Draw sources spheres
        Gizmos.color = m_cColorSource;
        Gizmos.DrawSphere(transform.position, 0.25f);
        Gizmos.DrawSphere(m_rSourceB.transform.position, 0.25f);
        Gizmos.DrawSphere(m_rSourceC.transform.position, 0.25f);
        Gizmos.DrawSphere(posD, 0.25f);

        

        Gizmos.color = Color.white;
        Gizmos.DrawSphere(m_vPosProjected, 0.15f);
    }
}
