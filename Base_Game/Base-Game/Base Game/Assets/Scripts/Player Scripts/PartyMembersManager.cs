using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartyMembersManager : MonoBehaviour
{
    public GameObject SallyPrefab;
    public GameManager GM;
    public PlayerMovement Player;

    // Start is called before the first frame update
    public void StartParty()
    {

        GameObject SignSally = GameObject.Find("Sign Sally (1)");
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        if(SignSally != null && GM.gameProgress.SallyJoined)
        {
            SignSally.SetActive(false);
        }

        if(GM.gameProgress.SallyJoined)
        {
            CreateSally();
        }
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

    }

    /*
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player = GameObject.Find("Player").GetComponent<Transform>();
        if(GM.gameProgress.SallyJoined)
        {
            CreateSally();
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
       
    }

    public void CreateSally()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        var SallyGO = Instantiate(SallyPrefab, Player.transform.position, Quaternion.identity);
        SallyGO.transform.position = Player.transform.position;

    }

    
    public void AddSally()
    {
        GameManager GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        GM.PartyMembers += 1;
        GM.gameProgress.PartyMembers = GM.PartyMembers;
        GM.gameProgress.SallyJoined = true;
        CreateSally();
    }
}
