using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using UnityEngine.XR;

public class Deck : MonoBehaviour
{
    [SerializeField]
    PlayerCardObj PlayerCardObj;
    [SerializeField]
    Hand Hand;
    [SerializeField]
    Field Field;
    [SerializeField]
    DiscardArea DiscardArea;
    [SerializeField]
    private Text CardCount;
    [SerializeField] 
    private StatusManager statusManager;
    List<CardController> cards = new List<CardController>();

    private int dealNum = 3;
    private bool isDealing = false;

    public void UpdateDeck()
    {
        if (!isDealing&&Hand.GetCardCount() <= 0 && Field.GetCardCount() <= 0)
        {
            ResetPos();
            StartCoroutine(DelayCardDeal());
        }
        CardCount.text = cards.Count.ToString();
    }

    private IEnumerator DelayCardDeal()
    {
        isDealing = true;
        //yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < dealNum; i++)
        {
            yield return new WaitForSeconds(0.01f);

            if (cards.Count <= 0) break;
            CardController card = Draw();
            Hand.AddCard(card);
        }

        yield return null;
        Hand.ArrangeCards();

        isDealing = false;
    }

    public void SetUp()
    {
        DiscardArea.AddtoDeck();
        while (cards.Count > 0)
        {
            Destroy(cards[0].gameObject);
            cards.RemoveAt(0);
        }

        foreach (var cardData in statusManager.playerStatusInstance.playerDeck.GetDeckList())
        {
            CardController card = Spawn(cardData);
            card.gameObject.SetActive(false);
            cards.Add(card);
        }

        Shutful();

        CardCount.text = cards.Count.ToString();
    }

    public CardController Draw()
    {
        CardController card = cards[0];
        cards.RemoveAt(0);
        return card;
    }

    public void AddCard(CardController card)
    {
        cards.Add(card);
        card.transform.SetParent(transform);
        card.gameObject.SetActive(false);
        card.transform.localPosition = Vector3.zero;
        CardCount.text = cards.Count.ToString();
    }

    public void ResetPos()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].transform.localPosition = Vector3.zero;
        }
    }

    public void Shutful()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            CardController temp = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    public CardController Spawn(int cardID)
    {
        CardController card = Instantiate(PlayerCardObj, transform).GetComponent<CardController>();
        card.Init(cardID);
        return card;
    }

    public CardController Spawn(CardData cardData)
    {
        CardController card = Instantiate(PlayerCardObj, transform).GetComponent<CardController>();
        card.Init(cardData); // CardDataÇ≈èâä˙âªÇ≈Ç´ÇÈÇÊÇ§Ç…Ç∑ÇÈ
        return card;
    }

    public CardController Spawn(string character, int cardID)
    {
        CardController card = Instantiate(PlayerCardObj, transform).GetComponent<CardController>();
        card.Init(character, cardID);
        return card;
    }

    public int GetCardCount()
    {
        return cards.Count;
    }

    public void ResetDeck()
    {
        Field.RemoveCard();
        StartCoroutine(DelayedArrange());
        DiscardArea.AddtoDeck();
        while (cards.Count > 0)
        {
            Destroy(cards[0].gameObject);
            cards.RemoveAt(0);
        }
        CardCount.text = cards.Count.ToString();
    }

    private IEnumerator DelayedArrange()
    {
        yield return new WaitForSeconds(0.2f);
        Hand.ArrangeCards();
    }
}
