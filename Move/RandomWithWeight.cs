using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWithWeight
{
    Dictionary<int, float> itemDropDict;

    private void InitializeDicts()
    {
        itemDropDict = new Dictionary<int, float>();
        itemDropDict.Add(0, 25.0f);
        itemDropDict.Add(1, 25.0f);
        itemDropDict.Add(2, 50.0f);
    }

    public int Choose()
    {
        InitializeDicts();

        // 確率の合計値を格納
        float total = 0;

        // 敵ドロップ用の辞書からドロップ率を合計する
        foreach (KeyValuePair<int, float> elem in itemDropDict)
        {
            total += elem.Value;
        }

        // Random.valueでは0から1までのfloat値を返すので
        // そこにドロップ率の合計を掛ける
        float randomPoint = Random.value * total;

        // randomPointの位置に該当するキーを返す
        foreach (KeyValuePair<int, float> elem in itemDropDict)
        {
            if (randomPoint < elem.Value)
            {
                return elem.Key;
            }
            else
            {
                randomPoint -= elem.Value;
            }
        }
        return 0;
    }
}