using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ListenToTextEmittionOfReplayer : MonoBehaviour
{
    public MouseReplayerThreadMono m_replayer;
    public EmittionTextEvent m_onTextEmittedOnUnityThread;

    private Queue<string> m_textReceived = new Queue<string>();
    [System.Serializable]
    public class EmittionTextEvent : UnityEvent<string> { }

    private void Update()
    {
        if (m_textReceived.Count > 0) { 
            m_onTextEmittedOnUnityThread.Invoke(m_textReceived.Dequeue());
        }
    }

    void Start()
    {
        m_replayer.GetReplayer().AddTextEmittedListener(TextEmitted);
    }

    void OnDestroy()
    {
        m_replayer.GetReplayer().RemoveTextEmittedListener(TextEmitted);
    }

    private void TextEmitted(string text)
    {
        m_textReceived.Enqueue(text);
    }
}
