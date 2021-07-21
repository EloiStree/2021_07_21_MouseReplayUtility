using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeMouseInterface : MouseInterfaceMono
{


    public Camera m_gameCamera;

    public MouseActionType m_left;
    public MouseActionType m_mid;
    public MouseActionType m_right;



    public Vector3 m_mousePositionInPixel;
    public uint m_screenWidth ;
    public uint m_screenHeight;

    public RectTransform m_fakeMouse;
    public Image m_leftButtonImage;
    public Image m_midButtonImage;
    public Image m_rightButtonImage;


    public int m_l2rPx;
    public double m_l2rPct;
    public int m_t2dPx;
    public double m_t2dPct;


    //private void Update()
    //{
    //    GetMainScreenDimension(out uint a, out uint b);
    //    GetMousePositionFromTopLeftMainScreen(out double aa, out double bb);
    //}


    public override void GetMainScreenDimension(out uint width, out uint height)
    {
        width = m_screenWidth = (uint)m_width;
        height = m_screenHeight = (uint)m_height;
    }

    public override void GetMouseButtonInfo( MouseButtonType button, out MouseActionType action)
    {
        if (button == MouseButtonType.Left)
            action = m_isLeftPressed ? MouseActionType.Press : MouseActionType.Release;

        else if (button == MouseButtonType.Right) 
            action = m_isMiddlePressed ? MouseActionType.Press : MouseActionType.Release;

        else if (button == MouseButtonType.Middle) 
            action = m_isRightPressed ? MouseActionType.Press : MouseActionType.Release;

        else { action =  MouseActionType.Release; }
    }

    public override void GetMousePositionFromTopLeftMainScreen(out double pourcentLeft2Right, out double pourcentTop2Down)
    {
        GetMainScreenDimension(out uint w, out uint h);
        GetMousePositionFromTopLeftMainScreenInPixel(out int l2r, out int t2d);

        pourcentLeft2Right  = (double)l2r / (double)w;
        pourcentTop2Down  = (double)t2d / (double)h;
        m_l2rPct = pourcentLeft2Right;
        m_t2dPct = pourcentTop2Down;
    }

    [Header(" Current State")]
    public Vector3 mousePosition;
    public bool m_isLeftPressed;
    public bool m_isMiddlePressed;
    public bool m_isRightPressed;
    public float m_width;
    public float m_height;

    [Header("Replay")]

    public bool m_replayLeftPressed;
    public bool m_replayMiddlePressed;
    public bool m_replayRightPressed;



    private void Update()
    {
        m_width = Screen.width;
        m_height = Screen.height;
        mousePosition = Input.mousePosition;
        m_isLeftPressed = Input.GetMouseButton(0);
        m_isMiddlePressed = Input.GetMouseButton(1);
        m_isRightPressed = Input.GetMouseButton(2);

        if (m_isNewPositionRequested)
        {

            m_fakeMouse.anchorMin = m_newPositionRequested;
            m_fakeMouse.anchorMax = m_newPositionRequested;
            m_isNewPositionRequested = false;
        }

        RefreshButtonOfFakeMouse();
    }

    public override void GetMousePositionFromTopLeftMainScreenInPixel(out int pixelLeft2Right, out int pixelTop2Down)
    {
        if (m_gameCamera == null) {
            pixelLeft2Right = 0;
            pixelTop2Down = 0;
            return;
        }
        GetMainScreenDimension(out uint w, out uint h);
        m_mousePositionInPixel = mousePosition;
        pixelLeft2Right  = (int)m_mousePositionInPixel.x;
        pixelTop2Down  = ((int)h) - (int)m_mousePositionInPixel.y;
        m_l2rPx = pixelLeft2Right;
        m_t2dPx = pixelTop2Down;
    }

    public bool m_isNewPositionRequested;
    public Vector2 m_newPositionRequested;

    public override void SetMousePositionFromTopLeftMainScreen(double pourcentLeft2Right, double pourcentTop2Down)
    {
        m_isNewPositionRequested = true;
        m_newPositionRequested = new Vector2((float)pourcentLeft2Right, 1f- (float)pourcentTop2Down);
    }

    public override void SetMousePositionFromTopLeftMainScreenInPixel(int pixelLeft2Right, int pixelTop2Down)
    {
        GetMainScreenDimension(out uint width, out uint height);
        SetMousePositionFromTopLeftMainScreen(pixelLeft2Right / (double) width, pixelTop2Down / (double)height);
    }

    public override void TriggerMouseChangeState(MouseButtonType button, MouseActionType action)
    {
        bool isPress = action == MouseActionType.Press;
        if (button == MouseButtonType.Left)
        {
            m_replayLeftPressed = isPress;
        }
        if (button == MouseButtonType.Middle)
        {
            m_replayMiddlePressed = isPress;
        }
        if (button == MouseButtonType.Right)
        {
            m_replayRightPressed = isPress;
        }
    }

    public void RefreshButtonOfFakeMouse() {


        m_leftButtonImage.color = m_replayLeftPressed ? Color.green : Color.white;
        m_midButtonImage.color = m_replayMiddlePressed ? Color.green : Color.white;
        m_rightButtonImage.color = m_replayRightPressed ? Color.green : Color.white;
    }

    


}



public abstract class MouseInterfaceMono : MonoBehaviour, AbstractMouseInterface
{
    public abstract void GetMainScreenDimension(out uint width, out uint height);
    public abstract void GetMouseButtonInfo(MouseButtonType button, out MouseActionType action);
    public abstract void GetMousePositionFromTopLeftMainScreen(out double pourcentLeft2Right, out double pourcentTop2Down);
    public abstract void GetMousePositionFromTopLeftMainScreenInPixel(out int pixelLeft2Right, out int pixelTop2Down);
    public abstract void SetMousePositionFromTopLeftMainScreen(double pourcentLeft2Right, double pourcentTop2Down);
    public abstract void SetMousePositionFromTopLeftMainScreenInPixel(int pixelLeft2Right, int pixelTop2Down);
    public abstract void TriggerMouseChangeState(MouseButtonType button, MouseActionType action);
}