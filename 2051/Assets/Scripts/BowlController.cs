using UnityEngine;

public class BowlController : MonoBehaviour
{
    public BreadController m_BreadController = default;

    void OnTriggerEnter2D(Collider2D coll)
    {
        m_BreadController.AddIngredient();
    }

    public void OnMouseDown()
    {
        m_BreadController.Knead();
    }
}
