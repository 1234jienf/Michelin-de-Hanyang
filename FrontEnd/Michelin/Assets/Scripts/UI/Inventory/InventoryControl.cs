using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// Inventory ���� type
[System.Serializable]
public class InventoryItem
{
    public string itemCode;
    public int count;
}


public class InventoryControl : MonoBehaviour
{
    private bool inventroy_show;
    public GameObject Inventory;
    public ItemInfo info;
    public int money;
    public int[] capacity;
    public TMP_Text money_text;
    public TMP_Text capacity_text;


    // Category ����
    public Button button0;
    public Button button1;
    public Button button2;
    public Button button3;
    private Button[] buttons; // ��ư �迭

    public Color selectedColor = Color.white; // ���õ� ��ư�� ��
    private Color defaultColor; // �⺻ ��ư ����


    // category �� slot
    private int categoryNum;
    public GameObject[] SlotsParent;
    private List<ItemSlot[]> slots;


    // Load Json(Inventory Content)
    public List<ItemSlot[]> GetSlots() { return slots; }

    public void LoadMoney(int _money)
    {
        money = GameMgr.Instance.money;
        money_text.text = money + "��";
    }
    public void LoadToInven(int _arrayNum, string _itemCode, int _itemCnt)
    {
        for (int i = 0; i < info.weapon.Count; i++)
            if (info.weapon[i].itemCode == _itemCode)
                slots[0][_arrayNum].AddItem(info.weapon[i], _itemCnt);

        for (int i = 0; i < info.vehicle.Count; i++)
            if (info.vehicle[i].itemCode == _itemCode)
                slots[1][_arrayNum].AddItem(info.vehicle[i], _itemCnt);
        
        for (int i = 0; i < info.ingredients.Count; i++)
            if (info.ingredients[i].itemCode == _itemCode)
                slots[2][_arrayNum].AddItem(info.ingredients[i], _itemCnt);
        
        for (int i = 0; i < info.recipes.Count; i++)
            if (info.recipes[i].itemCode == _itemCode)
                slots[3][_arrayNum].AddItem(info.recipes[i], _itemCnt);
           
    }
    
    // �� ����
    public void switchCategory()
    {
        foreach (var _slot in SlotsParent)
        {
            _slot.SetActive(false);
        }
        SlotsParent[categoryNum].SetActive(true);
    }
 

    // ������ ȹ��
    public void addItem(SOItem _item, int count = 1)
    {
        string category = _item.itemCode.Substring(0, 2);
        int slot_idx;
        switch (category)
        {
            case "wp":
                slot_idx = 0;
                break;

            case "vh":
                slot_idx = 1;
                break;

            case "ig":
                slot_idx = 2;
                break;

            default:
                slot_idx = 3;
                break;
        }
        for (int i = 0; i < slots[slot_idx].Length; i++)
        {
            if (slots[slot_idx][i].item != null)
            {
                if (slots[slot_idx][i].item.itemCode == _item.itemCode)
                {
                    slots[slot_idx][i].SetSlotCount(count);
                    return;
                }
            }
        }

        for (int i = 0; i < slots[slot_idx].Length; i++)
        {
            if (slots[slot_idx][i].item == null)
            {
                slots[slot_idx][i].AddItem(_item, count);
                return;
            }
        }
    }


    // ---��ư����---
    public void ActivateSlot(int caseNumber)
    {
        // ��� GameObject�� ���� ��Ȱ��ȭ
        foreach (var slot in SlotsParent)
        {
            slot.SetActive(false);
        }

        SlotsParent[caseNumber].SetActive(true);  
    }

    void ButtonClicked(Button clickedButton, int num)
    {
        foreach (var button in buttons)
        {
            button.interactable = true; // ��ư�� �ٽ� Ŭ�� �����ϰ� �մϴ�.
            SetButtonColor(button, defaultColor); // �⺻ �������� ����
        }

        // Ŭ���� ��ư�� ��Ȱ��ȭ�ϰ� ������ ����
        clickedButton.interactable = false; // Ŭ���� ��ư�� ��Ȱ��ȭ
        SetButtonColor(clickedButton, selectedColor); // ���õ� �������� ����

        // ���⿡ ��ư Ŭ���� ���� �߰� ������ �����մϴ�.
        categoryNum = num;
        ActivateSlot(categoryNum); 
    }
    // ��ư�� ������ �����ϴ� �޼ҵ�
    void SetButtonColor(Button button, Color color)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        button.colors = cb;
    }

    // Start is called before the first frame update
    void Start()
    {
        slots = new List<ItemSlot[]>(); // slots ����Ʈ�� �ʱ�ȭ�մϴ�.

        for (int i = 0; i < SlotsParent.Length; i++) // SlotsParent�� ���̸� �������� �ݺ�
        {
            // SlotsParent�� �� GameObject���� ItemSlot ������Ʈ �迭�� �����ͼ� slots ����Ʈ�� �߰�
            slots.Add(SlotsParent[i].GetComponentsInChildren<ItemSlot>());
        }

        inventroy_show = false;
        categoryNum = 0;
        money_text.text = money + "��";
        capacity_text.text = "010/020";
        

        buttons = new Button[] { button0, button1, button2, button3 };
        defaultColor = button0.colors.normalColor;

        // �� ��ư�� ������ �߰�
        for (int i = 0; i < 4; i++)
        {
            int index = i;
            buttons[i].onClick.AddListener(() => ButtonClicked(buttons[index], index));
            capacity[i] = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Hunting Ground")
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                inventroy_show = !inventroy_show;
            }
            Inventory.SetActive(inventroy_show);
        }
        else
        {
            if(Inventory.activeSelf) Inventory.SetActive(false);
        }
        
    }
}