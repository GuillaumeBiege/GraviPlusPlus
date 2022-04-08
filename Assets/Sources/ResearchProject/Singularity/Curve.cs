using System;
using System.Collections;
using System.Collections.Generic;



namespace graviplus
{
    [Serializable]
    public class Curve
    {
        public enum Type
        {
            LINEAR,
            RLINEAR,
            DIVIDE,
            CONSTANT
        }

        public static float ProcessCurveConstant(float _force)
        {
            return _force;
        }

        public static float ProcessCurveLinear(float _force, float _distance, float _distanceMax)
        {
            float forceMax = _force * _distanceMax;

            return forceMax - _distance;
        }

        public static float ProcessReverseCurveLinear(float _force, float _distance, float _distanceMax)
        {
            return _force * _distanceMax;
        }

        public static float ProcessCurveDivide(float _force, float _distance, float _distanceMax, float _dividePower)
        {
            float denominator = 1f;
            for (int i = 0; i < _dividePower; i++)
            {
                denominator = denominator * _distance;
            }

            return _force / denominator;
        }

    }

    

    //public class CurveLogaritmic : Curve
    //{
    //    public override float Process(float _force, float _distance, float _distanceMax)
    //    {
    //        return _force * _distance * 0.2f;
    //    }

        
    //}
}

