using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RewardOptionUI : MonoBehaviour
{
    [SerializeField] private Text descriptionText;
    //[SerializeField] private Image highlight;

    private RewardDataModel reward;

    public void Show(RewardData model)
    {
        reward = new RewardDataModel(model);
        descriptionText.text = SwitchDescription();
    }

    private string SwitchDescription()
    {
        string description="";

        switch (reward.type)
        {
            case RewardType.AddCard:
                description = $"新しいカード獲得：\n{reward.card.letter}\n（{reward.card.cardType}:{reward.card.power}）";
                break;
            case RewardType.IncreasePullSpeed:
                description = "引く速さが上昇（+0.2）";
                break;
            case RewardType.DecreaseSpiritConsumeRatio:
                description = "霊力消費が減少（-5）";
                break;
            case RewardType.IncreaseSpiritRecoveryRatio:
                description = "霊力回復量が増加（+5）";
                break;
        }

        return description;
    }

    public RewardDataModel GetReward()
    {
        return reward;
    }
}


