using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PunctualDemoUI : MonoBehaviour
{
    [SerializeField] Toggle m_rHideUIToggle = default;

    [SerializeField] SingularityComponent m_rSingularity = default;
    [SerializeField] GameObject SingularitySphere = default;
    [SerializeField] SphereSpawner m_rSpawner = default;
    [SerializeField] GameObject m_rMenuSing = default;
    [SerializeField] GameObject m_rMenuSpawner = default;
    [SerializeField] Toggle m_rIsPositiveToggle = default;
    [SerializeField] GraviSlider m_rForcePowerSlider = default;
    [SerializeField] GraviSlider m_rRangeSlider = default;

    [SerializeField] GraviSlider m_rSpawnRateSlider = default;
    [SerializeField] GraviSlider m_rPositionSlider = default;

    [SerializeField] Transform PosSpawnerA = default;
    [SerializeField] Transform PosSpawnerB = default;


    // Start is called before the first frame update
    void Start()
    {
        m_rHideUIToggle.onValueChanged.AddListener(SetToggleActiveMenu);
        m_rIsPositiveToggle.onValueChanged.AddListener(SetToggleIsPositive);

        m_rForcePowerSlider.m_fminValue = m_rSingularity.m_fForceClampMin;
        m_rForcePowerSlider.m_fmaxValue = m_rSingularity.m_fForceClampMax;
        m_rForcePowerSlider.SetSliderValue( m_rSingularity.m_fForce / m_rSingularity.m_fForceClampMax);

        m_rRangeSlider.m_fminValue = m_rSingularity.m_fRangeMin;
        m_rRangeSlider.m_fmaxValue = m_rSingularity.m_fRangeMax;
        m_rRangeSlider.SetSliderValue(1f);

        m_rSpawnRateSlider.m_fminValue = 1f;
        m_rSpawnRateSlider.m_fmaxValue = 5f;
        m_rSpawnRateSlider.SetSliderValue(0.1f);

        m_rPositionSlider.SetSliderValue(0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        m_rSingularity.m_fForce = m_rForcePowerSlider.GetValue();
        m_rSingularity.m_fRangeMax = m_rRangeSlider.GetValue();
        Vector3 scale = new Vector3(1f, 1f, 1f) * m_rRangeSlider.GetValue() * 2f;
        //SingularitySphere.transform.localScale = scale;

        m_rSpawner.spawnRate = m_rSpawnRateSlider.GetValue();


        m_rSpawner.transform.position = Vector3.Lerp(PosSpawnerA.position, PosSpawnerB.position, m_rPositionSlider.GetValue());
    }

    void SetToggleActiveMenu(bool _b)
    {
        //TODO : Secure ref and add error
        m_rMenuSing.SetActive(_b);
        m_rMenuSpawner.SetActive(_b);
    }

    void SetToggleIsPositive(bool _b)
    {
        //TODO : Secure ref and add error
        m_rSingularity.m_bIsPositive = _b;

        Text label = m_rIsPositiveToggle.GetComponentInChildren<Text>();
        if (label != null)
        {
            if (_b)
            {
                label.text = "Attractive";
                label.color = Color.red;
                //SingularitySphere.GetComponent<MeshRenderer>().material.SetColor("Color_base", Color.red);
                //SingularitySphere.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            else
            {
                label.text = "Repulsive";
                label.color = Color.blue;
                //SingularitySphere.GetComponent<MeshRenderer>().material.SetColor("Color_base", Color.blue);
                //SingularitySphere.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
