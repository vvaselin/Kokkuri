using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    private List<CardData> deck = new List<CardData>();

    public List<CardData> GetDeckList()
    {
        return deck;
    }
}
