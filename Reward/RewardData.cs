using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RewardType
{
    AddCard,
    IncreasePullSpeed,
    DecreaseSpiritConsumeRatio,
    IncreaseSpiritRecoveryRatio
}

[CreateAssetMenu(fileName = "RewardData", menuName = "Create RewardData")]
public class RewardData : ScriptableObject
{
    public RewardType type;
    public CardData card; // AddCardÇÃèÍçáÇÃÇ›égóp

    public RewardData(RewardType type, CardData card = null)
    {
        this.type = type;
        this.card = card;

    }
    public RewardData Initialize(RewardType rewardType, CardData cardData = null)
    {
        this.type = rewardType;
        this.card = cardData;
        return this;
    }
}
