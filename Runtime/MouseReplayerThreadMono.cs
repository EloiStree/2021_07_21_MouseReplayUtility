using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MouseReplayerThreadMono : MonoBehaviour
{

    public MouseInterfaceMono m_defaultMouseInterface;

    internal void PlayReplayFromFile(string path)
    {
        if (File.Exists(path)) {

            string t = File.ReadAllText(path);
            MouseRecordSequenceToTextStorage.ConvertToRecord(t, out MouseRecordSequence seq);
            m_thread.PlayReplaySequence(seq);
        }
    }

    public MouseReplayerThread m_thread = new MouseReplayerThread();
    public bool m_startThreadAtAwake;


    public UnityEvent m_onStart;
    public UnityEvent m_onStartReplay;
    public UnityEvent m_onStartRecord;
    public UnityEvent m_onStop;

    public uint GetSequenceActionCount()
    {
        return m_thread.GetSequenceActionCount();
    }

    public MouseReplayerThread.ThreadMode GetReplayerMode()
    {
        return m_thread.GetReplayerMode();
    }

    public double GetTimePast()
    {
        return m_thread.GetTimePast();
    }

    public void OpenDefaultRecordDirectory()
    {
        Application.OpenURL(Directory.GetCurrentDirectory());
    }

    internal uint GetSequenceActionRecordCount()
    {
       return m_thread.GetSequenceActionRecordCount();
    }

    public double GetTimePastRecord()
    {
        return m_thread.m_recorder.m_timeInMilliseconds;
    }

    internal uint GetSequenceActionReplayCount()
    {
        return m_thread.GetSequenceActionReplayCount();
    }

    public double GetTimePastReplay()
    {
        return m_thread.m_replaying.m_currentTimeInMs;
    }

    internal void SaveAsReplayAsFileTo(string directoryPath, string fileName, string fileExtension)
    {
        if(Directory.Exists(directoryPath))
        File.WriteAllText(directoryPath + "/" + fileName+"."+ fileExtension, m_thread.GetSequenceAsText());
    }

    public MouseRecorderLogic GetRecorder()
    {
        return m_thread.GetRecorder();
    }
    public MouseReplayingLogic GetReplayer()
    {
        return m_thread.GetReplayer() ;
    }


    public void AddMouseEventListener(MouseReplayingLogic.TextEventEmission listener)
    {
        m_thread.m_replaying.m_listenToTextEmittedByReplay += listener;
    }
    public void RemoveMouseEventListener(MouseReplayingLogic.TextEventEmission listener)
    {
        m_thread.m_replaying.m_listenToTextEmittedByReplay -= listener;
    }


    public MouseRecorderLogic.RecordType m_defautlRecordType;
    public void SetMouseInterface(AbstractMouseInterface mouseInteface)
    {
        m_thread.SetMouseInterface(mouseInteface);
    }

    public System.Threading.ThreadPriority m_priority;

    public float m_delayBetweenFrameRecord=200;

    public void ReplayRecord() {

        m_thread.StopRecordingOrPlaying();
        string sequence= m_thread.GetSequenceAsText();
       MouseRecordSequenceToTextStorage.ConvertToRecord(sequence, out MouseRecordSequence record);
        m_thread.PlayReplaySequence(record);
        m_onStartReplay.Invoke();
    }

    public void PlaySequence(MouseRecordSequence sequence) {


        m_onStartReplay.Invoke();
    }

    public void StartRecording()
    {
        m_thread.StopRecordingOrPlaying();
        m_thread.StartRecording();
        m_onStartRecord.Invoke();

    }
   
    public void StopRecording()
    {

        m_thread.StopRecordingOrPlaying();
        m_onStop.Invoke();

    }

    internal void SaveAsFileNearExe(string relativePath)
    {
        string path = Directory.GetCurrentDirectory() + "/" + relativePath;
        string dir = Path.GetDirectoryName(path);
        Directory.CreateDirectory(dir);

        File.WriteAllText(path, m_thread.GetSequenceAsText()) ;
        ;
    }

    public void StopRecordingOrPlaying() {

        m_thread.StopRecordingOrPlaying();
        m_onStop.Invoke();
    }
    public void KillTheThread() {
        m_thread.StopThread();
    }
    public void FlushAll() { 
    
    }

    void Awake()
    {
        m_thread.GetRecorder().SetRecordType(m_defautlRecordType);
        m_thread.SetMouseInterface(m_defaultMouseInterface);
        if(m_startThreadAtAwake)
            m_thread.Start(m_priority);
    }

    public MouseRecordSequence GetSequenceAsReference()
    {
        return m_thread.m_recorder.m_recordSequenceTemp;
    }

    public MouseRecordSequence GetSequenceAsCopy()
    {

        MouseRecordSequenceToTextStorage.ConvertToText(GetSequenceAsReference(), out string text);
        MouseRecordSequenceToTextStorage.ConvertToRecord(text, out MouseRecordSequence recCopy);
        return recCopy;
    }

    public void Play(MouseRecordSequence sequence)
    {
        m_thread.m_replaying.StartPlaying( sequence);
        m_onStartReplay.Invoke();
    }

    private void OnDestroy()
    {
        m_thread.StopThread();

    }
    private void OnApplicationQuit()
    {
        m_thread.StopThread();

    }
}
public class ActionToDoTime {


