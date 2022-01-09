using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState
{
    START, PLAYERTURN, SALLYTURN, ENEMYTURN, ENEMYTURN2, WON, LOST
}

public class BattleSystem : MonoBehaviour
{
    public GameManager GM;

    public GameObject playerPrefab;
    public GameObject SallyPrefab;
    //public GameObject enemyPrefab;
    public GameObject playerButtons;

    public GameObject TargetAttackButtons;
    public TMP_Text TargetAttack1Text;
    public TMP_Text TargetAttack2Text;

    public GameObject TargetHealButtons;
    public TMP_Text TargetHeal1Text;
    public TMP_Text TargetHeal2Text;

    public Transform playerBattleStation;
    public Transform playerBattleStation2;
    public Transform enemyBattleStation;
    public Transform enemyBattleStation2;

    public GameObject playerBattleStationGO;
    public GameObject playerBattleStation2GO;
    public GameObject enemyBattleStationGO;
    public GameObject enemyBattleStation2GO;

    public GameObject playerHUDGO;
    public BattleHUD playerHUD;
    public GameObject SallyHUDGO;
    public BattleHUD SallyHUD;

    public BattleHUD enemyHUD;
    public GameObject enemyHUDGO;
    public GameObject enemySpriteGO;

    public BattleHUD enemy2HUD;
    public GameObject enemy2HUDGO;
    public GameObject enemy2SpriteGO;

    public LootTableItem thisLoot;
    private Inventory2 inventory;
    private int tempCoins;

    public AudioClip selectSound;
    public AudioClip attackSound;
    public AudioClip destroySound;
    public AudioClip healSound;

    [Header("IFrame Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public bool isFlashing;
    private SpriteRenderer[] playerSprite;
    private SpriteRenderer[] SallySprite;

    //PlayerInfo playerUnit;
    //Unit enemyUnit;

    

    public TMP_Text dialogueText;

    public BattleState state;

    public string WinSceneToLoad;
    public string LoseSceneToLoad;


    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log(GM.gameProgress.PlayerName);
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        if(GM == null)
        {
            GM = GameObject.Find("GameManager").GetComponent<GameManager>();
            //Debug.Log("GM was null, but we found it!");
        }
        
        tempCoins = 0;
        playerHUDGO.SetActive(true);
        playerBattleStationGO.SetActive(true);
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);

        playerButtons.SetActive(false);

        if(GM.PartyMembers == 2 && GM.SallyInfo.currentHP > 0)
        {
           Debug.Log("load sally?");
            playerBattleStation2GO.SetActive(true);
            SallyHUDGO.SetActive(true);
            GameObject SallyGO = Instantiate(SallyPrefab, playerBattleStation2);
        }

        enemyHUDGO.SetActive(true);
        enemyBattleStationGO.SetActive(true);
        if(GM.currentEnemySlot1 != null)
        {
            Debug.Log("currentEnemySlot1 = " + GM.currentEnemySlot1.enemyPrefab);
            Debug.Log("enemyBattleStation = " + enemyBattleStation);
            enemySpriteGO = Instantiate(GM.currentEnemySlot1.enemyPrefab, enemyBattleStation);
        }
        else
        {
            Debug.LogError("GM.currentEnemySlot1 == null, which at this point should never happen.");
            enemyHUDGO.SetActive(false);
            enemyBattleStationGO.SetActive(false);
        }
        if(GM.currentEnemySlot2 != null)
        {
            Debug.Log("Loading the second enemy now!");
            enemy2HUDGO.SetActive(true);
            enemyBattleStation2GO.SetActive(true);
            enemy2SpriteGO = Instantiate(GM.currentEnemySlot2.enemyPrefab, enemyBattleStation2);
        }
        else
        {
            enemy2HUDGO.SetActive(false);
            enemyBattleStation2GO.SetActive(false);
        }
        if(!enemyHUDGO.activeSelf && !enemy2HUDGO.activeSelf)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }

        dialogueText.text = "A wild " + GM.currentEnemySlot1.enemyName + " approaches!";

        playerHUD.SetHUD(GM.playerInfo);
        if(SallyHUDGO.activeSelf)
        {
            SallyHUD.SetHUD(GM.SallyInfo);
        }

