using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Demo_MouseTripleClickRecord : MonoBehaviour
{
    public MouseInterfaceMono m_mouseInterface;

    public float m_timeToTriggerAction=5;
    public float m_isLeftDownTime; // Start recording
    public float m_isMiddleDownTime; // replay
    public float m_isRightDownTime; // stop recording
     float m_isLeftDownTimePrevious; // Start recording
     float m_isMiddleDownTimePrevious; // replay
     float m_isRightDownTimePrevious; // stop recording

    public MouseActionType m_isLeftDown; // Start recording
    public MouseActionType m_isMiddleDown; // replay
    public MouseActionType m_isRightDown; // stop recording


    public UnityEvent m_startRecording;
    public UnityEvent m_replayRecording;
    public UnityEvent m_stopRecording;


    void Start()
    {
        InvokeRepeating("CheckMousePressState",0,0.5f);
    }


    public void CheckMousePressState()
    {
        m_mouseInterface.GetMouseButtonInfo(MouseButtonType.Left, out m_isLeftDown);
        m_mouseInterface.GetMouseButtonInfo(MouseButtonType.Middle, out m_isMiddleDown);
        m_mouseInterface.GetMouseButtonInfo(MouseButtonType.Right, out m_isRightDown);

        if (m_isLeftDown == MouseActionType.Press)
            m_isLeftDownTime += 0.5f;
        else m_isLeftDownTime = 0;

        if (m_isMiddleDown == MouseActionType.Press)
            m_isMiddleDownTime += 0.5f;
        else m_isMiddleDownTime = 0;

        if (m_isRightDown == MouseActionType.Press)
            m_isRightDownTime += 0.5f;
        else m_isRightDownTime = 0;

        if (m_isLeftDownTimePrevious < m_timeToTriggerAction && m_isLeftDownTime >= m_timeToTriggerAction)
            m_startRecording.Invoke();
        if (m_isMiddleDownTimePrevious < m_timeToTriggerAction && m_isMiddleDownTime >= m_timeToTriggerAction)
            m_replayRecording.Invoke();
        if (m_isRightDownTimePrevious < m_timeToTriggerAction && m_isRightDownTime >= m_timeToTriggerAction)
            m_stopRecording.Invoke();


            m_isLeftDownTimePrevious  =m_isLeftDownTime   ;
            m_isMiddleDownTimePrevious=m_isMiddleDownTime  ;
            m_isRightDownTimePrevious = m_isRightDownTime;

    }
}
