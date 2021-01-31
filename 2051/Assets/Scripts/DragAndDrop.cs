using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    Vector3 m_OriginalPosition = default;

    public bool m_CanMove = true;

    bool m_IsDragging = false;

    void Start()
    {
        m_OriginalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (!m_CanMove)
            m_IsDragging = false;

        if(m_IsDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
            transform.SetAsLastSibling();
        }
    }

    public void OnMouseDown()
    {
        if(m_CanMove)
            m_IsDragging = true;
    }

    public void OnMouseUp()
    {
        if (m_CanMove)
            RestartPosition();
    }

    public void RestartPosition()
    {
        m_IsDragging = false;
        transform.position = m_OriginalPosition;
    }
}
