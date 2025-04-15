using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    //ステータス
    [SerializeField]
    private PlayerStatus playerStatus;
    [SerializeField]
    private KokkuriStatusDataBase kokkuriDatabase; // こっくりさんのデータベース
    private int currentKokkuriIndex = 0; // 現在のこっくりさんのインデックス
    //ステータスのインスタンス化
    public PlayerStatus playerStatusInstance { get; private set; }
    public KokkuriStatus kokkuriStatusInstance { get; private set; }

    [SerializeField]
    private Text kokkuriNAME;


    //HPゲージ
    [SerializeField]
    private Slider playerHPgauge;
    [SerializeField]
    private Text p_HPtext;
    [SerializeField]
    private Slider KokkuriHPgauge;
    [SerializeField]
    private Text k_HPtext;
    //霊力ゲージ
    [SerializeField]
    private Image spiritgauge;
    [SerializeField]
    private Image Kokkurispiritgauge;

    [SerializeField]
    private Text p_ATKtext;
    [SerializeField]
    private Text k_ATKtext;

    [SerializeField]
    private Text p_pullSpeedtext;
    [SerializeField]
    private Text k_pullSpeedtext;
    [SerializeField]
    private Text KokkuriNumber;


    [SerializeField]
    CoinMove coinmove;
    [SerializeField]
    private KokkuriDeck k_deck;

    public void UpdateGauge()
    {
        if (playerHPgauge != null) playerHPgauge.value = playerStatusInstance.HP;
        if (KokkuriHPgauge != null) KokkuriHPgauge.value = kokkuriStatusInstance.HP;
        if (spiritgauge != null) spiritgauge.fillAmount = playerStatusInstance.Spirit / playerStatusInstance.MaxSpirit;
        if (Kokkurispiritgauge != null) Kokkurispiritgauge.fillAmount = kokkuriStatusInstance.Spirit / kokkuriStatusInstance.MaxSpirit;
    }

    public void UpdateText()
    {
        if (p_HPtext != null) p_HPtext.text = playerStatusInstance.HP.ToString();
        if (k_HPtext != null) k_HPtext.text = kokkuriStatusInstance.HP.ToString();
        if (p_ATKtext != null) p_ATKtext.text = $"ATK：{playerStatusInstance.ATK.ToString()}";
        if (k_ATKtext != null) k_ATKtext.text = $"ATK：{kokkuriStatusInstance.ATK.ToString()}";
        if (p_pullSpeedtext!=null) p_pullSpeedtext.text = $"PullSpeed：{playerStatusInstance.pullSpeed.ToString()}";
        if (k_pullSpeedtext != null) k_pullSpeedtext.text = $"PullSpeed：{kokkuriStatusInstance.pullSpeed.ToString()}";
        if (KokkuriNumber != null) KokkuriNumber.text = $"{currentKokkuriIndex + 1}/{kokkuriDatabase.kokkuris.Count}体目";
    }

    public void InitStatus()
    {
        playerStatusInstance = Instantiate(playerStatus);
        playerStatusInstance.Spirit = playerStatusInstance.MaxSpirit;

        playerStatusInstance.playerDeck = Instantiate(playerStatusInstance.playerDeck);

        currentKokkuriIndex = 0;
        NextKokkuri();
        P_GaugeSetUp();
        K_GaugeSetUp();
        UpdateText();
    }

    public void P_GaugeSetUp()
    {
        if (playerHPgauge != null) playerHPgauge.maxValue = playerStatusInstance.MaxHP;
        if (spiritgauge != null) spiritgauge.fillAmount = playerStatusInstance.Spirit / playerStatusInstance.MaxSpirit;
    }

    public void K_GaugeSetUp()
    {
        if (KokkuriHPgauge != null) KokkuriHPgauge.maxValue = kokkuriStatusInstance.MaxHP;
        if (Kokkurispiritgauge != null) Kokkurispiritgauge.fillAmount = kokkuriStatusInstance.Spirit / kokkuriStatusInstance.MaxSpirit;
    }

    public void NextKokkuri()
    {
        if (currentKokkuriIndex < kokkuriDatabase.kokkuris.Count)
        {
            SoundEffectController.instance.PlayKokkuriAppear();
            kokkuriStatusInstance = Instantiate(kokkuriDatabase.kokkuris[currentKokkuriIndex]);
            Debug.Log($"こっくりさん{currentKokkuriIndex}体目:{kokkuriStatusInstance.NAME}");
            if (kokkuriDatabase.kokkuris==null) Debug.Log("kokkuris is null");
            
            if(kokkuriNAME.text==null) Debug.Log("kokkuriNAME is null");
            kokkuriNAME.text = kokkuriStatusInstance.NAME;

            kokkuriStatusInstance.Spirit = kokkuriStatusInstance.MaxSpirit;

            k_deck.SetUp();

            K_GaugeSetUp();

            coinmove.SetKokkuriPullSetting();

            CutInController.Instance.CutInAction();
        }
        else
        {
            Debug.Log("No Next Kokkuri!");
        }

        
    }

    public int IncrementKokkuriIndex()
    {
        currentKokkuriIndex++;
        return currentKokkuriIndex;
    }

    public void ApplyReward(RewardDataModel reward)
    {
        switch (reward.type)
        {
            case RewardType.AddCard:
                // プレイヤーデッキにカード追加
                Debug.Log($"Adding card: {reward.card.letter}");
                playerStatusInstance.playerDeck.cards.Add(reward.card);
                break;
            case RewardType.IncreasePullSpeed:
                Debug.Log("Increasing pull speed");
                playerStatusInstance.pullSpeed += 0.2f;
                break;
            case RewardType.DecreaseSpiritConsumeRatio:
                Debug.Log("Decreasing spirit consume ratio");
                playerStatusInstance.SpiritConsumeRatio = Mathf.Max(0, playerStatusInstance.SpiritConsumeRatio - 0.5f);
                break;
            case RewardType.IncreaseSpiritRecoveryRatio:
                Debug.Log("Increasing spirit recovery ratio");
                playerStatusInstance.SpiritRecoveryRatio += 0.5f;
                break;
        }
    }


    public bool KokkuriIsZero() => currentKokkuriIndex >= kokkuriDatabase.kokkuris.Count;
}
