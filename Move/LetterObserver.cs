using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterObserver : MonoBehaviour
{
    [SerializeField]
    private Deck deck;
    [SerializeField]
    private Hand hand;
    [SerializeField]
    PlayerCardObj PlayerCardObj;


    [SerializeField]
    KokkuriDeck KokkuriDeck;
    [SerializeField]
    KokkuriField KokkuriField;
    [SerializeField]
    PlayerCardObj kokkuriCardObj;

    private CardController playerCard;
    private CardController kokkuriCard;

    //[SerializeField]
    //private CardDataBase PlayerCardDataBase;
    //private CardData playerCard;

    //[SerializeField]
    //private CardDataBase KokkuriCardDataBase;
    //private CardData kokkuriCard;

    //現在の入力文字
    private string playerInput = "";
    private string kokkuriInput = "";
    //文字の入力インデックス
    private int playerWordIndex = 0;
    private int kokkuriWordIndex = 0;

    //入力に必要な時間
    private float stayTimeThreshold = 0.5f;
    private Coroutine playerInputCoroutine;
    private Coroutine kokkuriInputCoroutine;

    [SerializeField]
    private Text PlayerText;
    [SerializeField]
    private Text KokkuriText;
    private float resetColorDelay = 1.0f; // 言霊完成後に残す時間

    //ステータス
    [SerializeField]
    private StatusManager statusManager;


    //目的文字のエフェクト
    [SerializeField]
    private ParticleSystem playerEffect;
    [SerializeField]
    private ParticleSystem kokkuriEffect;
    //入力成功エフェクト
    [SerializeField]
    private ParticleSystem SparkEffect;

    public void UpdateLetterManagement()
    {
        if(playerCard!=null) PlayerText.text = GetColoredText(playerCard.GetLetter(), playerWordIndex, "#4545ff");
        if(kokkuriCard!=null) KokkuriText.text = GetColoredText(kokkuriCard.GetLetter(), kokkuriWordIndex, "#ff4545");


    }

    public void SetWordFromCard(CardController card)
    {
        if (card == null)
        {
            Debug.LogError("SetWordFromCard: card がNULL");
            return;
        }

        playerCard = card;
        PlayerText.text = playerCard.GetLetter();
        playerWordIndex = 0;
        UpdateNextLetterEffect();
    }

    public void InitSetting()
    {
        playerWordIndex = 0;
        kokkuriWordIndex = 0;
        PlayerText.text = "";
        KokkuriText.text = "";
        playerCard = null;
        kokkuriCard = null;
        //statusManager.InitStatus();
        KokkuriDeck.SetUp();
        //playerCard = DrawCard(PlayerCardDataBase);
        kokkuriCard = DrawCard();

        UpdateNextLetterEffect();
        UpdateNextKokkuriLetterEffect();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Letter"))
        {
            string letter = other.GetComponent<CharManager>().GetCharacter();

            bool isPlayerNextLetter = (playerCard!= null && 
                                        playerWordIndex < playerCard.GetLetter().Length &&
                                        letter == playerCard.GetLetter()[playerWordIndex].ToString());
            bool isKokkuriNextLetter = (kokkuriCard != null &&
                                        kokkuriWordIndex < kokkuriCard.GetLetter().Length &&
                                        letter == kokkuriCard.GetLetter()[kokkuriWordIndex].ToString());

            //プレイヤーの文字入力
            if (isPlayerNextLetter && letter != playerInput)
            {
                playerInput = letter;
                //コルーチンが実行中なら停止
                if (playerInputCoroutine != null) StopCoroutine(playerInputCoroutine);

                playerInputCoroutine = StartCoroutine(PlayerCountStayTime(letter));
            }

            //こっくりさんの文字入力
            if (isKokkuriNextLetter && letter != kokkuriInput)
            {
                kokkuriInput = letter;
                //コルーチンが実行中なら停止
                if (kokkuriInputCoroutine != null) StopCoroutine(kokkuriInputCoroutine);

                kokkuriInputCoroutine = StartCoroutine(KokkuriCountStayTime(letter));
            }
        }
    }

    public bool IsPlayerLetterCompleted()
    {
        return (playerCard != null && playerWordIndex >= playerCard.GetLetter().Length);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Letter") &&
            other.GetComponent<CharManager>().GetCharacter() == playerInput)
        {
            if (playerInputCoroutine != null)
            {
                StopCoroutine(playerInputCoroutine);
                playerInputCoroutine = null;
            }
            playerInput = "";
        }

        if (other.CompareTag("Letter") &&
            other.GetComponent<CharManager>().GetCharacter() == kokkuriInput)
        {
            if (kokkuriInputCoroutine != null)
            {
                StopCoroutine(kokkuriInputCoroutine);
                kokkuriInputCoroutine = null;
            }
            kokkuriInput = "";
        }
    }

    //文字完成した後ちょっと残す
    private IEnumerator PlayerCountStayTime(string letter)
    {
        yield return new WaitForSeconds(stayTimeThreshold);
        playerWordIndex++;

        SoundEffectController.instance.PlayLetterInput();
        HitStpoController.Instance.SlowMotion(0.3f, 0.2f);

        UpdateNextLetterEffect();

        OnSparkEffect();

        if (IsPlayerLetterCompleted())
        {
            ApplyPlayerCardEffect(playerCard);

            //言霊リセット
            StartCoroutine(ResetPlayerTextColorAfterDelay()); 
        }
        playerInput = "";
    }

    public IEnumerator ResetKokkuri()
    {
        yield return new WaitForSeconds(1.0f);
        //言霊リセット
        ResetKokkuriWord();
    }

    private IEnumerator KokkuriCountStayTime(string letter)
    {
        yield return new WaitForSeconds(stayTimeThreshold);
        kokkuriWordIndex++;

        CoinMove coinMove = GetComponent<CoinMove>();
        coinMove.StopKokkuriPull();

        SoundEffectController.instance.PlayK_LetterInput();
        HitStpoController.Instance.SlowMotion(0.3f, 0.2f);

        UpdateNextKokkuriLetterEffect();

        OnSparkEffect();

        if (kokkuriWordIndex >= kokkuriCard.GetLetter().Length)
        {
            Debug.Log("[言霊完成] こっくりさん: " + kokkuriCard.GetLetter());
            KokkuriField.RemoveCard();

            if (statusManager.kokkuriStatusInstance.HP>0)
            {
                
            }

            ApplyKokkuriCardEffect(kokkuriCard);
            //言霊リセット
            StartCoroutine(ResetKokkuriTextColorAfterDelay());

            
        }
        kokkuriInput = "";
    }

    private void ApplyPlayerCardEffect(CardController card)
    {
        SoundEffectController.instance.PlayCardEffect(card.model.cardType);
        switch (card.model.cardType)
        {
            case CardType.Attack:
                //ダメージ=(攻撃力×カードパワー)
                statusManager.kokkuriStatusInstance.HP -= (statusManager.playerStatusInstance.ATK * card.model.power);
                if (statusManager.playerStatusInstance.ATK * card.model.power >= statusManager.kokkuriStatusInstance.HP)
                {
                    statusManager.kokkuriStatusInstance.HP = 0;
                }
                HitStpoController.Instance.Stop(0.2f, 0.1f, 0.2f);
                Debug.Log($"こっくりさんのHP:{statusManager.kokkuriStatusInstance.HP}");
                break;
            case CardType.Heal:
                statusManager.playerStatusInstance.HP += (statusManager.playerStatusInstance.ATK * card.model.power);
                if (statusManager.playerStatusInstance.HP > statusManager.playerStatusInstance.MaxHP)
                {
                    statusManager.playerStatusInstance.HP = statusManager.playerStatusInstance.MaxHP;
                }
                Debug.Log($"プレイヤーのHP:{statusManager.playerStatusInstance.HP}");
                break;
            case CardType.Buff:
                statusManager.playerStatusInstance.ATK += card.model.power;
                Debug.Log($"プレイヤーの攻撃力:{statusManager.playerStatusInstance.ATK}");
                break;
            case CardType.Debuff:
                statusManager.kokkuriStatusInstance.ATK -= card.model.power;
                if (statusManager.kokkuriStatusInstance.ATK < 1)
                {
                    statusManager.kokkuriStatusInstance.ATK = 1;
                }
                Debug.Log($"こっくりさんの攻撃力:{statusManager.kokkuriStatusInstance.ATK}");
                break;
            case CardType.Special:
                //特殊効果
                break;
        }
    }

    private void ApplyKokkuriCardEffect(CardController card)
    {
        SoundEffectController.instance.PlayCardEffect(card.model.cardType);
        switch (card.model.cardType)
        {
            case CardType.Attack:
                //ダメージ=(攻撃力×カードパワー)
                statusManager.playerStatusInstance.HP -= (statusManager.kokkuriStatusInstance.ATK * card.model.power);
                if(statusManager.kokkuriStatusInstance.ATK * card.model.power >= statusManager.playerStatusInstance.HP)
                {
                    statusManager.playerStatusInstance.HP = 0;
                }
                //ダメージ演出
                HitStpoController.Instance.KokkuriStop(0.2f, 0.1f, 0.2f);
                Debug.Log($"プレイヤーのHP:{statusManager.playerStatusInstance.HP}");
                break;
            case CardType.Heal:
                statusManager.kokkuriStatusInstance.HP += (statusManager.kokkuriStatusInstance.ATK * card.model.power);
                if (statusManager.kokkuriStatusInstance.HP > statusManager.kokkuriStatusInstance.MaxHP)
                {
                    statusManager.kokkuriStatusInstance.HP = statusManager.kokkuriStatusInstance.MaxHP;
                }
                Debug.Log($"こっくりさんのHP:{statusManager.kokkuriStatusInstance.HP}");
                break;
            case CardType.Buff:
                statusManager.kokkuriStatusInstance.ATK += card.model.power;
                Debug.Log($"こっくりさんの攻撃力:{statusManager.kokkuriStatusInstance.ATK}");
                break;
            case CardType.Debuff:
                statusManager.playerStatusInstance.ATK -= card.model.power;
                if (statusManager.playerStatusInstance.ATK < 1)
                {
                    statusManager.playerStatusInstance.ATK = 1;
                }
                Debug.Log($"プレイヤーの攻撃力:{statusManager.playerStatusInstance.ATK}");
                break;
            case CardType.Special:
                //特殊効果
                break;
        }
    }

    private string GetColoredText(string fullText, int index, string colorCode)
    {
        if (index == 0) return fullText;

        string coloredPart = $"<color={colorCode}>{fullText.Substring(0, index)}</color>";
        string remainingPart = fullText.Substring(index);
        return coloredPart + remainingPart;
    }

    IEnumerator ResetPlayerTextColorAfterDelay()
    {
        yield return new WaitForSeconds(resetColorDelay);
        playerCard = null;
        PlayerText.text = "";
        //playerWordIndex = 0;
        //UpdateNextLetterEffect();
    }

    public void ResetPlayerText()
    {
        playerCard = null;
        PlayerText.text = "";
    }

    IEnumerator ResetKokkuriTextColorAfterDelay()
    {
        yield return new WaitForSeconds(resetColorDelay);
        ResetKokkuriWord();
    }

    void ResetKokkuriWord()
    {
        KokkuriField.RemoveCard();
        kokkuriCard = DrawCard();
        KokkuriText.text = kokkuriCard.GetLetter();
        kokkuriWordIndex = 0;
        UpdateNextKokkuriLetterEffect();
    }

    public CardController DrawCard()
    {
        CardController card = KokkuriDeck.Draw();
        KokkuriField.AddCard(card);
        return card;
        //int cardID = Random.Range(0, database.cards.Count);
        //Debug.Log("DrawCard: " + cardID);
        //return Instantiate(database.cards[cardID]);
    }

    public bool CardIsNull()
    {
        return playerCard == null;
    }

    void UpdateNextLetterEffect()
    {
        if (playerEffect != null)
        {
            playerEffect.Stop();
            Light light = playerEffect.GetComponent<ParticleSystem>().GetComponentInChildren<Light>();
            if (light != null)
            {
                light.enabled = false;
            }
        }

        if (playerCard!=null&&(playerWordIndex < playerCard.GetLetter().Length))
        {
            Vector3 nextLetterPos = FindLetterPosition(playerCard.GetLetter()[playerWordIndex].ToString());
            OnLetterInput(nextLetterPos, playerEffect);
        }
    }

    void UpdateNextKokkuriLetterEffect()
    {
        if(kokkuriEffect != null)
        { 
            kokkuriEffect.Stop();
            Light light = kokkuriEffect.GetComponent<ParticleSystem>().GetComponentInChildren<Light>();
            if (light != null)
            {
                light.enabled = false;
            }
        }

        if (kokkuriWordIndex < kokkuriCard.GetLetter().Length)
        {
            Vector3 nextLetterPos = FindLetterPosition(kokkuriCard.GetLetter()[kokkuriWordIndex].ToString());
            OnLetterInput(nextLetterPos, kokkuriEffect);
        }
    }

    void OnLetterInput(Vector3 position, ParticleSystem effect)
    {
        if (effect != null)
        {
            effect.transform.position = position+Vector3.down*0.1f;
            effect.Simulate(0, true, true);
            effect.Play();
            Light light = effect.GetComponent<ParticleSystem>().GetComponentInChildren<Light>();
            if (light != null)
            {
                light.enabled = true;
            }
        }
    }

    void OnSparkEffect()
    {
        if (SparkEffect != null)
        {
            SparkEffect.transform.position = transform.position;
            SparkEffect.Play();
        }
    }

    Vector3 FindLetterPosition(string targetLetter)
    {
        GameObject[] letters = GameObject.FindGameObjectsWithTag("Letter");

        foreach (GameObject letter in letters)
        {
            CharManager charManager = letter.GetComponent<CharManager>();
            if (charManager != null && charManager.GetCharacter() == targetLetter)
            {
                return letter.transform.position;
            }
        }

        return Vector3.zero;
    }

    public CardController Spawn(string character ,int cardID)
    {
        CardController card = Instantiate(PlayerCardObj, transform).GetComponent<CardController>();
        card.Init(character,cardID);
        return card;
    }


    public string GetCurrentPlayerLetter()
    {
        return playerCard.GetLetter().Substring(0, playerWordIndex);
    }

    public string GetCurrentKokkuriLetter()
    {

        return kokkuriCard.GetLetter().Substring(0, kokkuriWordIndex);
    }

    public string GetPlayerNextLetter()
    {
        return playerCard.GetLetter()[playerWordIndex].ToString();
    }

    public string GetKokkuriNextLetter()
    {
        if (kokkuriCard == null || kokkuriCard.GetLetter().Length == 0)
        {
            Debug.LogError("GetKokkuriNextLetter: kokkuriCard is Null");
            return "";
        }

        if (kokkuriWordIndex < 0)
        {
            Debug.LogWarning("GetKokkuriNextLetter: kokkuriWordIndex < 0");
            kokkuriWordIndex = 0;
        }
        else if (kokkuriWordIndex >= kokkuriCard.GetLetter().Length)
        {
            Debug.LogWarning("GetKokkuriNextLetter: kokkuriWordIndex >= kokkuriCard.letter.Length");
            kokkuriCard = null;
            return "";
        }
        return kokkuriCard.GetLetter()[kokkuriWordIndex].ToString();
    }


}
