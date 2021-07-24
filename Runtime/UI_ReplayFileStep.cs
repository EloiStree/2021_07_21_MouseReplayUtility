using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_ReplayFileStep : MonoBehaviour
{

    public UI_ReplaySequences m_sequencesGroup;
    public InputField m_fileToReplay;


    public void ReplayThisOneOnly()
    {
        m_sequencesGroup.ReplayOnlyThis(this);

    }
    public void ReplayFromHere()
    {

        m_sequencesGroup.ReplayFromHere(this);
    }


    private void Awake()
    {
        m_fileToReplay.text = PlayerPrefs.GetString(m_randomId);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetString(m_randomId, m_fileToReplay.text);
    }

    internal string GetPathOfSequence()
    {
        return m_fileToReplay.text;
    }
    [ContextMenu("Random Id")]
    public void GenerateRandomId()
    {
        m_randomId = UnityEngine.Random.value + "" + UnityEngine.Random.value + "" + UnityEngine.Random.value + "" + UnityEngine.Random.value;
    }

    public string m_randomId;
    public void Reset()
    {
        GenerateRandomId();
    }
}
