using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ReplayFile : MonoBehaviour
{

    public MouseReplayerThreadMono m_replayer;
    public InputField m_fileToReplay;


    public void Replay() {

        m_replayer.PlayReplayFromFile(m_fileToReplay.text);
    }


    private void Awake()
    {
        m_fileToReplay.text= PlayerPrefs.GetString(m_randomId);
    }
    
    private void OnDestroy()
    {
        PlayerPrefs.SetString(m_randomId, m_fileToReplay.text);
    }

    [ContextMenu("Random Id")]
    public void GenerateRandomId() {
        m_randomId = UnityEngine.Random.value + "" + UnityEngine.Random.value + "" + UnityEngine.Random.value + "" + UnityEngine.Random.value;
    }

   public  string m_randomId;
    public void Reset()
    {
        GenerateRandomId();
    }
}
