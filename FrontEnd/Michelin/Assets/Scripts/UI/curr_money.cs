using TMPro;
using UnityEngine;

public class curr_money : MonoBehaviour
{
    public TMP_Text money_script;
    
    public void SetMoneyScript(int money)
    {
        money_script.text = "���簡�� �ݾ�\n" + money + "��";
    }
}