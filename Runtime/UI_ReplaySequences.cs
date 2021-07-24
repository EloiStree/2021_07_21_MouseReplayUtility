using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UI_ReplaySequences : MonoBehaviour
{

    public MouseReplayerThreadMono m_replayer;
    public UI_ReplayFileStep[] m_replayStepByStep;


    public int GetIndexOf(UI_ReplayFileStep target) {
        for (int i = 0; i < m_replayStepByStep.Length; i++)
        {
            if (target == m_replayStepByStep[i])
                return i;

        }
        return -1;
    }

    public void PlayAll() {

        if (m_replayStepByStep.Length== 0)
            return;
        ReplayFromHere(m_replayStepByStep[0]);
    
    }

    public void ReplayFromHere(UI_ReplayFileStep target)
    {
        MouseRecordSequence fusedSequence = new MouseRecordSequence();
        int index = GetIndexOf(target);
        if (index > -1) {

            for (int i = index; i < m_replayStepByStep.Length; i++)
            {
                Debug.Log("Append " + i+": "+ m_replayStepByStep[i].GetPathOfSequence());
                fusedSequence.AppendOtherSequence(GetSequence( m_replayStepByStep[i]));

            }

        }
        m_replayer.Play(fusedSequence);
    }

    public void ReplayOnlyThis(UI_ReplayFileStep target)
    {

        m_replayer.Play(GetSequence(target));
    }


    public MouseRecordSequence GetSequence(UI_ReplayFileStep sequence) {
        return GetSequence(sequence.GetPathOfSequence());
    }
    public MouseRecordSequence GetSequence(string path) {

        if (!File.Exists(path))
        { 
            return new MouseRecordSequence(); 
        }
        string t = File.ReadAllText(path);
        MouseRecordSequenceToTextStorage.ConvertToRecord(t, out MouseRecordSequence rec);
        return rec;

    }
}

