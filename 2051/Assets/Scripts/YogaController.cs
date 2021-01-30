using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YogaController : MonoBehaviour
{
    public Slider m_VideoSlider = default;

    public List<Sprite> m_Postures = default;

    public Image m_VideoPostureImage = default;

    public int m_VideoPosture = 0;

    public Image m_SelectedPostureImage = default;

    public int m_SelectedPosture = 0;

    [Tooltip("Minimum number of postures in the mini-game.")]
    public int m_MinPostures = 3;

    [Tooltip("Maximum number of postures in the mini-game.")]
    public int m_MaxPostures = 5;

    // Number of postures in the mini-game
    private int m_NPostures = default;

    private bool m_Holding = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_VideoPostureImage) return;

        for (int i = 0; i < m_Postures.Count; i++)
        {
            if (m_Postures[i] == m_VideoPostureImage.sprite)
                m_VideoPosture = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Holding)
        {
            m_VideoSlider.value += Time.deltaTime;
            m_VideoSlider.value %= m_VideoSlider.maxValue;
        }
    }

    public void PreviousPosture()
    {
        m_SelectedPosture += m_Postures.Count - 1;
        m_SelectedPosture %= m_Postures.Count;
        m_SelectedPostureImage.sprite = m_Postures[m_SelectedPosture];
    }

    public void NextPosture()
    {
        m_SelectedPosture++;
        m_SelectedPosture %=  m_Postures.Count;
        m_SelectedPostureImage.sprite = m_Postures[m_SelectedPosture];
    }

    public void HoldPostureDown()
    {
        if (m_VideoPosture == m_SelectedPosture)
        {
            m_Holding = true;
        }
    }

    public void HoldPostureUp()
    {
        m_Holding = false;
    }
}
