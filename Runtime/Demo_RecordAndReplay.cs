using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Demo_RecordAndReplay : MonoBehaviour
{

    public float m_cycleTime = 20;
    public float m_cycleTimeLeft = 20;

    public Image m_backgroundState;
    public Text m_timeLeft;

    public bool m_isRecording;

    public UnityEvent m_requestRecord;
    public UnityEvent m_requestReplay;

    private void Awake()
    {


        SwitchState();
    }

    void Update()
    {
        m_cycleTimeLeft -= Time.deltaTime;
        m_timeLeft.text = string.Format("{0:0.0}", m_cycleTimeLeft);

        if (m_cycleTimeLeft < 0) {
            SwitchState();
            m_cycleTimeLeft = m_cycleTime;
        }
    }

    public Color m_recordColor;
    public Color m_replayColor;
    private void SwitchState()
    {
        m_isRecording = !m_isRecording;
        m_backgroundState.color = m_isRecording ? m_recordColor : m_replayColor;
        
        if (m_isRecording)
        {
            m_requestRecord.Invoke();
        }
        else {

            m_requestReplay.Invoke();
        }
    }
}
