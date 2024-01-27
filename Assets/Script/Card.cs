using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    public Material nameDecalMat;
    public Material iconDecalMat;
    public string description;
    public float dropRate;

    [Header("Function")]
    public CardFunctions.Function function;
    public float timer;
    public int integer;

    public bool unlocked;
}
