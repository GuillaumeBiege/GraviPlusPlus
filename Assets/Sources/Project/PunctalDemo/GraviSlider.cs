using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraviSlider : MonoBehaviour
{
    [SerializeField] Slider m_rSlider = default;
    Text m_rTitle = default;
    [SerializeField] Text m_rTextMin = default;
    [SerializeField] Text m_rTextMax = default;

    [SerializeField] string m_sTitle = "Slider";

    
    public float m_fminValue = 0f;
    public float m_fmaxValue = 0f;
    [Range(0f, 1f)]
    public float m_fSliderValue = 0f;

    // Start is called before the first frame update
    void Start()
    {
        m_rTitle = GetComponent<Text>();
        //m_rSlider = GetComponentInChildren<Slider>();
        m_rSlider.minValue = 0f;
        m_rSlider.maxValue = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        m_rTitle.text = m_sTitle + " : " + ((int)(m_fSliderValue * m_fmaxValue)).ToString();

        m_rTextMin.text = m_fminValue.ToString();
        m_rTextMax.text = m_fmaxValue.ToString();

        m_fSliderValue = m_rSlider.normalizedValue;
    }

    public float GetValue()
    {
        return m_fmaxValue * m_fSliderValue;
    }

    public void SetSliderValue(float _f)
    {
        m_fSliderValue = _f;
        m_rSlider.normalizedValue = m_fSliderValue;
    }
}
