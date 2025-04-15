using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardGenerator : MonoBehaviour
{
    [SerializeField] private CardDataBase cardDatabase;

    public RewardGenerator Instance;

    private void Awake()
    {
        Instance = this;
    }

    public List<RewardData> GetRandomRewards()
    {
        List<RewardData> allRewards = new List<RewardData>();

        // カードからランダム1枚
        var cardList = cardDatabase.GetDeckList();
        if (cardList.Count > 0)
        {
            CardData randomCard = cardList[Random.Range(0, cardList.Count)];
            // CreateInstanceを使用してRewardDataをインスタンス化
            allRewards.Add(ScriptableObject.CreateInstance<RewardData>().Initialize(RewardType.AddCard, randomCard));
        }

        // ステータス系報酬
        allRewards.Add(ScriptableObject.CreateInstance<RewardData>().Initialize(RewardType.IncreasePullSpeed));
        allRewards.Add(ScriptableObject.CreateInstance<RewardData>().Initialize(RewardType.DecreaseSpiritConsumeRatio));
        allRewards.Add(ScriptableObject.CreateInstance<RewardData>().Initialize(RewardType.IncreaseSpiritRecoveryRatio));

        // ランダムに3つ選んで返す
        var selected = new List<RewardData>();
        while (selected.Count < 3 && allRewards.Count > 0)
        {
            int i = Random.Range(0, allRewards.Count);
            selected.Add(allRewards[i]);
            allRewards.RemoveAt(i);
        }

        return selected;
    }

}

