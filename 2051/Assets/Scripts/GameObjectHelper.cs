using UnityEngine;
using UnityEngine.UI;

public static class GameObjectHelper
{
    public static void SetVisible(Image image, bool visible = true)
    {
        Color newColor = image.color;
        newColor.a = visible ? 1 : 0;
        image.color = newColor;
    }
}
