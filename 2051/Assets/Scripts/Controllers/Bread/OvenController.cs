using UnityEngine;


public class OvenController : MonoBehaviour
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

    /// <summary>
    /// On trigger enter, start the oven
    /// </summary>
    /// <param name="collider">Collider that entered the trigger</param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_BreadController)
            m_BreadController.InitOven();
    }

    /// <summary>
    /// On mouse clicked, get the bread from the oven
    /// </summary>
    public void OnMouseDown()
    {
        if (m_BreadController)
            m_BreadController.GetBreadFromOven();
    }
}
