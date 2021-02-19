using UnityEngine;


public class BowlController : MonoBehaviour
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
    /// On trigger enter, add the ingredient
    /// </summary>
    /// <param name="collider">Collider that entered the trigger</param>
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (m_BreadController)
            m_BreadController.AddIngredient();
    }

    /// <summary>
    /// On mouse clicked, knead the dough
    /// </summary>
    void OnMouseDown()
    {
        if (m_BreadController)
            m_BreadController.Knead();
    }
}
