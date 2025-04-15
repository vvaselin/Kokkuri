using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class KokkuriDeck : MonoBehaviour
{
    [SerializeField]
    PlayerCardObj kokkuriCardObj;
    [SerializeField]
    KokkuriField Field;
    List<CardController> cards = new List<CardController>();
    [SerializeField]
    StatusManager statusManager;

    public void SetUp()
    {
        while (cards.Count > 0)
        {
            Destroy(cards[0].gameObject);
            cards.RemoveAt(0);
        }

        foreach (var cardData in statusManager.kokkuriStatusInstance.kokkuriDeck.GetDeckList())
        {
            CardController card = Spawn(cardData);
            card.gameObject.SetActive(false);
            cards.Add(card);
        }
        Shutful();
    }
    public void SetUp(string character)
    {
        while (cards.Count > 0)
        {
            Destroy(cards[0].gameObject);
            cards.RemoveAt(0);
        }

        for (int i = 0; i <= 6; i++)
        {
            // 事前にカードデータが存在するか確認
            string path = character + "Card/" + character + "Card" + i;
            CardData cardData = Resources.Load<CardData>(path);

            if (cardData == null)
            {
                Debug.LogWarning($"CardData not found at path: Resources/{path}. Skipping index {i}.");
                continue;
            }

            CardController card = Spawn(character, i);
            card.gameObject.SetActive(false);
            cards.Add(card);
        }
        Shutful();
    }

    public CardController Draw()
    {
        if (cards.Count == 0)
        {
            Debug.LogWarning("KokkuriDeck: Draw called with empty deck.");
            return null;
        }

        int index = Random.Range(0, cards.Count);
        CardController card = cards[index];
        cards.RemoveAt(index);
        return card;
    }

    public void AddCard(CardController card)
    {
        cards.Add(card);
        card.transform.SetParent(transform);
        card.gameObject.SetActive(false);
        card.transform.localPosition = Vector3.zero;
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

    public CardController Spawn(string character, int cardID)
    {
        CardController card = Instantiate(kokkuriCardObj, transform).GetComponent<CardController>();
        card.Init(character, cardID);
        return card;
    }

    public CardController Spawn(CardData cardData)
    {
        CardController card = Instantiate(kokkuriCardObj, transform).GetComponent<CardController>();
        card.Init(cardData); // CardDataで初期化できるようにする
        return card;
    }

    public int GetCardCount()
    {
        return cards.Count;
    }
}
