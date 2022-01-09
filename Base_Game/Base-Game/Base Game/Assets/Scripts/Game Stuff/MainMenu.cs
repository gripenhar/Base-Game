using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UI_InputWindow inputWindow;

    // Start is called before the first frame update
    private void Start()
    {
        //inputWindow = GameObject.Find("UI_InputWindow").GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        //SceneManager.LoadScene("OpeningCutscene");

    }

    public void QuitToDesktop()
    {
        Application.Quit();
    }

}

