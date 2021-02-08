using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class DragAndDropUIElement : MonoBehaviour
{
    RectTransform m_RectTransform = default;

    public Vector3 m_OriginalPosition = default;

    public bool m_CanMove = true;

    bool m_IsDragging = false;

    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_OriginalPosition = m_RectTransform.localPosition;
    }

    void Update()
    {
        if (!m_CanMove)
            m_IsDragging = false;

        if (m_IsDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
            transform.SetAsLastSibling();
        }
    }

    public void OnMouseDown()
    {
        if (m_CanMove)
        {
            m_OriginalPosition = m_RectTransform.localPosition;
            m_IsDragging = true;
        }
    }

    public void OnMouseUp()
    {
        if (m_CanMove)
            RestartPosition();
    }

    public void RestartPosition()
    {
        if (m_IsDragging)
        {
            m_IsDragging = false;
            m_RectTransform.localPosition = m_OriginalPosition;
        }
        else
            m_OriginalPosition = m_RectTransform.localPosition;
    }
}