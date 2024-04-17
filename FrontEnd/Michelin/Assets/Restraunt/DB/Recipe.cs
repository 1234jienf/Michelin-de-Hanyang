using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "recipe")]
public class Recipe : ScriptableObject
{
    public int Level;
    public Sprite Food;
    public string Name;
    public string Ingredient;
    public string Process1;
    public string Process2;
    public string Process3;
    public string Process4;
    public string Process5;
    public string Process6;
    public string Process7;

    public void Print () {
        Debug.Log(Name + ": " + "재료 : " + Ingredient + "만드는 방법 : " + Process1);
    }


}
