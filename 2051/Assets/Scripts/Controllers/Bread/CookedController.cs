using UnityEngine;


public class CookedController : MonoBehaviour
{
    /// <summary>
    /// The bread mini-game controller
    /// </summary>
    BreadController m_BreadController = default;


    // Awake is called when the script instance is being loaded
    public void Start()
    {
        InitGameController();
    }

    /// <summary>
    /// Init the game controller
    /// </summary>
    void InitGameController()
    {
        GameObject gameController = GameObject.FindGameObjectWithTag(StringHelper.gameControllerTag);
        if (gameController)
            m_BreadController = gameController.GetComponent<BreadController>();
    }

    // On mouse clicked, get the bread done
    public void OnMouseDown()
    {
        if (m_BreadController)
            m_BreadController.GetBreadDone();
    }
}
