using UnityEngine;
using UnityEngine.EventSystems;

public class CardMouseHandler : MonoBehaviour, IPointerEnterHandler
{
    private Hand hand;
    private int cardIndex;

    public void SetHand(Hand hand, int index)
    {
        this.hand = hand;
        this.cardIndex = index;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hand != null)
        {
            hand.SelectCard(cardIndex);
        }
    }
}
