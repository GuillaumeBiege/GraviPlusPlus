using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBallSwitchActivation : MonoBehaviour
{
    Switch m_rSwitch = default;
    [SerializeField] GameObject BridgePlatform = default;

    private void Awake()
    {
        m_rSwitch = FindObjectOfType<Switch>();
        m_rSwitch.ONSwitchActivated += ActivateBridge;
    }

    private void OnDestroy()
    {
        m_rSwitch.ONSwitchActivated -= ActivateBridge;
    }

    public void ActivateBridge()
    {
        BridgePlatform.SetActive(true);
    }
}
