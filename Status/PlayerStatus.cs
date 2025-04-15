using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Create PlayerStatusData")]
public class PlayerStatus : ScriptableObject
{
    public float MaxHP;
    public float HP;
    public float MaxSpirit;
    public float Spirit;
    public float ATK;
    public float SpiritConsumeRatio;
    public float SpiritRecoveryRatio;
    public float pullSpeed;
    public CardDataBase playerDeck;
}
