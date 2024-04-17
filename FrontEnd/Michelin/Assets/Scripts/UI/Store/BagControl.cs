using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BagControl : MonoBehaviour
{
    public GameMgr gameMgr;
    public GameObject BagPrefab;
    public GameObject BagParent;

    private List<BagSlot> slot;
    public int total_price;
    public TMP_Text total_price_text;

    public int money;
    public TMP_Text money_text;

    public Button button_clear;

    public Toggle toggle_buy, toggle_sell;
    public bool is_buy;

    private void Awake()
    {
        gameMgr = GameMgr.Instance;
    }

    public void addItem(SOItem _item, int count = 1)
    {
        if (count == 0) return;
        total_price += (_item.itemCost * count);
        total_price_text.text = total_price + "��";

        for (int i = 0; i < slot.Count; i++)
        {

            if (slot[i].item.itemCode == _item.itemCode)
            {
                slot[i].SetSlotCount(count); 
                return;
            }

        }
        GameObject new_slot = Instantiate(BagPrefab, BagParent.transform);
        new_slot.GetComponent<BagSlot>().AddItem(_item, count);

        slot.Add(new_slot.GetComponent<BagSlot>());
    }

    private void Start()
    {
        slot = new List<BagSlot>(BagParent.GetComponentsInChildren<BagSlot>());
        button_clear.onClick.AddListener(() => EmptyBag());

        total_price = 0;
        total_price_text.text = total_price + "��";

        money = gameMgr.money;
        gameMgr.inventoryControl.LoadMoney(money);
        money_text.text = "���簡�� �ݾ�\n" + money + "��";

        is_buy = true;
        toggle_buy.onValueChanged.AddListener((isOn) => {
            if (isOn)
            {
                is_buy = true;
            }
        });

        toggle_sell.onValueChanged.AddListener((isOn) => {
            if (isOn)
            {
                is_buy = false;
            }
        });
    }

    public void BuyItems()
    {
        
        if (is_buy)
        {
            money -= total_price;
            gameMgr.money = money;
            gameMgr.inventoryControl.LoadMoney(money);
            money_text.text = "���簡�� �ݾ�\n" + money + "��";
            for (int i = 0; i < slot.Count; i++)
            {
                gameMgr.inventoryControl.addItem(slot[i].item, slot[i].itemCount);
            }
            EmptyBag();
        }
        else
        {
            money += total_price;
            gameMgr.money = money;
            gameMgr.inventoryControl.LoadMoney(money);
            money_text.text = "���簡�� �ݾ�\n" + money + "��";
            for (int i = 0; i < slot.Count; i++)
            {
                gameMgr.inventoryControl.addItem(slot[i].item, -slot[i].itemCount);
            }
            EmptyBag();
        }
    }

    public void EmptyBag()
    {
        Debug.Log("����");
        total_price = 0;
        total_price_text.text = total_price + "��";

        foreach (BagSlot slotItem in slot)
        {
            Destroy(slotItem.gameObject);
        }
        // ����Ʈ ����
        slot.Clear();

    }

}
