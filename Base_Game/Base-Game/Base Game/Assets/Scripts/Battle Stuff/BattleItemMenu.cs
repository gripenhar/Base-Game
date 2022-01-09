using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleItemMenu : MonoBehaviour
{
    public GameManager GM;
    public BattleSystem BS;
    private BattleHUD battleHUD;
    private BattleHUD battleHUD2;

    public GameObject OutsideItemsPanel;
    //private bool usingItemPanel;

    [SerializeField] private RectTransform itemMenu;
	[SerializeField] private RectTransform itemButtonTemplate;
	[SerializeField] private RectTransform itemMenuContainer;

    public InventoryItem battleBomb;
    public InventoryItem redHealthPotion;

    public Inventory2 inventory2;
    private bool showItems;
    private List<GameObject> tempItemButtons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        inventory2 = GameObject.Find("ItemManager").GetComponent<Inventory2>();
        battleHUD = GameObject.Find("PlayerBattleHUD").GetComponent<BattleHUD>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        BS = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();
        //usingItemPanel = false;
    }

    // Update is called once per frame
    void Update()
    {
 
    }
    //show items needs to be called like show responses
    public void ShowItems()
    {
        float itemMenuHeight = 0;
        List<InventoryItem> items = inventory2.itemList;
        int itemCount = 0;
        //if(!showItems)
        //{
        foreach (InventoryItem item in items)
        {
            if(item.inBattle)
            {
            if(!showItems)
            {
            GameObject itemButton = Instantiate(itemButtonTemplate.gameObject, itemMenuContainer);
            itemButton.gameObject.SetActive(true);
            itemButton.GetComponent<TMP_Text>().text = item.itemName;
            itemButton.GetComponent<Button>().onClick.AddListener(call:() => OnPickedItem(item, itemCount));

            tempItemButtons.Add(itemButton);

            itemMenuHeight += itemButtonTemplate.rect.size.y;

            itemCount += 1;
            //showItems = true;
            }
            OutsideItemsPanel.SetActive(true);
            itemMenu.gameObject.SetActive(true);
            //usingItemPanel = true;
            }
        }
        showItems = true;
        //}

    }

    private void OnPickedItem(InventoryItem item, int itemCount)
    {
        itemMenu.gameObject.SetActive(false);
        OutsideItemsPanel.SetActive(false);
        showItems = false;
        BS = GameObject.Find("BattleSystem").GetComponent<BattleSystem>();

        foreach (GameObject button in tempItemButtons)
        {
            Destroy(button);
        }
        tempItemButtons.Clear();

       //item events
       if(item == redHealthPotion)
           {
               inventory2.itemList.Remove(redHealthPotion);
               BS.RedHealthPotion();
               /*if(BS.state == BattleState.PLAYERTURN)
               {
                    GM.playerInfo.currentHP += 10;
                    battleHUD.SetHP(GM.playerInfo.currentHP);

               }
               if(BS.state == BattleState.SALLYTURN)
               {
                    GM.SallyInfo.currentHP += 10;
                    battleHUD2.SetHP(GM.SallyInfo.currentHP);
               }*/

           }

       if(item == battleBomb)
       {

        inventory2.itemList.Remove(battleBomb);
        BS.BattleBomb();
       }

    }
    /*
    private void HideIfClickedOutside(GameObject panel)
   {
         if (Input.GetMouseButton(0) && panel.activeSelf && 
             !RectTransformUtility.RectangleContainsScreenPoint(
                 panel.GetComponent<RectTransform>(), 
                 Input.mousePosition, 
                 Camera.main)) {
            Debug.Log("We are NOT clicking the button!");
             panel.SetActive(false);
         }
     }
     */

     public void CloseMenu()
     {
     if(showItems) 
     {
     itemMenu.gameObject.SetActive(false);
     OutsideItemsPanel.SetActive(false);
     }
     }
}
