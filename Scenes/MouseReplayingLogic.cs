using System;

public class MouseReplayingLogic
{

    public AbstractMouseInterface m_mouseInterface;
    public MouseRecordSequence m_replayingSequenceTemp=new MouseRecordSequence();


    public bool m_isPlaying;
    
    DateTime m_startPlaying = new DateTime();
    DateTime m_currentTime = new DateTime();
    public ulong m_currentTimeInMs;
    DateTime m_previousTime = new DateTime();
    public ulong m_previousTimeInMs;

    public void StartPlaying(MouseRecordSequence sequence) {

        m_replayingSequenceTemp = sequence;
        m_startPlaying = DateTime.Now;
        m_currentTime = DateTime.Now;
         m_previousTimeInMs = m_currentTimeInMs = 0;
        m_isPlaying = true;


    }

    public void StopReplaying()
    {
        m_isPlaying = false;
    }


    public void ThreadFunction() {

        if (m_mouseInterface == null)
            return;

        if (!m_isPlaying)
            return;

        m_currentTime = DateTime.Now;
        m_currentTimeInMs = (ulong)(m_currentTime - m_startPlaying).TotalMilliseconds;
        m_previousTimeInMs = (ulong)(m_previousTime - m_startPlaying).TotalMilliseconds;

        if (m_currentTimeInMs != m_previousTimeInMs) { 
            ActionToDoTime[] actions =  m_replayingSequenceTemp.GetActionsBetween(m_previousTimeInMs, m_currentTimeInMs);
            foreach (ActionToDoTime action in actions)
            {
                if (action is ActionToDo_MouseClick)
                    Execute((ActionToDo_MouseClick)action);
                if (action is ActionToDo_MoveAt)
                    Execute((ActionToDo_MoveAt)action);
                if (action is ActionToDo_TextEventRelease)
                    Execute((ActionToDo_TextEventRelease)action);
            }

            m_previousTime = m_currentTime;
            if (!m_replayingSequenceTemp.IsThereSomeLeftAfter(m_currentTimeInMs))
                StopReplaying();
        }
    }

    public delegate void TextEventEmission(string text);
    public TextEventEmission m_listenToTextEmittedByReplay;
    private void Execute(ActionToDo_TextEventRelease action)
    {
        if (m_listenToTextEmittedByReplay != null)
            m_listenToTextEmittedByReplay(action.m_textEventValue);
    }

    private void Execute(ActionToDo_MoveAt action)
    {
        m_mouseInterface.SetMousePositionFromTopLeftMainScreen(action.m_pourcentLeft2Right, action.m_pourcentTop2Down);
    }

    private void Execute(ActionToDo_MouseClick action)
    {
        m_mouseInterface.TriggerMouseChangeState(action.m_mouseButton, action.m_actionType);
    }
}