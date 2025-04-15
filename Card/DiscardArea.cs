using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardArea : MonoBehaviour
{
    List<CardController> cards = new List<CardController>();
    [SerializeField]
    Deck deck;
    [SerializeField]
    Hand hand;
    [SerializeField]
    Field field;

    [SerializeField] 
    private Text CardCountText;

    private void Update()
    {
        if (hand.GetCardCount() <= 0 && deck.GetCardCount()<=0 && field.GetCardCount()<=0)
        {
            AddtoDeck();
            deck.Shutful();
            CardCountText.text = cards.Count.ToString();
        }
    }

    public void AddtoDeck()
    {
        StartCoroutine(AddtoDeckCoroutine());
    }

    public IEnumerator AddtoDeckCoroutine()
    {
        ResetPos();

        yield return null;

        while (cards.Count > 0)
        {
            CardController card = Draw();
            deck.AddCard(card);
            yield return null;
        }
        CardCountText.text = cards.Count.ToString();
    }

    public void AddCard(CardController card)
    {
        cards.Add(card);
        card.transform.SetParent(transform);
        card.transform.DOLocalMove(Vector3.zero,0.2f);
        card.gameObject.SetActive(false);
        CardCountText.text = cards.Count.ToString();
    }

    public void ResetPos()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.localPosition = Vector3.zero;
        }
    }

    public CardController Draw()
    {
        CardController card = cards[0];
        cards.RemoveAt(0);
        return card;
    }
}
