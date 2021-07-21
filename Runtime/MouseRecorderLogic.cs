using System;
using System.Threading;

public class MouseRecorderLogic
{
    public AbstractMouseInterface m_mouseInterface;
    public MouseRecordSequence m_recordSequenceTemp = new MouseRecordSequence();


    public int m_timeBetweenFrameInMs;
    public bool m_recording;

    public RecordType m_recordType = RecordType.RecordEverything;

    public enum RecordType {
        RecordEverything,
        RecordJustTheClickAndRelease
    }

    public int m_l2rPx;
    public int m_t2bPx;
    public MouseActionType m_mouseLeft = MouseActionType.Release;
    public MouseActionType m_mouseMid = MouseActionType.Release;
    public MouseActionType m_mouseRight = MouseActionType.Release;




    public ulong m_timeInMilliseconds;
    DateTime m_startRecording = new DateTime();
    DateTime m_nowTime = new DateTime();


    public void StartRecording()
    {
        m_recordSequenceTemp.ResetToZero();
        m_recording = true;
        m_startRecording = DateTime.Now;
        m_nowTime = DateTime.Now;
    }

    public void StopRecording()
    {

        m_recording = false;
    }



    public void ThreadFunction()
    {
        if (m_mouseInterface==null)
            return;
        if (m_recording)
        {
            m_nowTime = DateTime.Now;
            m_timeInMilliseconds = (ulong)(m_nowTime - m_startRecording).TotalMilliseconds;
            if (m_recordType == RecordType.RecordEverything)
            {
                RecordMousePositionIfMoved();
            }
            RecordMouseClickIfChanged();
        }
        Thread.Sleep(m_timeBetweenFrameInMs);
    }

    public void SetMillisecondsBetweenRecord(uint milliseconds)
    {
        m_timeBetweenFrameInMs =(int) milliseconds;
    }

    public void SetRecordType(RecordType recordEverything)
    {
        m_recordType = recordEverything;
    }

    public uint GetActionCount()
    {
        return m_recordSequenceTemp.GetCount();
    }

    private void RecordMouseClickIfChanged()
    {
        m_mouseInterface.GetMouseButtonInfo(MouseButtonType.Left, out MouseActionType l);
        if (m_mouseLeft != l) {

            RecordMousePositionIfMoved();
            NotifyChanged(MouseButtonType.Left, l);
            m_mouseLeft = l;

        }

        m_mouseInterface.GetMouseButtonInfo(MouseButtonType.Middle, out MouseActionType m);
        if (m_mouseMid != m)
        {
            RecordMousePositionIfMoved();
            NotifyChanged(MouseButtonType.Middle, m);
            m_mouseMid = m;
        }

        m_mouseInterface.GetMouseButtonInfo(MouseButtonType.Right, out MouseActionType r);
        if (m_mouseRight != r)
        {
            RecordMousePositionIfMoved();
            NotifyChanged(MouseButtonType.Right, r);
            m_mouseRight = r;
        }
    }

    private void NotifyChanged(MouseButtonType button, MouseActionType action, ulong additionalDelay=10)
    {
        m_recordSequenceTemp.AddMoveChangeOfStateKey(m_timeInMilliseconds + additionalDelay, button, action);
    }

    private void RecordMousePositionIfMoved()
    {
        
            m_mouseInterface.GetMousePositionFromTopLeftMainScreenInPixel(out int l2rpx, out int t2dpx);
            if (IsNewCoordinate(ref l2rpx, ref t2dpx))
            {
                RecordMousePosition(l2rpx, t2dpx);
            }
        
    }

    private void RecordMousePosition(int l2rpx, int t2dpx)
    {
        m_mouseInterface.GetMousePositionFromTopLeftMainScreen(out double l2rPct, out double t2dPct);
        m_l2rPx = l2rpx;
        m_t2bPx = t2dpx;
        m_recordSequenceTemp.AddMoveMouseKey((ulong)m_timeInMilliseconds, l2rPct, t2dPct);
    }

    private bool IsNewCoordinate(ref int l2r, ref int t2d)
    {
        return l2r != m_l2rPx || t2d != m_t2bPx;
    }

}
