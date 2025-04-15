using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RewardsDataBase")]
public class RewardsDataBase : ScriptableObject
{
    public List<RewardData> rewards = new List<RewardData>();
    public List<RewardData> GetRewards()
    {
        return rewards;
    }
}
