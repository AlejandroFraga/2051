using UnityEngine;


[RequireComponent(typeof(RectTransform))]
public class DragAndDropUIElement : MonoBehaviour
{
    /// <summary>
    /// Rect transform of the game object
    /// </summary>
    RectTransform m_RectTransform = default;

    [Tooltip("Original position of the game object.")]
    public Vector3 m_OriginalPosition = default;

    [Tooltip("Controls if the game object can be moved.")]
    public bool m_CanMove = true;

    /// <summary>
    /// Is the game object being dragged
    /// </summary>
    bool m_IsDragging = false;


    // Start is called before the first frame update
    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_OriginalPosition = m_RectTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_CanMove)
            m_IsDragging = false;

        // If we're dragging the object, we calculate the screen to world point, and move the game object there
        if (m_IsDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
            transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// On mouse down, start dragging the game object
    /// </summary>
    public void OnMouseDown()
    {
        if (m_CanMove)
        {
            m_OriginalPosition = m_RectTransform.localPosition;
            m_IsDragging = true;
        }
    }

    /// <summary>
    /// On mouse up, stop dragging the game object and restart the position
    /// </summary>
    public void OnMouseUp()
    {
        if (m_CanMove)
            RestartPosition();
    }

    /// <summary>
    /// Set the position of the game object to the original
    /// </summary>
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