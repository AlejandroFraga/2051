using UnityEngine.SceneManagement;


public class StartingController : FadingCollectionController
{

    /// <summary>
    /// Go to the room scene and start the game
    /// </summary>
    protected override void IndexOutOfBounds()
    {
        SceneManager.LoadScene(StringHelper.roomScene);
    }
}
