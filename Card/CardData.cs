using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CardType
{
    Attack,
    Heal,
    Buff,
    Debuff,
    Special
}

[CreateAssetMenu(fileName = "CardData", menuName = "Create CardData")]
public class CardData : ScriptableObject
{
    public string letter;
    public float power;
    public CardType cardType;
}

