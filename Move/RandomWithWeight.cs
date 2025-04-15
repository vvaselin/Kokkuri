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

        // �m���̍��v�l���i�[
        float total = 0;

        // �G�h���b�v�p�̎�������h���b�v�������v����
        foreach (KeyValuePair<int, float> elem in itemDropDict)
        {
            total += elem.Value;
        }

        // Random.value�ł�0����1�܂ł�float�l��Ԃ��̂�
        // �����Ƀh���b�v���̍��v���|����
        float randomPoint = Random.value * total;

        // randomPoint�̈ʒu�ɊY������L�[��Ԃ�
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