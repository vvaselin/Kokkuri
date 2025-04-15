using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    CoinMove coinmove;
    [SerializeField]
    LetterObserver letterObserver;

    //ステータス
    [SerializeField]
    private StatusManager statusManager;

    private bool isPlaying = false;
    private bool isStarting = false;
    private bool isPause = false;
    private bool isRewarding = false;

    [SerializeField]
    private Deck deck;
    [SerializeField]
    private Hand hand;
    [SerializeField]
    private DiscardArea discardArea;
    [SerializeField]
    private Field field;

    [SerializeField]
    private Pause pause;

    [SerializeField]
    private RewardPopup rewardPopup;

    private Kokkuri inputAction;

    private Vector2 move;
    private Vector2 look;

    [SerializeField]
    private RewardGenerator rewardGenerator;

    public Deck Deck { get => deck; set => deck = value; }
    public Hand Hand { get => hand; set => hand = value; }

    void OnDestroy()
    {
        if (inputAction != null)
        {
            inputAction.Disable();
            inputAction.Dispose();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        inputAction = new Kokkuri();
        inputAction.Enable();
        

        BGMController.instance.ChangeBGM("Title");
        BGMController.instance.Play();

        isPause = false;
        isPlaying = false;
        isStarting = false;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!isPause)
        {
            if (!isPlaying && !isStarting && inputAction.Player.Fire.triggered)
            {
                StartCoroutine(GameInit());
            }
            else if (isPlaying)
            {
                
                letterObserver.UpdateLetterManagement();
                coinmove.UpdateMove();
                statusManager.UpdateGauge();
                statusManager.UpdateText();

                deck.UpdateDeck();
                hand.UpdateHand();

                if (statusManager.playerStatusInstance.HP <= 0)
                {
                    CutInController.Instance.GameOvercutInAction();

                    coinmove.StopSpiritEffect();

                    isPlaying = false;
                    StartCoroutine(ResetGame());
                }

                if (statusManager.kokkuriStatusInstance.HP <= 0)
                {
                    statusManager.IncrementKokkuriIndex();

                    if (statusManager.KokkuriIsZero())
                    {
                        CutInController.Instance.CLEARcutInAction();

                        coinmove.StopSpiritEffect();

                        isPlaying = false;
                        StartCoroutine(ResetGame());
                    }
                    else
                    {
                        isPlaying = false;
                        isRewarding = true;
                        rewardPopup.ShowOptions(rewardGenerator.Instance.GetRandomRewards());
                        StartCoroutine(WaitInput());
                        //statusManager.NextKokkuri();
                        //StartCoroutine(letterObserver.ResetKokkuri());
                    }


                }

                if (letterObserver.IsPlayerLetterCompleted())
                {
                    field.RemoveCard();
                }

                if (isStarting && inputAction.Player.Select.triggered && letterObserver.CardIsNull())
                {
                    ConfirmCardSelection();
                }

            }

            if (isRewarding)
            {
                rewardPopup.UpdateRewardPopup();
            }

            if (inputAction.Player.Pause.triggered)
            {
                isPause = true;
                pause.PauseGame();
            }
        }
        else
        {
            //pause.UpdatePause();

            if (inputAction.Player.Pause.triggered)
            {
                isPause = false;
                pause.ResumeGame();
            }
        }
    }

    private IEnumerator GameInit()
    {
        BGMController.instance.Stop();

        yield return new WaitForSeconds(0.2f);

        statusManager.InitStatus();
        letterObserver.InitSetting();
        coinmove.InitSetting();

        deck.SetUp();

        BGMController.instance.ChangeBGM("Battle");

        //yield return StartCoroutine(FadeInCanvas(StatusCanvas, 0.3f));

        BGMController.instance.Play();
        CutInController.Instance.StartCutIn();

        isPlaying = true;

        isStarting = true;
    }

    private IEnumerator FadeInCanvas(CanvasGroup canvasGroup, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    private void ConfirmCardSelection()
    {
        CardController selectedCard = hand.GetSelectedCard(); // ここで選択
        if (selectedCard != null)
        {
            StartCoroutine(WaitForSwipeAndAddCard(selectedCard)); // アニメーションを待ってから追加
        }
    }

    private IEnumerator WaitForSwipeAndAddCard(CardController card)
    {
        SoundEffectController.instance.PlayCardSelect(); // SEを鳴らす
        yield return new WaitForSeconds(0.35f); // スワイプアニメーションが終わるのを待つ

        letterObserver.SetWordFromCard(card); // 文字をセット

        hand.TrashAllCard(); // 手札を全て捨てる

        yield return new WaitForSeconds(0.2f); // 文字がセットされるのを待つ
        field.AddCard(card); // アニメーション後に場に追加

        Debug.Log("カード選択: " + card.GetLetter());
    }

    private IEnumerator ResetGame()
    {
        letterObserver.ResetPlayerText();
        deck.ResetDeck();

        yield return new WaitForSeconds(2.0f);

        BGMController.instance.ChangeBGM("Title");
        BGMController.instance.Play();
        CutInController.Instance.ResetCutInAction();

        yield return new WaitForSeconds(0.3f);

        isStarting = false;

    }

    public void EndPause()
    {
        isPause = false;
    }

    IEnumerator WaitInput()
    {
        Debug.Log("WaitInput");

        yield return new WaitUntil(() => (rewardPopup.CurrentIndex != -1 && inputAction.Player.Select.triggered));
        
        rewardPopup.ConfirmSelection();
        rewardPopup.gameObject.SetActive(false);

        
        deck.ResetDeck();
        deck.SetUp();

        statusManager.NextKokkuri();
        StartCoroutine(letterObserver.ResetKokkuri());

        isRewarding = false;
        isPlaying = true;
    }
}