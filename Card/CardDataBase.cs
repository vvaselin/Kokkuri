using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CardDataBase")]
public class CardDataBase : ScriptableObject
{
    public List<CardData> cards = new List<CardData>();

    public List<CardData> GetDeckList()
    {
        return cards;
    }
}
