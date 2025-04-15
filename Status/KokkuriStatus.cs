using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Create KokkuriStatusData")]
public class KokkuriStatus : ScriptableObject
{
    public string NAME;
    public float MaxHP;
    public float HP;
    public float MaxSpirit;
    public float Spirit;
    public float ATK;
    public float SpiritConsumeRatio;
    public float SpiritRecoveryRatio;
    public float pullSpeed;
    public float pullIntervalMax;
    public float pullDurationMax;
    public CardDataBase kokkuriDeck;
}
