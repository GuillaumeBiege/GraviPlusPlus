using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using graviplus;

public abstract class SingularityComponent : MonoBehaviour
{
    #region Variables
    //FORCE
    //Attraction/Repulsion force of the singularity
    public float m_fForce = 5f;
    //Clamp limits to avoid extreme behaviour
    public float m_fForceClampMin = 0f;
    public float m_fForceClampMax = 100f;


    //RANGE
    //Singularity limit influence distance
    public float m_fRangeMin = 0f;
    public float m_fRangeMax = 5f;


    public bool m_bIsPositive = true;
    
    public Curve.Type m_eCurve = Curve.Type.LINEAR;
    public int m_iDividePower = 1;

    protected Color m_cColorPositive = Color.red;
    protected Color m_cColorNegative = Color.blue;
    protected Color m_cColorSource = Color.green;
    
    #endregion

    private void Awake()
    {
        SingularityManager.Instance.Registration(this);
    }

    private void OnDestroy()
    {
        SingularityManager.Instance.Deregistration(this);
    }

    protected Vector3 CalcAxialVector(Vector3 _singularityPosA, Vector3 _singularityPosB)
    {
        return (_singularityPosB - _singularityPosA).normalized;
    }


    protected float GenerateForce(Vector3 _posProjected, Vector3 _targetPos )
    {
        float distance = Vector3.Distance(_posProjected, _targetPos);

        //Range verification
        if (distance >= m_fRangeMin && distance <= m_fRangeMax)
        {
            float result = 0;
            switch (m_eCurve)
            {
                case Curve.Type.LINEAR:
                    result = Curve.ProcessCurveLinear(m_fForce, distance, m_fRangeMax);
                    break;
                case Curve.Type.RLINEAR:
                    result = Curve.ProcessReverseCurveLinear(m_fForce, distance, m_fRangeMax);
                    break;
                case Curve.Type.DIVIDE:
                    result = Curve.ProcessCurveDivide(m_fForce, distance, m_fRangeMax, m_iDividePower);
                    break;
                case Curve.Type.CONSTANT:
                    result = Curve.ProcessCurveConstant(m_fForce);
                    break;
                default:
                    break;
            }
            

            //CLAMPS
            result = (result >= m_fForceClampMax) ? m_fForceClampMax : result;
            result = (result <= m_fForceClampMin) ? m_fForceClampMin : result;

            //Attraction / Repulsion
            if (m_bIsPositive)
                return result;
            else
                return -result;


        }
        else
        {
            return 0f;
        }
    }

    public virtual Vector3 CalcForce(Vector3 _targetPos)
    {
        return Vector3.zero;
    }



    #region Error handling
    protected virtual void GenerateErrorMessage(string _methodName, string _cause)
    {
        Debug.LogError("Issue occurec in " + this.GetType().Name + " in method : " + _methodName + " => " + _cause);
    }
    #endregion


}






