﻿using UnityEngine;


[ExecuteInEditMode, RequireComponent(typeof(RectTransform))]
public class ResponsiveUIElement : MonoBehaviour
{
    [Tooltip("Position on the x-axis of the UI element.")]
    public float m_PosX = default;

    [Tooltip("Width of the UI element.")]
    public float m_Width = default;

    [Tooltip("Desired aspect ratio of the UI element.")]
    static float m_DesiredAspectRatio = (1080.0f / 1920.0f);

    /// <summary>
    /// Rect transform component of the game object
    /// </summary>
    RectTransform m_RectTransform = default;

    /// <summary>
    /// Anchor min when the aspect ratio is lower than the desired
    /// </summary>
    static readonly Vector2 m_LowerAnchorMin = new Vector2(0.5f, 0.0f);

    /// <summary>
    /// Anchor max when the aspect ratio is lower than the desired
    /// </summary>
    static readonly Vector2 m_LowerAnchorMax = new Vector2(0.5f, 1.0f);


    // Awake is called when the script instance is being loaded
    void Awake()
    {
        Start();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        UpdateByAspectRatio();
    }

    // Update is called once per frame
    void Update()
    {
        // If the width is negative, something went wrong, so we force the update
        UpdateByAspectRatio();
    }

    /// <summary>
    /// Updates the rect transform by the new aspect ratio
    /// </summary>
    void UpdateByAspectRatio()
    {
        if (CurrentAspectRatio() > m_DesiredAspectRatio)
            UpdateByGreaterAspectRatio();

        else
            UpdateByLowerAspectRatio();
    }

    /// <summary>
    /// Updates the rect transform by the new aspect ratio being greater than the desired
    /// </summary>
    void UpdateByGreaterAspectRatio()
    {
        m_RectTransform.anchorMin = m_LowerAnchorMin;
        m_RectTransform.anchorMax = m_LowerAnchorMax;

        if (m_Width > 0)
            GameObjectHelper.SetWidth(m_RectTransform, m_Width);

        GameObjectHelper.SetLocalPositionX(m_RectTransform, m_PosX);
    }

    /// <summary>
    /// Updates the rect transform by the new aspect ratio being lower than the desired
    /// </summary>
    void UpdateByLowerAspectRatio()
    {
        m_RectTransform.anchorMin = Vector2.zero;
        m_RectTransform.anchorMax = Vector2.one;

        float aspectRatioRel = AspectRatioRel();

        if (m_Width > 0)
            GameObjectHelper.SetWidth(m_RectTransform, m_Width * aspectRatioRel);

        GameObjectHelper.SetLocalPositionX(m_RectTransform, m_PosX * aspectRatioRel);
    }

    /// <summary>
    /// Returns the current aspect ratio of the window
    /// </summary>
    /// <returns></returns>
    static float CurrentAspectRatio()
    {
        return (float) Screen.width / (float) (Screen.height != 0 ? Screen.height : 1);
    }

    /// <summary>
    /// Returns the current aspect ratio of the window divided by the desired aspect ratio
    /// </summary>
    /// <returns></returns>
    static float AspectRatioRel()
    {
        float result = CurrentAspectRatio() / (m_DesiredAspectRatio != 0.0f ? m_DesiredAspectRatio : 1);
        return result > 0 ? result : 1;
    }
}
