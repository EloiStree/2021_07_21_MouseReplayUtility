using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_DelayButton : MonoBehaviour
{

    public float m_timeCountdownUsed = 5;
    public float m_timeCountdown = 5;
    public Text m_timeLeft;

    public UnityEvent m_doAfterTime;


    public bool m_isActive;

    private void Awake()
    {
        m_timeLeft.text = ""+ m_timeCountdown;
    }

    private void Update()
    {
        if (m_isActive) { 
            m_timeCountdown -= Time.deltaTime;
            
        }
        if (m_timeCountdown < 0) { 
            m_timeCountdown = m_timeCountdownUsed;
            m_isActive = false;
            m_doAfterTime.Invoke();
        }

        m_timeLeft.text = string.Format("{0:0}", m_timeCountdown);
    }


    public void StartCountdown() {

        m_timeCountdown = m_timeCountdownUsed;
        m_isActive = true;
    }
}
