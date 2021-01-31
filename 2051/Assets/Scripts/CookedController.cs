using UnityEngine;

public class CookedController : MonoBehaviour
{
    public BreadController m_BreadController = default;

    public void OnMouseDown()
    {
        m_BreadController.GetBreadDone();
    }
}
