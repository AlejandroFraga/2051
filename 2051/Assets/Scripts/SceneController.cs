using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public List<Button> buttonList;

    private int index;

    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    public void upgradeIndex() {

        if (index == 0) {
            buttonList[index].interactable = false;
            index++;
            buttonList[index].interactable = true;
        }
        else if (index < buttonList.Count)
        {
            index++;
            buttonList[index].interactable = false;
            index++;
            buttonList[index].interactable = true;
        }
    }

    public int returnIndex() {
        return index;
    }
}
