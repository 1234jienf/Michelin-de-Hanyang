using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ��ǰ ��Ŭ���� ���ż��� ����, ��ٱ��Ϸ� ������ ��ŭ, ��Ŭ���� �ٷ� ��ٱ��Ͽ�
public class ItemUI : MonoBehaviour, IPointerClickHandler
{
    public SOItem item;
    
    private string itemName;
    public Image itemImg;
    public TMP_Text itemNameText;
    public TMP_Text itemPriceText;
    public TMP_Text itemDescriptionText;

    public BagControl myBag;

    private GameObject popup;

    public void SetItem(SOItem _item)
    {
        item = _item;
        itemName =_item.itemName;
        itemImg.sprite = _item.icon;
        itemNameText.text = itemName;
        if (_item.itemCost == 0)
        {
            itemPriceText.text = "-";
        }
        else
        {
            itemPriceText.text = _item.itemCost + "��";
        }
        itemDescriptionText.text = _item.itemDescription;
    }

    public void setBag(BagControl bag)
    {
        myBag = bag;
    }

    public void setPop(GameObject p)
    {
        popup = p;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int max_budget = GameMgr.Instance.money - myBag.total_price;
        confirmUI confirm = popup.GetComponent<confirmUI>();
        // ��Ŭ���� �����Ϸ��� eventData.button�� Ȯ��
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // ��Ŭ�� �� ��ٱ��Ͽ� ���
            if (max_budget >= item.itemCost)
                myBag.addItem(item);
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("��Ŭ��");
            // ��Ŭ�� �� �˾�â(���ż��� ����)
            popup.SetActive(true);
            confirm.setConfirm(item);
        }
    }
}