    public ulong m_timeInMilliseconds;

    public ActionToDoTime(ulong when)
    {
        m_timeInMilliseconds = when;
    }
}

public class ActionToDo_MoveAt : ActionToDoTime
{
    public double m_pourcentLeft2Right;
    public double m_pourcentTop2Down;

    public ActionToDo_MoveAt(ulong timeInMilliseconds, double pourcentLeft2Right, double pourcentTop2Down):base(timeInMilliseconds)
    {
        m_pourcentLeft2Right = pourcentLeft2Right;
        m_pourcentTop2Down = pourcentTop2Down;
    }
}
public class ActionToDo_MouseClick : ActionToDoTime
{

    public MouseButtonType m_mouseButton;
    public MouseActionType m_actionType;

    public ActionToDo_MouseClick(ulong timeInMilliseconds, MouseButtonType mouseButton, MouseActionType actionType) : base(timeInMilliseconds)
    {
        m_mouseButton = mouseButton;
        m_actionType = actionType;
    }
}
public class ActionToDo_TextEventRelease : ActionToDoTime
{
    public string m_textEventValue;

    public ActionToDo_TextEventRelease(ulong timeInMilliseconds, string textEventValue) : base(timeInMilliseconds)
    {
        m_textEventValue = textEventValue;
    }
}

public interface AbstractMouseInterface {

    public void GetMousePositionFromTopLeftMainScreen(out double pourcentLeft2Right, out double pourcentTop2Down);
    public void SetMousePositionFromTopLeftMainScreen(double pourcentLeft2Right, double pourcentTop2Down);

    public void GetMousePositionFromTopLeftMainScreenInPixel(out int pixelLeft2Right, out int pixelTop2Down);
    public void SetMousePositionFromTopLeftMainScreenInPixel(int pixelLeft2Right, int pixelTop2Down);

    public void GetMainScreenDimension(out uint width, out uint height);

    public void GetMouseButtonInfo( MouseButtonType button, out MouseActionType action);
    public void TriggerMouseChangeState(MouseButtonType button, MouseActionType action);
}

public enum MouseButtonType { Left, Middle, Right }
public enum MouseActionType { Press, Release }




public class MouseRecordSequence {


    public List<ActionToDoTime> m_actionRecorded = new List<ActionToDoTime>();


    public void AddMoveMouseKey(ulong timeInMilliseconds, double left2RightPourcentOnScreen, double top2DownPourcentOnScreen)
    {
        m_actionRecorded.Add(new ActionToDo_MoveAt(timeInMilliseconds, left2RightPourcentOnScreen, top2DownPourcentOnScreen));
    }

    public void AddMoveChangeOfStateKey(ulong timeInMilliseconds, MouseButtonType button, MouseActionType action, double left2RightPourcentOnScreen, double top2DownPourcentOnScreen)
    {
        m_actionRecorded.Add(new ActionToDo_MoveAt(timeInMilliseconds, left2RightPourcentOnScreen, top2DownPourcentOnScreen));
        m_actionRecorded.Add(new ActionToDo_MouseClick(timeInMilliseconds, button, action));
    }
    public void AddMoveChangeOfStateKey(ulong timeInMilliseconds, MouseButtonType button, MouseActionType action, double left2RightPourcentOnScreen, double top2DownPourcentOnScreen, ulong addDelayBeforeClickingInMs=20)
    {
        m_actionRecorded.Add(new ActionToDo_MoveAt(timeInMilliseconds, left2RightPourcentOnScreen, top2DownPourcentOnScreen));
        m_actionRecorded.Add(new ActionToDo_MouseClick(timeInMilliseconds + addDelayBeforeClickingInMs, button, action));
    }

    public void AddMoveChangeOfStateKey(ulong timeInMilliseconds, MouseButtonType button, MouseActionType action)
    {
        m_actionRecorded.Add(new ActionToDo_MouseClick(timeInMilliseconds, button, action));
    }
    public void AddTextEvent(ulong timeInMilliseconds, string text)
    {
        m_actionRecorded.Add(new ActionToDo_TextEventRelease(timeInMilliseconds, text));
    }


    public void CheckThatItIsSortByTime() {

        m_actionRecorded.OrderBy(k => k.m_timeInMilliseconds);
    }

    public void ResetToZero()
    {
        m_actionRecorded.Clear();
    }

