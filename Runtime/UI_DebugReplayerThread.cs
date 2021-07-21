using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DebugReplayerThread : MonoBehaviour
{

    public MouseReplayerThreadMono m_replayThread;

    public Text m_threadMode;
    
    [Header("Record")]
    public Text m_sequenceActionCountRecord;
    public Text m_timePastRecord;

    [Header("Replay")]
    public Text m_sequenceActionCountReplay;
    public Text m_timePastReplay;

    void Start()
    {

        InvokeRepeating("Refresh", 0, 0.2f);
        
    }

    void Refresh()
    {
        m_threadMode.text = m_replayThread.GetReplayerMode().ToString();

        m_sequenceActionCountRecord.text = m_replayThread.GetSequenceActionRecordCount().ToString();
        m_timePastRecord.text = m_replayThread.GetTimePastRecord().ToString();

        m_sequenceActionCountReplay.text = m_replayThread.GetSequenceActionReplayCount().ToString();
        m_timePastReplay.text = m_replayThread.GetTimePastReplay().ToString();


    }

    public void SetMillisecondsBetweenRecord(string milliseconds)
    {
        if(uint.TryParse(milliseconds, out uint ms))
             m_replayThread.GetRecorder().SetMillisecondsBetweenRecord(ms);
    }

    public void SetMillisecondsBetweenRecord(int milliseconds)
    {
        m_replayThread.GetRecorder().SetMillisecondsBetweenRecord( (uint) milliseconds);
    }

    public void SetRecordTypeFromDropdown(int dropdownId)
    {

        if (dropdownId == 0)
            m_replayThread.GetRecorder().SetRecordType(MouseRecorderLogic.RecordType.RecordEverything);
        if (dropdownId == 1)
            m_replayThread.GetRecorder().SetRecordType(MouseRecorderLogic.RecordType.RecordJustTheClickAndRelease);
    }


}


