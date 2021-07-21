using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Win32Mouse;

public class MouseReplayerWithEvent : MonoBehaviour
{

    public POINT m_mousePositionUser32;
    public float m_x, m_y;
    void Start()
    {

        Invoke("Yo", 5);
    }

    public void Yo() {

        Win32Mouse.SetCursorPos(20,80);
    }
    void Update()
    {

        Win32Mouse.GetCursorPos(out  m_mousePositionUser32);
        m_x = m_mousePositionUser32.x;
        m_y = m_mousePositionUser32.y;

    }
}
