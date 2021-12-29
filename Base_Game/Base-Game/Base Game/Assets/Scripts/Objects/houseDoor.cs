using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class houseDoor : Interactable
{
    private Animator anim;

    [Header("New Scene Variables")]
    public string sceneToLoad;
    public Vector2 playerPosition;
    public VectorValue playerStorage;

    [Header("Transition Variables")]
    public GameObject fadeInPanel;
    public GameObject fadeOutPanel;
    public float fadeWait;

    void Awake()
    {
        if(fadeInPanel != null)
        {
            GameObject panel = Instantiate(fadeInPanel, Vector3.zero, Quaternion.identity) as GameObject;
            Destroy(panel, 1);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    if(Input.GetButtonDown("attack") && playerInRange)
        {
            anim.SetBool("isOpen", true);
            playerStorage.initialValue = playerPosition;
            GameObject.Find("HealthHolder").SetActive(false);
            GameObject.Find("Coin info").SetActive(false);
            StartCoroutine(FadeCo());
        }
    }
   
    public IEnumerator FadeCo()
    {   
        yield return new WaitForSeconds(fadeWait);

        if(fadeOutPanel != null)
        {
            Instantiate(fadeOutPanel, Vector3.zero, Quaternion.identity);
    
        }
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }

}