        enemyHUD.SetEnemyHUD(GM.currentEnemySlot1);
        //Debug.Log($@"Battle started with enemy #1: [{GM.currentEnemySlot1.enemyName}]! Current HP = [{GM.currentEnemySlot1.currentHP}]");
        if(enemy2HUDGO.activeSelf)
        {
            enemy2HUD.SetEnemyHUD(GM.currentEnemySlot2);
            //Debug.Log($@"Battle started with enemy #2: [{GM.currentEnemySlot2.enemyName}]! Current HP = [{GM.currentEnemySlot2.currentHP}]");
        }

        yield return new WaitForSeconds(1f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    public void ChooseTargetAttack()
    {
        // Check if more than 1 Enemy is alive
        // TODO: this needs to hold each enemy slot
        Debug.Log("Did... uh... did we even get here?");
        if(GM.currentEnemySlot1.isAlive && (GM.currentEnemySlot2 != null && GM.currentEnemySlot2.isAlive))
        {
            Debug.Log("there are two enemies alive, choose which to attack");
            TargetHealButtons.SetActive(false);
            TargetAttackButtons.SetActive(true);
            TargetAttack1Text.text = GM.currentEnemySlot1.enemyName;
            TargetAttack2Text.text = GM.currentEnemySlot2.enemyName;
        }
        // If Enemy2 is dead OR null
        else if(GM.currentEnemySlot1.isAlive && (GM.currentEnemySlot2 == null || !GM.currentEnemySlot2.isAlive))
        {
            Debug.Log("Enemy 2 is dead OR not in this battle, we can only attack enemy 1");
            if(state == BattleState.PLAYERTURN)
            {
                StartCoroutine(PlayerAttack(GM.currentEnemySlot1));
            }
            else if(state == BattleState.SALLYTURN)
            {
                StartCoroutine(SallyAttack(GM.currentEnemySlot1));
            }
        }
        // If Enemy1 is dead, but 
        else if(!GM.currentEnemySlot1.isAlive && (GM.currentEnemySlot2 != null && GM.currentEnemySlot2.isAlive))
        {
            Debug.Log("Enemy 1 is dead, we can only attack enemy 2");
            if(state == BattleState.PLAYERTURN)
            {
                StartCoroutine(PlayerAttack(GM.currentEnemySlot2));
            }
            else if(state == BattleState.SALLYTURN)
            {
                StartCoroutine(SallyAttack(GM.currentEnemySlot2));
            }
        }
    }

    public void ChooseTargetHeal()
    {
        // Check if more than 1 Player character (Player, Sally, etc) is alive
        // TODO: make this dynamic to the number of playerCharacters
        if(GM.PartyMembers == 2)
        {
            Debug.Log("there are two allies in the PARTY!!");
            if((GM.playerInfo.currentHP > 0) && (GM.SallyInfo.currentHP > 0))
            {
                Debug.Log("there are two allies alive, choose which to heal");
                TargetAttackButtons.SetActive(false);
                TargetHealButtons.SetActive(true);
                TargetHeal1Text.text = GM.playerInfo.unitName;
                TargetHeal2Text.text = GM.SallyInfo.unitName;
            }
            else
            {
                Debug.Log("one ally is down, the other can only heal themself!");
                if(state == BattleState.PLAYERTURN)
                {
                    StartCoroutine(PlayerHeal(GM.playerInfo));
                }
                else if(state == BattleState.SALLYTURN)
                {
                    StartCoroutine(SallyHeal(GM.SallyInfo));
                }
            }
        }
        // Only 1 player character is alive
        else
        {
            Debug.Log("only one ally, they can only heal themself!");
            if(state == BattleState.PLAYERTURN)
            {
                StartCoroutine(PlayerHeal(GM.playerInfo));
            }
            else if(state == BattleState.SALLYTURN)
            {
                StartCoroutine(SallyHeal(GM.SallyInfo));
            }
        }
    }

    IEnumerator PlayerAttack(EnemyInfo targetEnemy)
    {
        playerButtons.SetActive(false);
        //if(GM.PartyMembers == 2)
        //{
        //    state = BattleState.SALLYTURN;
        //}
        //else
        //{
        //    state = BattleState.ENEMYTURN;
        //}
        bool isDead = targetEnemy.TakeDamage(GM.playerInfo.damage);
        enemyHUD.SetHP(GM.currentEnemySlot1.currentHP);
        if(GM.currentEnemySlot2 != null)
        {
            enemy2HUD.SetHP(GM.currentEnemySlot2.currentHP);
        }
        dialogueText.text = "Player's attack is successful";
        this.GetComponent<AudioSource>().PlayOneShot(attackSound);

        yield return new WaitForSeconds(1f);

        if(!GM.currentEnemySlot1.isAlive && enemyHUDGO.activeSelf)
        {
            enemyHUDGO.SetActive(false);
            tempCoins += GM.currentEnemySlot1.coinsHeld;
            enemySpriteGO.SetActive(false);
            this.GetComponent<AudioSource>().PlayOneShot(destroySound);
        }
        if(GM.currentEnemySlot2 != null && !GM.currentEnemySlot2.isAlive && enemy2HUDGO.activeSelf) 
        {
            enemy2HUDGO.SetActive(false);
            tempCoins += GM.currentEnemySlot2.coinsHeld;
            enemy2SpriteGO.SetActive(false);
            this.GetComponent<AudioSource>().PlayOneShot(destroySound);
        }

        // TODO: check if ALL enemies are dead
        if(GM.currentEnemySlot2 != null)
        {
            if(!GM.currentEnemySlot1.isAlive && !GM.currentEnemySlot2.isAlive)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else
            {
                if(GM.PartyMembers == 2 && GM.SallyInfo.currentHP > 0)
                {
                    state = BattleState.SALLYTURN;
                    SallyTurn();
                }
                else
                {
                    state = BattleState.ENEMYTURN;
                    StartCoroutine(EnemyTurn());
                }
            }
        }
        else
        {
            if(isDead)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            if(GM.PartyMembers == 2 && GM.SallyInfo.currentHP > 0)
            {
                state = BattleState.SALLYTURN;
                SallyTurn();
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
    }

    IEnumerator SallyAttack(EnemyInfo targetEnemy)
    {
        playerButtons.SetActive(false);
        state = BattleState.ENEMYTURN;
        bool isDead = targetEnemy.TakeDamage(GM.playerInfo.damage);
        enemyHUD.SetHP(GM.currentEnemySlot1.currentHP);
        if(GM.currentEnemySlot2 != null)
        {
            enemy2HUD.SetHP(GM.currentEnemySlot2.currentHP);
        }
        dialogueText.text = "Sally's attack is successful";
        this.GetComponent<AudioSource>().PlayOneShot(attackSound);

        yield return new WaitForSeconds(1f);
        if(!GM.currentEnemySlot1.isAlive && enemyHUDGO.activeSelf)
        {
            enemyHUDGO.SetActive(false);
            tempCoins += GM.currentEnemySlot1.coinsHeld;
            enemySpriteGO.SetActive(false);
            this.GetComponent<AudioSource>().PlayOneShot(destroySound);
        }
        if(GM.currentEnemySlot2 != null && !GM.currentEnemySlot2.isAlive && enemy2HUDGO.activeSelf) 
        {
            enemy2HUDGO.SetActive(false);
            tempCoins += GM.currentEnemySlot2.coinsHeld;
            enemy2SpriteGO.SetActive(false);
            this.GetComponent<AudioSource>().PlayOneShot(destroySound);
        }


        // check if the main, 1st enemy is alive
        if(!GM.currentEnemySlot1.isAlive)
        {
            // check if the enemy 2 is not null OR alive
            if(GM.currentEnemySlot2 == null || !GM.currentEnemySlot2.isAlive)
            {
                state = BattleState.WON;
                StartCoroutine(EndBattle());
            }
            else if(GM.currentEnemySlot2 != null && GM.currentEnemySlot2.isAlive)
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        //}
        //else
        //{
        //    if(!GM.currentEnemySlot1.isAlive && !GM.currentEnemySlot2.isAlive)
        //    {
        //        state = BattleState.WON;
        //        StartCoroutine(EndBattle());
        //    }
        //    else
         //   {
        //        StartCoroutine(EnemyTurn());
        //    }
        //}
    }

    IEnumerator EnemyTurn()
    {
        bool isPlayerDead = false;
        bool isSallyDead = false;
        if(GM.currentEnemySlot1 != null && GM.currentEnemySlot1.isAlive)
        {
            playerButtons.SetActive(false);
            dialogueText.text = GM.currentEnemySlot1.enemyName + " attacks!";

            yield return new WaitForSeconds(1f);
            this.GetComponent<AudioSource>().PlayOneShot(attackSound);
           
            int enemyRandomTargeting = Random.Range(1, GM.PartyMembers+1);
            //Debug.Log(enemyRandomTargeting + " :: Range is 1(inclusive) to " + (GM.PartyMembers+1) + "(exclusive).");

            switch(enemyRandomTargeting)
            {
                case 1:
                    // 1 == Player
                    isPlayerDead = GM.playerInfo.TakeDamage(GM.currentEnemySlot1.damage);
                    playerHUD.SetHP(GM.playerInfo.currentHP);
                    StartCoroutine(FlashCo());
                    break;
                case 2:
                    // 2 == Sally
                    isSallyDead = GM.SallyInfo.TakeDamage(GM.currentEnemySlot1.damage);
                    SallyHUD.SetHP(GM.SallyInfo.currentHP);
                    StartCoroutine(SallyFlashCo());
                    break;
                default:
                    // Default == Player
                    isPlayerDead = GM.playerInfo.TakeDamage(GM.currentEnemySlot1.damage);
                    playerHUD.SetHP(GM.playerInfo.currentHP);
                    StartCoroutine(FlashCo());
                    break;
            }

            //bool isPlayerDead = GM.playerInfo.TakeDamage(GM.currentEnemyBattle.damage);
            //playerHUD.SetHP(GM.playerInfo.currentHP);

            //bool isSallyDead = GM.SallyInfo.TakeDamage(GM.currentEnemyBattle.damage);
            //SallyHUD.SetHP(GM.SallyInfo.currentHP);

            if(isPlayerDead && playerHUDGO.activeSelf)
            {
                playerHUDGO.SetActive(false);
                GameObject PlayerUIGO = GameObject.Find("BattlePlayerContainer(Clone)");
                PlayerUIGO.SetActive(false);
                this.GetComponent<AudioSource>().PlayOneShot(destroySound);
            }
            if(isSallyDead && SallyHUDGO.activeSelf) 
            {
                SallyHUDGO.SetActive(false);
                GameObject SallyUIGO = GameObject.Find("BattleSallyContainer(Clone)");
                SallyUIGO.SetActive(false);
                this.GetComponent<AudioSource>().PlayOneShot(destroySound);
            }
        }
        yield return new WaitForSeconds(1f);

        if(GM.currentEnemySlot2 != null && GM.currentEnemySlot2.isAlive)
        {
            state = BattleState.ENEMYTURN2;
            StartCoroutine(EnemyTurn2());
        }
        else
        {
            if(isPlayerDead)
            {
                if(GM.PartyMembers > 1)
                {
                    if(isSallyDead)
                    {
                        state = BattleState.LOST;
                        StartCoroutine(EndBattle());
                    }
                    state = BattleState.SALLYTURN;
                    SallyTurn();
                }
                state = BattleState.LOST;
                StartCoroutine(EndBattle());
            }
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EnemyTurn2()
    {
        bool isPlayerDead = false;
        bool isSallyDead = false;
        if(GM.currentEnemySlot2 != null && GM.currentEnemySlot2.isAlive)
        {
            playerButtons.SetActive(false);
            dialogueText.text = GM.currentEnemySlot2.enemyName + " attacks!";

            yield return new WaitForSeconds(1f);
            this.GetComponent<AudioSource>().PlayOneShot(attackSound);
            
            int enemyRandomTargeting = Random.Range(1, GM.PartyMembers+1);
            //Debug.Log(enemyRandomTargeting + " :: Range is 1(inclusive) to " + (GM.PartyMembers+1) + "(exclusive).");

            switch(enemyRandomTargeting)
            {
                case 1:
                    // 1 == Player
                    isPlayerDead = GM.playerInfo.TakeDamage(GM.currentEnemySlot2.damage);
                    playerHUD.SetHP(GM.playerInfo.currentHP);
                    StartCoroutine(FlashCo());
                    break;
                case 2:
                    // 2 == Sally
                    isSallyDead = GM.SallyInfo.TakeDamage(GM.currentEnemySlot2.damage);
                    SallyHUD.SetHP(GM.SallyInfo.currentHP);
                    StartCoroutine(SallyFlashCo());
                    break;
                default:
                    // Default == Player
                    isPlayerDead = GM.playerInfo.TakeDamage(GM.currentEnemySlot2.damage);
                    playerHUD.SetHP(GM.playerInfo.currentHP);
                    StartCoroutine(FlashCo());
                    break;
            }

            //bool isPlayerDead = GM.playerInfo.TakeDamage(GM.currentEnemyBattle.damage);
            //playerHUD.SetHP(GM.playerInfo.currentHP);

            //bool isSallyDead = GM.SallyInfo.TakeDamage(GM.currentEnemyBattle.damage);
            //SallyHUD.SetHP(GM.SallyInfo.currentHP);

            if(isPlayerDead && playerHUDGO.activeSelf)
            {
                playerHUDGO.SetActive(false);
                GameObject PlayerUIGO = GameObject.Find("BattlePlayerContainer(Clone)");
                PlayerUIGO.SetActive(false);
                this.GetComponent<AudioSource>().PlayOneShot(destroySound);
            }
            if(isSallyDead && SallyHUDGO.activeSelf) 
            {
                SallyHUDGO.SetActive(false);
                GameObject SallyUIGO = GameObject.Find("BattleSallyContainer(Clone)");
                SallyUIGO.SetActive(false);
                this.GetComponent<AudioSource>().PlayOneShot(destroySound);
            }
        }

        yield return new WaitForSeconds(1f);

        if(isPlayerDead)
        {
            if(GM.PartyMembers > 1)
            {
                if(isSallyDead)
                {
                    state = BattleState.LOST;
                    StartCoroutine(EndBattle());
                }
                state = BattleState.SALLYTURN;
                SallyTurn();
            }
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        if(state == BattleState.WON)
        {
            //Debug.Log($@"GM.CurrentEnemyInfo.enemyName = [{GM.currentEnemySlot1.enemyName}]");
            GM.currentEnemySlot1.ResetHP();
            GM.currentEnemySlot1 = null;
            if(GM.currentEnemySlot2 != null)
            {
                GM.currentEnemySlot2.ResetHP();
                GM.currentEnemySlot2 = null;
            }

            // Loot and coins bloc
            dialogueText.text = $@"You won! You found {tempCoins} Coins!";
            inventory = GameObject.Find("ItemManager").GetComponent<Inventory2>();
            inventory.coins += tempCoins;
            yield return new WaitForSeconds(1.5f);
            MakeLoot();


            yield return new WaitForSeconds(.1f);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(WinSceneToLoad);
            
            yield return null;
            PlayerData data = SaveSystem.LoadPlayer();

            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];
            transform.position = position;
            GM.wasBattle = true;
        }
        else if(state == BattleState.LOST)
        {
            dialogueText.text = "Now ur dead what a loser";
            yield return new WaitForSeconds(1f);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(LoseSceneToLoad);

        }
    }

    void PlayerTurn()
    {
        playerButtons.SetActive(true);
        dialogueText.text = "Choose an action!";
        TargetAttackButtons.SetActive(false);
        TargetHealButtons.SetActive(false);
    }

    void SallyTurn()
    {
        //Debug.Log("It's Sally's turn");
        playerButtons.SetActive(true);
        dialogueText.text = "Choose Sally's action!";
        TargetAttackButtons.SetActive(false);
        TargetHealButtons.SetActive(false);
    }


    /// GH: TODO -----
    //  replace these hard-coded abilities with references stored on each PlayerInfo
    //  Help Keep YOUR BattleSystem Clean :D
    IEnumerator PlayerHeal(PlayerInfo playerInfo)
    {
        playerButtons.SetActive(false);
        //if(GM.PartyMembers == 2)
        //{
            //Debug.Log("Party of two!");
        //    state = BattleState.SALLYTURN;
        //}
        //else
        //{
        //    state = BattleState.ENEMYTURN;
        //}

        playerInfo.Heal(playerInfo.healAmt);
        if(playerInfo.currentHP > playerInfo.maxHP)
        {
            playerInfo.currentHP = playerInfo.maxHP;
        }
        playerHUD.SetHP(GM.playerInfo.currentHP);
        SallyHUD.SetHP(GM.SallyInfo.currentHP);

        dialogueText.text = "You can hear the sounds of weefing!";

        this.GetComponent<AudioSource>().PlayOneShot(healSound);
        yield return new WaitForSeconds(2f);

        if(GM.PartyMembers == 2 && GM.SallyInfo.currentHP > 0)
        {
            //Debug.Log("Party of two!");
            SallyTurn();
            state = BattleState.SALLYTURN;
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator SallyHeal(PlayerInfo playerInfo)
    {
        playerButtons.SetActive(false);
        //if(GM.PartyMembers == 2)
        //{
            //Debug.Log("Party of two!");
        //    state = BattleState.SALLYTURN;
        //}
        //else
        //{
        //    state = BattleState.ENEMYTURN;
        //}

        playerInfo.Heal(playerInfo.healAmt);
        if(playerInfo.currentHP > playerInfo.maxHP)
        {
            playerInfo.currentHP = playerInfo.maxHP;
        }
        playerHUD.SetHP(GM.playerInfo.currentHP);
        SallyHUD.SetHP(GM.SallyInfo.currentHP);

        dialogueText.text = "Sally shared some apples!";

        this.GetComponent<AudioSource>().PlayOneShot(healSound);
        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void OnAttackButton()
    {
        //Debug.Log($@"You clicked the AttackButton on state=[{state}]'s turn!'");
        this.GetComponent<AudioSource>().PlayOneShot(selectSound);
        if(state != BattleState.PLAYERTURN && state != BattleState.SALLYTURN) return;
        
        ChooseTargetAttack();

        //if(state == BattleState.PLAYERTURN)
        //{
            //ChooseTargetAttack();
            //StartCoroutine(PlayerAttack());
        //}
        //else if(state == BattleState.SALLYTURN)
        //{
            //ChooseTargetAttack();
            //StartCoroutine(SallyAttack());
        //}
    }

    public void OnHealButton()
    {
        //Debug.Log($@"You clicked the HealButton on state=[{state}]'s turn!'");
        this.GetComponent<AudioSource>().PlayOneShot(selectSound);
        if(state != BattleState.PLAYERTURN && state != BattleState.SALLYTURN)
        return;

        if(state == BattleState.PLAYERTURN)
        {
            ChooseTargetHeal();
            //StartCoroutine(PlayerHeal());
        }
        else if(state == BattleState.SALLYTURN)
        {
            ChooseTargetHeal();
            //StartCoroutine(SallyHeal());
        }
    }

    public void OnTargetEnemy1Button()
    {
        this.GetComponent<AudioSource>().PlayOneShot(selectSound);
        if(state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerAttack(GM.currentEnemySlot1));
        }
        else if(state == BattleState.SALLYTURN)
        {
            StartCoroutine(SallyAttack(GM.currentEnemySlot1));
        }
    }

    public void OnTargetEnemy2Button()
    {
        this.GetComponent<AudioSource>().PlayOneShot(selectSound);
        if(state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerAttack(GM.currentEnemySlot2));
        }
        else if(state == BattleState.SALLYTURN)
        {
            StartCoroutine(SallyAttack(GM.currentEnemySlot2));
        }
    }

    public void OnTargetPlayerButton()
    {
        this.GetComponent<AudioSource>().PlayOneShot(selectSound);
        if(state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerHeal(GM.playerInfo));
        }
        else if(state == BattleState.SALLYTURN)
        {
            StartCoroutine(SallyHeal(GM.playerInfo));
        }
    }

    public void OnTargetSallyButton()
    {
        this.GetComponent<AudioSource>().PlayOneShot(selectSound);
        if(state == BattleState.PLAYERTURN)
        {
            StartCoroutine(PlayerHeal(GM.SallyInfo));
        }
        else if(state == BattleState.SALLYTURN)
        {
            StartCoroutine(SallyHeal(GM.SallyInfo));
        }
    }

    public void RedHealthPotion()
    {
        StartCoroutine(RedHealthPotionCo());
    }

    IEnumerator RedHealthPotionCo()
    {
        playerButtons.SetActive(false);

        if(state == BattleState.PLAYERTURN)
        {
            //GM.playerInfo.Heal(10);
            GM.playerInfo.currentHP = GM.playerInfo.maxHP;
            playerHUD.SetHP(GM.playerInfo.currentHP);
        }
        else if(state == BattleState.SALLYTURN)
        {
            GM.SallyInfo.currentHP = GM.SallyInfo.maxHP;
            SallyHUD.SetHP(GM.SallyInfo.currentHP);
        }

        dialogueText.text = "Drank a potion!";
        this.GetComponent<AudioSource>().PlayOneShot(healSound);

        yield return new WaitForSeconds(2f);
        

        if(state == BattleState.PLAYERTURN)
        {
            if(GM.PartyMembers > 1 && GM.SallyInfo.currentHP > 0)
            {
                SallyTurn();
                state = BattleState.SALLYTURN;
            }
            else
            {
                state = BattleState.ENEMYTURN;
                StartCoroutine(EnemyTurn());
            }
        }
        else if(state == BattleState.SALLYTURN)
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void BattleBomb()
    {
        StartCoroutine(BattleBombCo());
    }

    IEnumerator BattleBombCo()
    {
        playerButtons.SetActive(false);
        dialogueText.text = "You set a bomb!";
        yield return new WaitForSeconds(.5f);
        dialogueText.text = "3...";
        yield return new WaitForSeconds(.5f);
        dialogueText.text = "3... 2...";
        yield return new WaitForSeconds(.5f);
        dialogueText.text = "3... 2... 1...";
        yield return new WaitForSeconds(.5f);
        dialogueText.text = "KABOOM!";
        this.GetComponent<AudioSource>().PlayOneShot(destroySound);
        
        bool enemy1IsDead = false;
        bool enemy2IsDead = false;
        if(GM.currentEnemySlot1.isAlive)
        {
            enemy1IsDead = GM.currentEnemySlot1.TakeDamage(20);
            enemyHUD.SetHP(GM.currentEnemySlot1.currentHP);
        }
        if(GM.currentEnemySlot2 != null && GM.currentEnemySlot2.isAlive)
        {
            enemy2IsDead = GM.currentEnemySlot2.TakeDamage(20);
            enemy2HUD.SetHP(GM.currentEnemySlot2.currentHP);
        }

        yield return new WaitForSeconds(.5f);

        dialogueText.text = "It dealt massive damage!";

        if(enemy1IsDead && enemyHUDGO.activeSelf)
        {
            enemyHUDGO.SetActive(false);
            tempCoins += GM.currentEnemySlot1.coinsHeld;
            enemySpriteGO.SetActive(false);
            this.GetComponent<AudioSource>().PlayOneShot(destroySound);
        }
        if(enemy2IsDead && enemy2HUDGO.activeSelf) 
        {
            enemy2HUDGO.SetActive(false);
            tempCoins += GM.currentEnemySlot2.coinsHeld;
            enemy2SpriteGO.SetActive(false);
            this.GetComponent<AudioSource>().PlayOneShot(destroySound);
        }

        // did we kill ALL the enemies?
        if(enemy1IsDead && (GM.currentEnemySlot2 == null || enemy2IsDead))
        {
            state = BattleState.WON;
            StartCoroutine(EndBattle());
        }

        if(state == BattleState.PLAYERTURN)
        {
            if(GM.PartyMembers > 1 && GM.SallyInfo.currentHP > 0)
            {
                SallyTurn();
                state = BattleState.SALLYTURN;
            }
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
        if(state == BattleState.SALLYTURN)
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    private IEnumerator FlashCo()
    {
        int temp = 0;
        isFlashing = false;
        playerSprite = GameObject.Find("BattlePlayerContainer(Clone)").GetComponentsInChildren<SpriteRenderer>();
        while(temp < numberOfFlashes)
        {
            var i = 0;
            playerSprite[i].color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            playerSprite[i].color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        isFlashing = true;
    }

    private IEnumerator SallyFlashCo()
    {
        int temp = 0;
        isFlashing = false;
        SallySprite = GameObject.Find("BattleSallyContainer(Clone)").GetComponentsInChildren<SpriteRenderer>();
        while(temp < numberOfFlashes)
        {
            var i = 0;
            SallySprite[i].color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            SallySprite[i].color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        isFlashing = true;
    }

    private void MakeLoot()
    {
        StartCoroutine(MakeLootCo());
    }

    private IEnumerator MakeLootCo()
    {
        if(thisLoot != null)
        {
            InventoryItem current = thisLoot.LootInventoryItem();
            if(current != null)
            {
                inventory = GameObject.Find("ItemManager").GetComponent<Inventory2>();
                inventory.itemList.Add(current);

                dialogueText.text = "You got a " + current.itemName + "!";
                yield return new WaitForSeconds(2f);
            }
        }
    }
}
