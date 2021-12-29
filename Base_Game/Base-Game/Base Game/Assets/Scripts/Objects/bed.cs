using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bed : Interactable
{

    [Header("Transition Variables")]
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float fadeWait;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void fadeInFadeOut()
    {
            if(fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, 1);
        }
        StartCoroutine(FadeCo());

    }

    public IEnumerator FadeCo()
    {   
        if(fadeOutPanel != null)
        {
            GameObject panel = Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
            Destroy(panel, 1);
    
        }
        yield return new WaitForSeconds(fadeWait);
    }
}
