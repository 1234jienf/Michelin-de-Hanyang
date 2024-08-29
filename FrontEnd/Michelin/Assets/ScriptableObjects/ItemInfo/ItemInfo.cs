using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Item 상세정보 리스트
[CreateAssetMenu(fileName = "ItemInfo", menuName = "ScriptableObjects/ItemInfo", order = 2)]
[System.Serializable]
public class ItemInfo : ScriptableObject
{
    public List<SOItem> weapon;
    public List<SOItem> vehicle;
    public List<SOItem> ingredients;
    public List<SOItem> recipes;
}