using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel
{
    public string letter;
    public float power;
    public CardType cardType;

    public CardModel(CardData data)
    {
        letter = data.letter;
        power = data.power;
        cardType = data.cardType;
    }

    public CardModel(int cardID)
    {
        CardData cardData = Resources.Load<CardData>("PlayerCard/PlayerCard" + cardID);
        letter = cardData.letter;
        power = cardData.power;
        cardType = cardData.cardType;
    }

    public CardModel(string character, int cardID)
    {
        CardData cardData = Resources.Load<CardData>(character + "Card/" + character + "Card" + cardID);
        letter = cardData.letter;
        power = cardData.power;
        cardType = cardData.cardType;
    }

}
