using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutOnTap : MonoBehaviour
{

    public float fadeTimeIn;
    public float fadeTimeOut;
    public int buttonIndex;
    public bool automaticFadeOut;

    private CanvasGroup canvasGroup;

    private GameObject sceneController;

    //si esto está a false no empezará la corrutina de fadeout
    private bool canStart;
 

    // Start is called before the first frame update
    void Start()
    {
        
        canvasGroup = GetComponent<CanvasGroup>();
        sceneController = GameObject.Find("SceneController");
        if (sceneController.GetComponent<SceneController>().returnIndex() == buttonIndex)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
        else {
            gameObject.GetComponent<Button>().interactable = false;
        }
        canStart = false;
        StartCoroutine(IFadeOnStart());
    }

    // Update is called once per frame
    void Update()
    {
   
    }

    public void Fade() {
        if (canStart == true)
        {
            StartCoroutine(IFade());
        }
    }

    IEnumerator IFade() {

        float counter = 0f;

            while (counter < fadeTimeOut)
            {
                counter += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, counter / fadeTimeOut/100);
                yield return null;
            }

        sceneController.GetComponent<SceneController>().upgradeIndex();
    }

    IEnumerator IFadeOnStart()
    {
        float counter = 0f;

        while (counter < fadeTimeIn)
        {
            counter += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, counter / fadeTimeIn/100);
            //gameObject.GetComponent<Button>().interactable = false;
            
            yield return null;
        }

        canStart = true;
        if (automaticFadeOut)
        {
            StartCoroutine(IFade());
        }
    }
}
