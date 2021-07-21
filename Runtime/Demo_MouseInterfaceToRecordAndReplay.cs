using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Demo_MouseInterfaceToRecordAndReplay : MonoBehaviour
{
    public MouseReplayerThreadMono m_mouseRecorder;
    public MouseInterfaceMono m_mouseInteface;

    public TextEvent m_onTextEventReceived;
    [System.Serializable]
    public class TextEvent : UnityEvent<string> { }


    private void Awake()
    {
        m_mouseRecorder.SetMouseInterface(m_mouseInteface);
        m_mouseRecorder.AddMouseEventListener(TextEventEmitted);
    }

    private void TextEventEmitted(string text)
    {
        m_onTextEventReceived.Invoke(text);
    }

    public void StartRecording() {

        m_mouseRecorder.StartRecording();
    }


    public void StopRecording() {

        m_mouseRecorder.StopRecording();
    }

    public void ReplayInMemoryRecord() {

        MouseRecordSequence sequence = m_mouseRecorder.GetSequenceAsReference();
        m_mouseRecorder.Play(sequence);
//        m_mouseRecorder.SaveAsFileNearExe("MouseRecord.txt");
    }




}
