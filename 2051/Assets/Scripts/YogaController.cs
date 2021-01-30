using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YogaController : MonoBehaviour
{

    public List<Sprite> m_Postures = default;

    public Image m_VideoPostureImage = default;

    public int m_VideoPosture = 0;

    public Image m_SelectedPostureImage = default;

    public int m_SelectedPosture = 0;

    [Tooltip("Minimum number of postures in the mini-game.")]
    public int m_MinPostures = 3;

    [Tooltip("Maximum number of postures in the mini-game.")]
    public int m_MaxPostures = 5;

    /// Number of postures in the mini-game
    private int m_NPostures = default;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectNumberOfPostures()
    {
        // Select a rand number between min and max

    }

    public void PreviousPosture()
    {
        m_SelectedPosture--;
        m_SelectedPosture %= m_Postures.Count;
        m_SelectedPostureImage.sprite = m_Postures[m_SelectedPosture];
    }

    public void NextPosture()
    {
        m_SelectedPosture++;
        m_SelectedPosture %= m_Postures.Count;
        m_SelectedPostureImage.sprite = m_Postures[m_SelectedPosture];
    }

    public void HoldPosture()
    {

    }
}
