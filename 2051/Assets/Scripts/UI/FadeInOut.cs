using System.Collections;
using UnityEngine;


[RequireComponent(typeof(CanvasGroup))]
public class FadeInOut : MonoBehaviour
{
    [Tooltip("Time of the fade in.")]
    public float m_FadeInTime = 3.0f;

    [Tooltip("Time of the fade out.")]
    public float m_FadeOutTime = 3.0f;

    [Tooltip("Index of the button.")]
    public int m_ButtonIndex = default;

    [Tooltip("Automate the fade out.")]
    public bool m_AutomaticFadeOut = default;

    /// <summary>
    /// Game controller to get the index from
    /// </summary>
    FadingCollectionController m_GameController = default;

    /// <summary>
    /// Canvas group to control the alpha value
    /// </summary>
    CanvasGroup m_CanvasGroup = default;

    /// <summary>
    /// The fade out can only start when set to true
    /// </summary>
    bool m_CanFadeOut = default;
 
    
    // Start is called before the first frame update
    void Start()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();

        InitGameController();
    }

    /// <summary>
    /// Init the game controller
    /// </summary>
    void InitGameController()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag(StringHelper.gameControllerTag);
        if (gameController)
        {
            m_GameController = gameController.GetComponent<FadingCollectionController>();
            gameObject.SetActive(m_GameController.GetIndex() == m_ButtonIndex);
        }
    }

    /// <summary>
    /// Fade in the GameObject
    /// </summary>
    public void FadeIn()
    {
        StartCoroutine(IFadeIn());
    }

    /// <summary>
    /// Called from the button.OnClick, fade out the GameObject
    /// </summary>
    public void FadeOut()
    {
        if (m_CanFadeOut)
            StartCoroutine(IFadeOut());
    }

    /// <summary>
    /// Returns the IEnumerator for the fade in function
    /// </summary>
    /// <returns>IEnumerator for the fade in function</returns>
    IEnumerator IFadeIn()
    {
        float counter = 0.0f;

        while (counter < m_FadeInTime)
        {
            counter += Time.deltaTime;
            m_CanvasGroup.alpha = counter / m_FadeInTime;
            yield return null;
        }

        m_CanFadeOut = true;

        if (m_AutomaticFadeOut)
            StartCoroutine(IFadeOut());
    }

    /// <summary>
    /// Returns the IEnumerator for the fade out function
    /// </summary>
    /// <returns>IEnumerator for the fade out function</returns>
    IEnumerator IFadeOut() {

        float counter = 0.0f;

        while (counter < m_FadeOutTime)
        {
            counter += Time.deltaTime;
            m_CanvasGroup.alpha = 1 - (counter / m_FadeInTime);
            yield return null;
        }

        m_GameController.AddIndex();
    }
}
