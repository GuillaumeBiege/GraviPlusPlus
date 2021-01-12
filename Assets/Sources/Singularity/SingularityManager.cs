using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using graviplus;

public class SingularityManager : MonoBehaviour
{
    #region SINGLETON PATTERN
    public static SingularityManager _instance;
    public static SingularityManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SingularityManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("@SingletonManager");
                    _instance = container.AddComponent<SingularityManager>();
                }
            }

            return _instance;
        }
    }
    #endregion


    [SerializeField] List<SingularityComponent> m_lSingularities = new List<SingularityComponent>();
    [SerializeField] List<SingularityTarget> m_lTargets = new List<SingularityTarget>();

    public bool Registration(SingularityComponent _singComp)
    {
        if (!m_lSingularities.Contains(_singComp))
        {
            m_lSingularities.Add(_singComp);
        }
        else
        {
            Debug.LogWarning("Issue ! Singularity manager :> " + _singComp.gameObject.name + " is already registered in the singularity list");
            return false;
        }
       

        return true;
    }

    public bool Registration(SingularityTarget _target)
    {
        if (!m_lTargets.Contains(_target))
        {
            m_lTargets.Add(_target);
        }
        else
        {
            Debug.LogWarning("Issue ! Singularity manager :> " + _target.gameObject.name + " is already registered in the singularity target list");
            return false;
        }

        return true;
    }

    public bool Deregistration(SingularityComponent _singComp)
    {
        if (m_lSingularities.Contains(_singComp))
        {
            m_lSingularities.Remove(_singComp);
        }
        else
        {
            Debug.LogWarning("Issue ! Singularity manager :> " + _singComp.gameObject.name + " has already been deregistered of the singularity list");
            return false;
        }

        return true;
    }

    public bool Deregistration(SingularityTarget _target)
    {
        if (m_lTargets.Contains(_target))
        {
            m_lTargets.Remove(_target);
        }
        else
        {
            Debug.LogWarning("Issue ! Singularity manager :> " + _target.gameObject.name + " has already been deregistered of the singularity target list");
            return false;
        }

        return true;
    }

    //Singularity influence is processed in the fixed Update to be done before physics is processed by Unity
    private void FixedUpdate()
    {
        foreach (SingularityTarget target in m_lTargets)
        {
            Vector3 sumForce = Vector3.zero;

            //Sum the force influancing the target from each singularities
            foreach (SingularityComponent singularity in m_lSingularities)
            {
                sumForce += singularity.CalcForce(target.transform.position);
            }

            //Setting the summed force to the Target
            target.ApplySingularityForce(sumForce);
        }
    }
}
