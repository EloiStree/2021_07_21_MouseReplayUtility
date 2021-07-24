using System;
using System.Collections.Generic;
using System.Linq;

public class MouseRecordSequencePlayingTrack
{
    private List<ActionToDoTime> m_actions;

    public MouseRecordSequencePlayingTrack(MouseRecordSequence sequence)
    {
        this.m_actions = sequence.GetActions().OrderBy(k=>k.m_timeInMilliseconds).ToList() ;
    }
    List<ActionToDoTime> tmp = new List<ActionToDoTime>();
    public List<ActionToDoTime> GetActionsBetween(ulong previousTimeInMs, ulong currentTimeInMs)
    {
        tmp.Clear();
        if (m_actions.Count <= 0)
            return tmp;

        for (int i = 0; i < m_actions.Count; i++)
        {
            if (m_actions[i].m_timeInMilliseconds <= currentTimeInMs)
            {
                tmp.Add(m_actions[i]);
            }
            else {
                break;
            }

        }
        for (int j = tmp.Count-1; j >= 0; j--)
        {
            m_actions.RemoveAt(j);
        }
        return tmp;

    }

    public bool IsItFinishedToPlay()
    {
        return m_actions.Count <= 0;
    }
}