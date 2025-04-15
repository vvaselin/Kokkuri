using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    public RewardOptionUI rewardOptionUIs;

    private void Awake()
    {
        rewardOptionUIs = GetComponent<RewardOptionUI>();
    }

    public void Init(RewardData data)
    {
        rewardOptionUIs.Show(data);
    }

    public RewardDataModel GetReward()
    {
        return rewardOptionUIs.GetReward();
    }
}
