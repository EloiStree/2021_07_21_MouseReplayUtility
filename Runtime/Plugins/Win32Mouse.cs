using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Win32Mouse
{
    [DllImport("User32.Dll")]
    public static extern long SetCursorPos(int x, int y);

   
    [DllImport("User32.Dll")]
    public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);
    [DllImport("User32.Dll")]
    public static extern bool GetCursorPos(out POINT lpPoint);

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;

        public POINT(int X, int Y)
        {
            x = X;
            y = Y;
        }
    }

    [DllImport("user32.dll", SetLastError = false)] 
    public static extern IntPtr GetDesktopWindow();


    [DllImport("user32.dll")]
    private static extern void mouse_event(UInt32 dwFlags, UInt32 dx, UInt32 dy, UInt32 dwData, IntPtr dwExtraInfo);
    //found at http://msdn.microsoft.com/en-us/library/ms646273(v=vs.85).aspx
    private const UInt32 MOUSEEVENTF_LEFTDOWN = 0x0002;  //The left button was pressed
    private const UInt32 MOUSEEVENTF_LEFTUP = 0x0004;  //The left button was released.
    private const UInt32 MOUSEEVENTF_RIGHTDOWN = 0x08;   //The right button was pressed
    private const UInt32 MOUSEEVENTF_RIGHTUP = 0x10;   //The left button was released.
    private const UInt32 MOUSEEVENTF_MIDDLEDOWN = 0x0020;  //The middle button was pressed
    private const UInt32 MOUSEEVENTF_MIDDLEUP = 0x0040;  //The middle button was released.


    const uint MOUSEEVENTF_WHEEL = 0x0800;
    const uint MOUSEEVENTF_HWHEEL = 0x01000;


    [DllImport("user32.dll")]
    public static extern short GetAsyncKeyState(UInt16 virtualKeyCode);
    //found at http://msdn.microsoft.com/en-us/library/dd375731(v=VS.85).aspx

    //Virtual key codes
    private const UInt16 VK_MBUTTON = 0x04;//middle mouse button
    private const UInt16  VK_LBUTTON = 0x01;//left mouse button
    private const UInt16 VK_RBUTTON = 0x02;//right mouse button


    public static void SendUpLeft() => mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, new System.IntPtr());
    public static void SendUpMiddle() => mouse_event(MOUSEEVENTF_MIDDLEUP, 0, 0, 0, new System.IntPtr());
    public static void SendUpRight() => mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, new System.IntPtr());


    ///<summary>

    /// Send the event Down for the Right Mouse Button

    ///</summary>

    public static void SendDownLeft() => mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, new System.IntPtr());
    public static void SendDownMiddle() => mouse_event(MOUSEEVENTF_MIDDLEDOWN, 0, 0, 0, new System.IntPtr());
    public static void SendDownRight() => mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, new System.IntPtr());



    public static void GetLeftButtonState(out bool isUp) => isUp = GetAsyncKeyState(VK_LBUTTON) == 0;
    public static void GetMiddleButtonState(out bool isUp) => isUp = GetAsyncKeyState(VK_MBUTTON) == 0;
    public static void GetRightButtonState(out bool isUp) => isUp = GetAsyncKeyState(VK_RBUTTON) == 0;


}