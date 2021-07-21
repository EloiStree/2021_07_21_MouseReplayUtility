using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win32MouseInterface : MouseInterfaceMono
{

    public int m_xPx, m_yPx;
    public double m_l2rPct, m_t2dPct;
    public uint m_widthResolution, m_heightResolution;

    private void Start()
    {

        m_widthResolution = (uint)Screen.currentResolution.width;
        m_heightResolution = (uint)Screen.currentResolution.height;
    }
    //Win32Mouse
    public override void GetMainScreenDimension(out uint width, out uint height)
    {
        width= m_widthResolution;
        height= m_heightResolution;
    }

    public override void GetMouseButtonInfo(MouseButtonType button, out MouseActionType action)
    {
        action = MouseActionType.Release;
        if (button == MouseButtonType.Left)
        {
            Win32Mouse.GetLeftButtonState(out bool isup);
            action = isup ? MouseActionType.Release : MouseActionType.Press;
        }
        else if (button == MouseButtonType.Middle)
        {
            Win32Mouse.GetMiddleButtonState(out bool isup);
            action = isup ? MouseActionType.Release : MouseActionType.Press;
        }
        else if (button == MouseButtonType.Right)
        {
            Win32Mouse.GetRightButtonState(out bool isup);
            action = isup ? MouseActionType.Release : MouseActionType.Press;
        }
    }

    public override void GetMousePositionFromTopLeftMainScreen(out double pourcentLeft2Right, out double pourcentTop2Down)
    {
        GetMainScreenDimension(out uint w, out uint h);
        GetMousePositionFromTopLeftMainScreenInPixel(out int l2r, out int t2d);

        pourcentLeft2Right = (double)l2r / (double)w;
        pourcentTop2Down = (double)t2d / (double)h;
        m_l2rPct = pourcentLeft2Right;
        m_t2dPct = pourcentTop2Down;
    }
    Win32Mouse.POINT m_pointTmp;
    public override void GetMousePositionFromTopLeftMainScreenInPixel(out int pixelLeft2Right, out int pixelTop2Down)
    {

        Win32Mouse.GetCursorPos(out m_pointTmp);

        pixelLeft2Right =m_xPx= m_pointTmp.x;
        pixelTop2Down = m_yPx = m_pointTmp.y;
    }

    public override void SetMousePositionFromTopLeftMainScreen(double pourcentLeft2Right, double pourcentTop2Down)
    {

        GetMainScreenDimension(out uint w, out uint h);
        SetMousePositionFromTopLeftMainScreenInPixel((int)(pourcentLeft2Right*(double)w), (int)(pourcentTop2Down * (double)h));
    }

    public override void SetMousePositionFromTopLeftMainScreenInPixel(int pixelLeft2Right, int pixelTop2Down)
    {
        Win32Mouse.SetCursorPos(pixelLeft2Right, pixelTop2Down);
    }

    public override void TriggerMouseChangeState(MouseButtonType button, MouseActionType action)
    {
        if (button == MouseButtonType.Left && action == MouseActionType.Press)
            Win32Mouse.SendDownLeft();
        else if (button == MouseButtonType.Left && action == MouseActionType.Release)
            Win32Mouse.SendUpLeft();

        else if (button == MouseButtonType.Middle && action == MouseActionType.Press)
            Win32Mouse.SendDownMiddle();
        else if (button == MouseButtonType.Middle && action == MouseActionType.Release)
            Win32Mouse.SendUpMiddle();

        else if (button == MouseButtonType.Right && action == MouseActionType.Press)
            Win32Mouse.SendDownRight();
        else if (button == MouseButtonType.Right && action == MouseActionType.Release)
            Win32Mouse.SendUpRight();
    }
}
