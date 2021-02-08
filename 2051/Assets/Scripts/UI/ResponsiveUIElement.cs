using UnityEngine;

[ExecuteInEditMode]
public class ResponsiveUIElement : MonoBehaviour
{
    Vector2 m_LastWindowSize = default;

    public float m_PosX = default;

    public float m_Width = default;

    static float m_DesiredAspectRatio = (608.0f / 1080.0f);

    bool m_GreaterAspectRatio = false;

    RectTransform m_RectTransform = default;

    static readonly Vector2 m_LowerAnchorMin = new Vector2(0.5f, 0.0f);

    static readonly Vector2 m_LowerAnchorMax = new Vector2(0.5f, 1.0f);

    void Awake()
    {
        Start();
    }

    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        UpdateByAspectRatio();
    }

    void UpdateByAspectRatio(bool forceUpdate = false)
    {
        Vector2 windowSize = new Vector2(Screen.width, Screen.height);

        if ((windowSize != m_LastWindowSize) || forceUpdate)
        {
            m_LastWindowSize = windowSize;

            m_GreaterAspectRatio = CurrentAspectRatio() > m_DesiredAspectRatio;

            UpdateRectTransform();
        }
    }

    void UpdateRectTransform()
    {
        if (m_GreaterAspectRatio)
        {
            m_RectTransform.anchorMin = m_LowerAnchorMin;
            m_RectTransform.anchorMax = m_LowerAnchorMax;

            if(m_Width != 0)
                RectTransformExtensions.SetWidth(m_RectTransform, m_Width);

            RectTransformExtensions.SetLocalPositionXIfNotZero(m_RectTransform, m_PosX);
        }
        else
        {
            m_RectTransform.anchorMin = Vector2.zero;
            m_RectTransform.anchorMax = Vector2.one;

            if (m_Width != 0)
                RectTransformExtensions.SetWidth(m_RectTransform, m_Width * AspectRatioRel());

            RectTransformExtensions.SetLocalPositionXIfNotZero(m_RectTransform, m_PosX * AspectRatioRel());
        }
    }

    static float CurrentAspectRatio()
    {
        return (float) Screen.width / (float) Screen.height;
    }

    static float AspectRatioRel()
    {
        return CurrentAspectRatio() / m_DesiredAspectRatio;
    }
}
