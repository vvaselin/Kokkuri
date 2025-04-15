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
                description = $"�V�����J�[�h�l���F\n{reward.card.letter}\n�i{reward.card.cardType}:{reward.card.power}�j";
                break;
            case RewardType.IncreasePullSpeed:
                description = "�����������㏸�i+0.2�j";
                break;
            case RewardType.DecreaseSpiritConsumeRatio:
                description = "��͏�������i-5�j";
                break;
            case RewardType.IncreaseSpiritRecoveryRatio:
                description = "��͉񕜗ʂ������i+5�j";
                break;
        }

        return description;
    }

    public RewardDataModel GetReward()
    {
        return reward;
    }
}


