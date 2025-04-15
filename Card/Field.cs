using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Field : MonoBehaviour
{
    [SerializeField]
    private Hand hand;
    [SerializeField]
    private DiscardArea discardArea;

    List<CardController> cards = new List<CardController>();

    public void AddCard(CardController card)
    {
        cards.Add(card);

        card.gameObject.SetActive(true);

        card.transform.SetParent(transform);
        card.transform.DOLocalMove(Vector3.zero, 0.2f);
        card.transform.localScale = Vector3.one;
    }

    public void RemoveCard()
    {
        while (cards.Count > 0)
        {
            CardController card = cards[0];
            cards.Remove(card);
            card.transform.SetParent(discardArea.transform);
            card.gameObject.SetActive(false);
            discardArea.AddCard(card);
        }
    }

    public int GetCardCount()
    {
        return cards.Count;
    }


}
