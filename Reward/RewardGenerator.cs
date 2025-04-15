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

        // �J�[�h���烉���_��1��
        var cardList = cardDatabase.GetDeckList();
        if (cardList.Count > 0)
        {
            CardData randomCard = cardList[Random.Range(0, cardList.Count)];
            // CreateInstance���g�p����RewardData���C���X�^���X��
            allRewards.Add(ScriptableObject.CreateInstance<RewardData>().Initialize(RewardType.AddCard, randomCard));
        }

        // �X�e�[�^�X�n��V
        allRewards.Add(ScriptableObject.CreateInstance<RewardData>().Initialize(RewardType.IncreasePullSpeed));
        allRewards.Add(ScriptableObject.CreateInstance<RewardData>().Initialize(RewardType.DecreaseSpiritConsumeRatio));
        allRewards.Add(ScriptableObject.CreateInstance<RewardData>().Initialize(RewardType.IncreaseSpiritRecoveryRatio));

        // �����_����3�I��ŕԂ�
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

