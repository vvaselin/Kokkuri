using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    public PlayerCardObj playerCardObj;
    public CardModel model;

    private void Awake()
    {
        playerCardObj = GetComponent<PlayerCardObj>();
    }

    public void Init(CardData data)
    {
        model = new CardModel(data);
        playerCardObj.Show(model);
    }

    public void Init(int cardID)
    {
        model = new CardModel(cardID);
        playerCardObj.Show(model);
    }

    public void Init(string character, int cardID)
    {
        model = new CardModel(character, cardID);
        playerCardObj.Show(model);
    }

    public string GetLetter()
    {
        return model.letter;
    }
}