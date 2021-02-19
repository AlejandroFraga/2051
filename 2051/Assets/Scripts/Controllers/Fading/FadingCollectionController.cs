using System.Collections.Generic;
using UnityEngine;

public abstract class FadingCollectionController : MonoBehaviour
{
    [Tooltip("All the buttons to iterate with fade in and fade out.")]
    public List<FadeInOut> m_Collection = new List<FadeInOut>();

    /// <summary>
    /// 
    /// </summary>
    protected int index = default;


    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        m_Collection[index].GetComponent<FadeInOut>().FadeIn();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetIndex()
    {
        return index;
    }

    /// <summary>
    /// 
    /// </summary>
    public void AddIndex()
    {
        index++;

        if (index < m_Collection.Count)
        {
            if(index > 0)
                m_Collection[index - 1].gameObject.SetActive(false);

            m_Collection[index].gameObject.SetActive(true);
            m_Collection[index].FadeIn();
        }
        else
            IndexOutOfBounds();
    }

    /// <summary>
    /// 
    /// </summary>
    protected abstract void IndexOutOfBounds();
}
