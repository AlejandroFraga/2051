using TMPro;
using UnityEngine;

public class FrameRateCounter : MonoBehaviour
{
    /* FIELDS */

    /// Public field that keeps tracks of the text field showing
    public bool IsShowing => m_TextField.gameObject.activeSelf;

    [SerializeField, Tooltip("The text field displaying the frame rate.")]
    TextMeshProUGUI m_TextField = default;

    [SerializeField, Tooltip("The delay in seconds between updates of the displayed frame rate.")]
    float m_PollingTime = 0.5f;

    /// Counter to keep track of the time until reach the polling time
    float m_Time = 0.0f;

    /// Counter to keep track of the frames shown during the polling time
    int m_FrameCount = 0;

    /* METHODS */

    /// Called once per frame.
    void Update()
    {
        // Update time.
        m_Time += Time.deltaTime;

        // Count this frame.
        m_FrameCount++;

        if (m_Time >= m_PollingTime)
        {
            // Update frame rate.
            int frameRate = Mathf.RoundToInt((float)m_FrameCount / m_Time);
            m_TextField.text = frameRate.ToString();

            // Reset time and frame frame count.
            m_Time -= m_PollingTime;
            m_FrameCount = 0;
        }
    }

    /// <summary>
    /// Function to show or hide the text field
    /// </summary>
    /// <param name="show">Show or hide the text field</param>
    public void Show(bool show)
    {
        m_TextField.gameObject.SetActive(show);
    }
}
