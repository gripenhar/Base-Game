using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerMovement : MonoBehaviour
{
    public GameManager GM;

    [Header("Dialogue Stuff")]
    [SerializeField] private DialogueUI dialogueUI;
    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable { get; set; }

    public PlayerState currentState;
    public float speed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    public Animator animator;

    //TODO Break off health system into its own component
    public FloatValue currentHealth;
    public FloatValue maxHealth;
    public SignalSender playerHealthSignal;

    public VectorValue startingPosition;
    //[SerializeField] private UI_Inventory uiInventory;
    private Inventory2 inventory2;
    public Inventory playerInventory;
    public SpriteRenderer receivedItemSprite;
    //public SignalSender playerHit;
    public SignalSender reduceMagic;

    [Header("IFrame Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerCollider;
    public SpriteRenderer mySprite;

    [Header("Projectile Stuff")]
    public GameObject projectile;
    public Item bow;
    
    // Start is called before the first frame update
    private void Awake(){

    }

    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        myRigidbody = GetComponent<Rigidbody2D>();
        currentHealth.RuntimeValue = maxHealth.RuntimeValue;

        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventory2 = GameObject.Find("ItemManager").GetComponent<Inventory2>();

        //inventory2 = new Inventory2();
        //uiInventory.SetInventory(inventory2);

        //Debug.Log("PlayerMovement.Start(): GM was null, but we found it!");


        /*
        // GH: swap out for LoadPlayer
        if(GM.wasBattle)
        {
            //Debug.Log("PlayerMovement.Start(): GM.wasBattle = true, which means we were just in a battle!");
            LoadPlayer();
            GM.wasBattle = false;
        }
        else
        {
            transform.position = startingPosition.initialValue;
        }
        */

    }

    // Update is called once per frame
    void Update()
    {

    if(playerInventory.coins < 0)
	{
	playerInventory.coins = 0;
	}

        if(currentHealth.RuntimeValue > maxHealth.RuntimeValue)
        {
            currentHealth.RuntimeValue = maxHealth.RuntimeValue;
        }
        if(currentHealth.RuntimeValue < 0f)
        {
            currentHealth.RuntimeValue = 0f;
        }

        // Is the Player in an interaction??
        if(dialogueUI != null && dialogueUI.IsOpen) return;
        if(currentState == PlayerState.interact)
        {
        animator.SetBool("moving", false);
        return;
        }
        
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("attack") && currentState != PlayerState.attack 
                                         && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }
        else if(Input.GetButtonDown("SecondWeapon") && currentState != PlayerState.attack 
                                                    && currentState != PlayerState.stagger)
        {
            if(playerInventory.CheckForItem(bow))
            {
                StartCoroutine(SecondAttackCo());
            }
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Interactable?.Interact(this);
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(0.3f);
        if(currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    private IEnumerator SecondAttackCo()
    {
        //animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        MakeArrow();
        //animator.SetBool("attacking", false);
        yield return new WaitForSeconds(0.3f);
        if(currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    private void MakeArrow()
    {
        if(playerInventory.currentMagic > 0)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            Arrow arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Setup(temp, ChooseArrowDirection());
            playerInventory.ReduceMagic(arrow.magicCost);
            reduceMagic.Raise();
        }
    }

    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    public void RaiseItem()
    {
        //if (inventory2.currentItem != null)
        //{
        if(currentState != PlayerState.interact)
        {
            animator.SetBool("receive item", true);
            currentState = PlayerState.interact;
            if (inventory2.currentItem != null)
            {
                receivedItemSprite.sprite = inventory2.currentItem.itemImage;
            }
        }
        else
        {
            animator.SetBool("receive item", false);
            currentState = PlayerState.idle;
            receivedItemSprite.sprite = null;
            inventory2.currentItem = null;
        }
        //}
    }

    void UpdateAnimationAndMove()
    {
        if(change != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        myRigidbody.MovePosition(
            transform.position + change * speed * Time.fixedDeltaTime
        );
    }

    public void Knock(float knockTime, float damage)
    {
        currentHealth.RuntimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RuntimeValue > 0)
        {
            StartCoroutine(KnockCo(knockTime));
            //currentHealth.RuntimeValue = 0f;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockCo(float knockTime)
    {
        //playerHit.Raise();
        if(myRigidbody != null)
        {
            StartCoroutine(FlashCo());
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

    private IEnumerator FlashCo()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while(temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerCollider.enabled = true;
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        //Debug.Log("saved");
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
    }


}
