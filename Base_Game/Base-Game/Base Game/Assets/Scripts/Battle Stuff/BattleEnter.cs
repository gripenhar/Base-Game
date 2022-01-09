using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleEnter : MonoBehaviour
{

    public GameManager GM;  
    public string sceneToLoad;
    private Enemy thisEnemy;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            thisEnemy = gameObject.GetComponent<Enemy>();
            if(GM == null)
            {
                GM = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            Debug.Log("BattleEnter: About to call GM.PrepareBattleScene()");

            GM.currentEnemySlot1 = thisEnemy.enemyInfo;
            if(thisEnemy.enemyInfo2 != null)
            {
                GM.currentEnemySlot2 = thisEnemy.enemyInfo2;
            }

            SaveSystem.SavePlayer(GM.player);
            SceneManager.LoadScene(sceneToLoad);
        }
    }

}
