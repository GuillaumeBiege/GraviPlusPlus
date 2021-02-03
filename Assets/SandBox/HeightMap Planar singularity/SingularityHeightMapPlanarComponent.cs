using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingularityHeightMapPlanarComponent : SingularityPlanarComponent
{
    [SerializeField] Texture2D m_rHeightMapRef = default;
    public override Vector3 CalcForce(Vector3 _targetPos)
    {
        
        Vector3 result = base.CalcForce(_targetPos);
        if (result.magnitude > 0.001f)
        {
            //Debug.Log("Pixel value = " + AdjusteForceByHeightMap().ToString());
            return result * AdjusteForceByHeightMap();
        }

        return Vector3.zero;
        
    }

    float AdjusteForceByHeightMap()
    {
        if (m_rHeightMapRef != null)
        {
            //Get the projected point normalized position in the boundaries
            //the axis and value of 1
            Vector3 Xaxis = m_rSourceB.transform.position - transform.position;
            Vector3 Yaxis = m_rSourceC.transform.position - transform.position;

            //The projected pos on those axis
            Vector3 ProjOnX = ((Vector3.Dot(m_vPosProjected /*- transform.position*/, Xaxis) / Xaxis.sqrMagnitude) * Xaxis);
            Vector3 ProjOnY = ((Vector3.Dot(m_vPosProjected /*- transform.position*/, Yaxis) / Yaxis.sqrMagnitude) * Yaxis);

            //Read the red and blue color on the coordinated pixel then surbstract red to blue to give the force factor and signe
            Debug.Log("x : " + (ProjOnX.magnitude / Xaxis.magnitude).ToString() + "  y : " + (ProjOnY.magnitude / Yaxis.magnitude).ToString());
            return m_rHeightMapRef.GetPixelBilinear(Mathf.Clamp(ProjOnX.magnitude / Xaxis.magnitude, 0f, 1f), Mathf.Clamp(ProjOnY.magnitude / Yaxis.magnitude, 0f, 1f)).r 
                - m_rHeightMapRef.GetPixelBilinear(Mathf.Clamp(ProjOnX.magnitude / Xaxis.magnitude, 0f, 1f), Mathf.Clamp(ProjOnY.magnitude / Yaxis.magnitude, 0f, 1f)).b;
        }

        return 0f;
    }
}
