using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_RecordLastReplayRecord : MonoBehaviour
{

    public MouseReplayerThreadMono m_replayer;
    public InputField m_directoryPath;
    public InputField m_fileName;
    public string fileExtension = ".mousereplay";


    public void Awake()
    {
        m_directoryPath.text = Directory.GetCurrentDirectory();
        m_fileName.text = "lastrecord";
    }

    public void SaveLastReplay()
    {
        m_replayer.SaveAsReplayAsFileTo(m_directoryPath.text, m_fileName.text, fileExtension);

    }
    public void SaveLastReplayWithFileName(string fileName)
    {
        m_replayer.SaveAsReplayAsFileTo(m_directoryPath.text, fileName, fileExtension);
    }

    public void OpenRecordDirectory()
    {
        m_replayer.OpenDefaultRecordDirectory();
    }
   
}