    public ActionToDoTime[] GetActionsBetween(ulong previousTimeInMs, ulong currentTimeInMs)
    {
        //COULD BE MORE EFFICIENT but i coded that quickly to check that it works.
        return m_actionRecorded.Where(k => k.m_timeInMilliseconds > previousTimeInMs && k.m_timeInMilliseconds <= currentTimeInMs).ToArray();
    }
    public ActionToDoTime[] GetActionsAfter( ulong currentTimeInMs)
    {
        //COULD BE MORE EFFICIENT but i coded that quickly to check that it works.
        return m_actionRecorded.Where(k => k.m_timeInMilliseconds > currentTimeInMs).ToArray();
    }

    public bool IsThereSomeLeftAfter(ulong currentTimeInMs)
    {
        return GetActionsAfter(currentTimeInMs).Length > 0;
    }

    public IEnumerable<ActionToDoTime> GetAllActionsSortedByTime()
    {
       return  m_actionRecorded.OrderBy(k=>k.m_timeInMilliseconds);
    }

    public uint GetCount()
    {
        return (uint) m_actionRecorded.Count;
    }
}

public class MouseRecordSequenceToTextStorage {

    public static void ConvertToText(MouseRecordSequence record, out string textToStore) {

        StringBuilder sb = new StringBuilder();

        foreach (ActionToDoTime action in record.GetAllActionsSortedByTime())
        {
            if (action is ActionToDo_MouseClick)
            {
                ActionToDo_MouseClick tmp = (ActionToDo_MouseClick)action;
                sb.Append(string.Format("C|{0:0}|{1}|{2}\n", tmp.m_timeInMilliseconds, tmp.m_mouseButton, tmp.m_actionType));
            }
            if (action is ActionToDo_MoveAt)
            {
                ActionToDo_MoveAt tmp = (ActionToDo_MoveAt)action;
                sb.Append(string.Format("M|{0:0}|{1}|{2}\n", tmp.m_timeInMilliseconds, tmp.m_pourcentLeft2Right, tmp.m_pourcentTop2Down));
            }
            if (action is ActionToDo_TextEventRelease)
            {
                ActionToDo_TextEventRelease tmp = (ActionToDo_TextEventRelease)action;
                sb.Append(string.Format("T|{0:0}|{1}\n", tmp.m_timeInMilliseconds, tmp.m_textEventValue));
            }

        }

        textToStore = sb.ToString();
    }
    public static void ConvertToRecord(string textStored, out MouseRecordSequence record) {

        record = new MouseRecordSequence();
        string [] lines = textStored.Split('\n');
        foreach (string l in lines)
        {
            if (l.ToLower().IndexOf("c|") == 0)
            {
                string[] tokens = l.Split('|');
                if (tokens.Length == 4)
                {
                    if (ulong.TryParse(tokens[1], out ulong total))
                    {
                        if (IsValideButton(tokens[2], out MouseButtonType button)
                            && IsValideButtonAction(tokens[3], out MouseActionType buttonaction))
                        {
                            record.AddMoveChangeOfStateKey(total, button, buttonaction);
                        }

                    }
                }
            }
            if (l.ToLower().IndexOf("m|") == 0)
            {
                
                string[] tokens = l.Split('|');
                if (tokens.Length == 4) {
                    if (ulong.TryParse(tokens[1], out ulong total) &&
                        double.TryParse(tokens[2], out double l2r) &&
                        double.TryParse(tokens[3], out double t2d) )
                    {
                        record.AddMoveMouseKey(total, l2r, t2d);
                    }
                }
            }
            if (l.ToLower().IndexOf("t|") == 0)
            {
                string t = l.Substring(2);
                int index = t.IndexOf("|");
                if (index > -1) {
                   string text=   t.Substring(index + 1);
                   string number = t.Substring(0, index);
                    if (ulong.TryParse(number, out ulong total)) { 
                        record.AddTextEvent(total, text);
                    }
                }
            }

        }

    }

    private static bool IsValideButton(string text, out MouseButtonType button)
    {
        button = MouseButtonType.Left;
           text = text.ToLower().Trim();
        if (text == "l" || text == "left") {     
            button = MouseButtonType.Left;

            return true;
        }
        if (text == "r" || text == "right")
        { 
            button = MouseButtonType.Right;

            return true;
        }
        if (text == "m" || text == "mid" || text == "middle" || text == "center" || text == "c")
        { 
            button = MouseButtonType.Middle;

            return true;
        }

        return false;
    }

    private static bool IsValideButtonAction(string text, out MouseActionType buttonaction)
    {
        buttonaction = MouseActionType.Press;
        text = text.ToLower().Trim();
        if (text == "p" || text == "press")
        { 
            buttonaction = MouseActionType.Press;
            return true;
        }
        if (text == "r" || text == "release")
        { 
            buttonaction = MouseActionType.Release;
            return true;
        }
        return false;

    }
}
