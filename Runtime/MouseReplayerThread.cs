using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MouseReplayerThread
{
    public AbstractMouseInterface m_mouseInterface;
    public Thread m_thread;
    public bool m_stopRequested;


    public enum ThreadMode { Recording, Playing, None }
    public ThreadMode m_currentThreadMode = ThreadMode.None;
    public MouseRecorderLogic m_recorder = new MouseRecorderLogic();
    public MouseReplayingLogic m_replaying = new MouseReplayingLogic();




    public void SetMouseInterface(AbstractMouseInterface mouseInterface)
    {

        m_recorder.m_mouseInterface = mouseInterface;
        m_replaying.m_mouseInterface = mouseInterface;
    }


    public void Start(System.Threading.ThreadPriority priority)
    {
        m_stopRequested = false;
        if (m_thread == null)
        {

            m_thread = new Thread(ThreadFunction);
            m_thread.Priority = priority;
            m_thread.Start();
        }
    }

    public uint GetSequenceActionRecordCount()
    {
       return  m_recorder.GetActionCount();
    }

    public uint GetSequenceActionReplayCount()
    {
        return m_replaying.GetActionCount();
    }

    public void StartRecording()
    {

        StopCurrentOperation();
        m_currentThreadMode = ThreadMode.Recording;
        m_recorder.StartRecording();

    }

    private void StopCurrentOperation()
    {
        m_recorder.StopRecording();
        m_replaying.StopReplaying();
    }

    internal MouseRecorderLogic GetRecorder()
    {
        return m_recorder;
    }
    internal MouseReplayingLogic GetReplayer()
    {
        return m_replaying;
    }

    public void StopRecording()
    {

        m_recorder.StopRecording();
        m_currentThreadMode = ThreadMode.None;
    }


    private void ThreadFunction(object obj)
    {


        while (!m_stopRequested)
        {


            if (m_currentThreadMode == ThreadMode.Recording)
            {
                m_recorder.ThreadFunction();

            }
            else if (m_currentThreadMode == ThreadMode.Playing)
            {

                m_replaying.ThreadFunction();
            }
            else
            {
                //Do Nothing And Wait
                Thread.Sleep(100);

            }



            Thread.Sleep(1);
        }
    }




    public void StopThread()
    {
        if (m_thread != null)
        {
            m_stopRequested = true;
            m_thread.Abort();
            m_thread = null;
        }
    }

    public void StopRecordingOrPlaying()
    {
        m_recorder.StopRecording();
        m_replaying.StopReplaying();
        m_currentThreadMode = ThreadMode.None;
    }

    public string GetSequenceAsText()
    {
        MouseRecordSequenceToTextStorage.ConvertToText(m_recorder.m_recordSequenceTemp, out string t);
        return t;
    }

    public void PlayReplaySequence(MouseRecordSequence record)
    {
        StopRecordingOrPlaying();
        m_replaying.StartPlaying(record);
        m_currentThreadMode = ThreadMode.Playing;
    }

    public ThreadMode GetReplayerMode()
    {
        return m_currentThreadMode;
    }

    public double GetTimePast()
    {
        if (m_currentThreadMode == MouseReplayerThread.ThreadMode.Playing)
            return m_replaying.m_currentTimeInMs;
        if (m_currentThreadMode == MouseReplayerThread.ThreadMode.Recording)
            return m_recorder.m_timeInMilliseconds;
        return 0;
    }

    public uint GetSequenceActionCount()
    {
        return m_recorder.m_recordSequenceTemp.GetCount();
    }
}

