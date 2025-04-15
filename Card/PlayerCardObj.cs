using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardObj : MonoBehaviour
{
    [SerializeField]
    private Text Kotodama;
    [SerializeField]
    private Text ValueText;
    [SerializeField]
    private Text TypeText;


    public void Show(CardModel cardModel)
    {
        Kotodama.text = cardModel.letter;
        ValueText.text = effectString(cardModel);
        TypeText.text = TypeTextSwitch(cardModel);
    }

    private string effectString(CardModel cardModel)
    {
        string text = "";
        switch(cardModel.cardType)
        {
            case CardType.Attack:
                //text = "ダメージ:\nATK×" + cardModel.power;
                text = $"敵にダメージ：\nATK×{cardModel.power}";
                break;
            case CardType.Heal:
                text = $"回復：\nATK×{cardModel.power}";
                break;
            case CardType.Buff:
                text = $"自身にバフ：\nATK+{cardModel.power}";
                break;
            case CardType.Debuff:
                text = $"敵にデバフ：\nATK-{cardModel.power}";
                break;
            case CardType.Special:
                text = "？？？";
                break;
        }
        return text;
    }

    private string TypeTextSwitch(CardModel cardModel)
    {
        string text = "";
        switch (cardModel.cardType)
        {
            case CardType.Attack:
                text = "攻";
                break;
            case CardType.Heal:
                text = "癒";
                break;
            case CardType.Buff:
                text = "強";
                break;
            case CardType.Debuff:
                text = "呪";
                break;
            case CardType.Special:
                text = "特";
                break;
        }
        return text;
    }
}
