using UnityEngine;

public class OvenController : MonoBehaviour
{
    public BreadController m_BreadController = default;

    void OnTriggerEnter2D(Collider2D coll)
    {
        m_BreadController.InitOven();
    }

    public void OnMouseDown()
    {
        m_BreadController.GetBreadFromOven();
    }
}
