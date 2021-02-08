using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public List<Button> buttonList;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        buttonList[index].GetComponent<FadeOutOnTap>().FadeIn();
    }


    public void upgradeIndex() {
        buttonList[index].gameObject.SetActive(false);
        index++;

        if (index < buttonList.Count)
        {
            buttonList[index].gameObject.SetActive(true);
            buttonList[index].GetComponent<FadeOutOnTap>().FadeIn();
        }
        else
        {
            SceneManager.LoadScene("RoomScene");
        }
    }

    public int returnIndex() {
        return index;
    }
}
