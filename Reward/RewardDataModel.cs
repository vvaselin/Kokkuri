using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardDataModel
{
    public RewardType type;
    public CardData card;

    public RewardDataModel(RewardData rewardData)
    {
        type = rewardData.type;
        card = rewardData.card;
    }

    public RewardDataModel(RewardType type, CardData card = null)
    {
        this.type = type;
        this.card = card;
    }
}
