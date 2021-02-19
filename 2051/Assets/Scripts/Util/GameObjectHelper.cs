using UnityEngine;
using UnityEngine.UI;


public static class GameObjectHelper
{
    /// <summary>
    /// Sets the gameObject of 'image' active if 'active' is true, the image is not null and has a gameObject
    /// </summary>
    /// <param name="image">Image to update its gameObject active property</param>
    /// <param name="active">Value to set the gameObject active property</param>
    public static void SetActive(Image image, bool active = true)
    {
        if (image)
            SetActive(image.gameObject, active);
    }

    /// <summary>
    /// Sets the 'gameObject' active if 'active' is true, the gameObject is not null
    /// </summary>
    /// <param name="gameObject">GameObject to update its active property</param>
    /// <param name="active">Value to set the gameObject active property</param>
    public static void SetActive(GameObject gameObject, bool active = true)
    {
        if (gameObject)
            gameObject.SetActive(active);
    }

    /// <summary>
    /// Changes the rect transform width to the given value
    /// </summary>
    /// <param name="trans">Rect transform of the UI element to change the width</param>
    /// <param name="width">The new width of the rectTransform</param>
    public static void SetWidth(this RectTransform trans, float width)
    {
        SetSize(trans, new Vector2(width, trans.rect.size.y));
    }

    /// <summary>
    /// Changes the rect transform size to the given value
    /// </summary>
    /// <param name="trans">Rect transform of the UI element to change the size</param>
    /// <param name="newSize">The new size of the rect transform</param>
    public static void SetSize(this RectTransform trans, Vector2 newSize)
    {
        Vector2 oldSize = trans.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        trans.offsetMin -= new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
        trans.offsetMax += new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
    }

    /// <summary>
    /// Changes the x-axis local position
    /// </summary>
    /// <param name="rectTrans">Rect transform of the UI element to change the x-axis local position</param>
    /// <param name="newLocalPosX">New x-axis local position</param>
    public static void SetLocalPositionX(this RectTransform rectTrans, float newLocalPosX)
    {
        if (rectTrans)
        {
            rectTrans.localPosition = new Vector3
            {
                x = newLocalPosX,
                y = rectTrans.localPosition.y,
                z = rectTrans.localPosition.z
            };
        }
    }
}
