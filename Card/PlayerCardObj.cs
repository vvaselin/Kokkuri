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
                //text = "�_���[�W:\nATK�~" + cardModel.power;
                text = $"�G�Ƀ_���[�W�F\nATK�~{cardModel.power}";
                break;
            case CardType.Heal:
                text = $"�񕜁F\nATK�~{cardModel.power}";
                break;
            case CardType.Buff:
                text = $"���g�Ƀo�t�F\nATK+{cardModel.power}";
                break;
            case CardType.Debuff:
                text = $"�G�Ƀf�o�t�F\nATK-{cardModel.power}";
                break;
            case CardType.Special:
                text = "�H�H�H";
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
                text = "�U";
                break;
            case CardType.Heal:
                text = "��";
                break;
            case CardType.Buff:
                text = "��";
                break;
            case CardType.Debuff:
                text = "��";
                break;
            case CardType.Special:
                text = "��";
                break;
        }
        return text;
    }
}
