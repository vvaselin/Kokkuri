using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hand : MonoBehaviour
{
    [SerializeField]
    private Field field;
    [SerializeField]
    private DiscardArea discardArea;

    List<CardController> cards = new List<CardController>();
    private int select = -1;
    private float lastInputTime = 0f;
    private const float resetTime = 3.0f;

    private float stickCooldown = 0.2f; // クールダウン時間（秒）
    private float lastStickMoveTime = 0f; // 最後にスティックを動かした時間

    private Kokkuri inputAction;

    Dictionary<CardController, int> originalSiblingIndex = new Dictionary<CardController, int>();

    void OnDestroy()
    {
        if (inputAction != null)
        {
            inputAction.Disable();
            inputAction.Dispose();
        }
    }


    private void Start()
    {
        inputAction = new Kokkuri();
        inputAction.Enable();
    }

    public void UpdateHand()
    {
        HandleSelection();
        CheckResetSelection();
    }

    public void AddCard(CardController card)
    {
        cards.Add(card);
        card.transform.SetParent(transform);
        card.gameObject.SetActive(true);
        //originalSiblingIndex[card] = card.transform.GetSiblingIndex();
    }

    

    public void ArrangeCards()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            float center = (cards.Count - 1) / 2.0f;
            float interval = 180f;
            float x = (i - center) * interval;

            var card = cards[i];

            //card.transform.localPosition = new Vector3(x, -500f, 0);

            card.transform.DOLocalMove(new Vector3(x, 0, 0), 0.2f).SetEase(Ease.OutQuad);
            card.transform.DOScale(Vector3.one, 0.2f);
        }
    }

    private void HandleSelection()
    {
        float input = inputAction.Player.Look.ReadValue<float>(); // スティックの入力
        float scroll = Input.mouseScrollDelta.y; // マウスホイール入力

        bool moved = false;

        if (Time.time - lastStickMoveTime > stickCooldown) // クールダウン判定
        {
            if (input > 0.3f || scroll < -0.05f) // 右 or 下にスクロール
            {
                if (select == -1) select = 0;
                else select = Mathf.Min(select + 1, cards.Count - 1);

                moved = true;
            }
            else if (input < -0.3f || scroll > 0.05f) // 左 or 上スクロール
            {
                if (select == -1) select = cards.Count - 1;
                else select = Mathf.Max(select - 1, 0);

                moved = true;
            }

            if (moved)
            {
                lastStickMoveTime = Time.time;
                lastInputTime = Time.time;
                UpdateSelection();
            }
        }
    }


    private void CheckResetSelection()
    {
        if (Time.time - lastInputTime > resetTime)
        {
            select = -1;
            UpdateSelection();
        }
    }

    public void SelectCard(int index)
    {
        if (index >= 0 && index < cards.Count)
        {
            select = index;
            lastInputTime = Time.time;
            UpdateSelection();
        }
    }

    private void UpdateSelection()
    {
        int sibling = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            
            var card = cards[i];
            var t = card.transform;
            t.DOComplete();

            if (i == select)
            {
                t.transform.localScale = Vector3.one * 1.5f;
                t.DOLocalMoveY(50f, 0.1f);
                t.SetAsLastSibling();
            }
            else
            {
                t.transform.localScale = Vector3.one;
                t.DOLocalMoveY(0f, 0.1f);
                t.SetSiblingIndex(sibling++);
            }
        }
    }

    public CardController GetSelectedCard()
    {
        if (select == -1 || cards.Count == 0) return null;

        int currentIndex = select;
        CardController card = cards[currentIndex];

        select = -1; // アニメーション中に別のカードを選択しないようにする

        // アニメーションを開始し、完了後に処理をする
        StartCoroutine(SwipeCardAnimation(card, currentIndex));

        return card;
    }

    private IEnumerator SwipeCardAnimation(CardController card, int index)
    {
        float duration = 0.3f; // アニメーション時間
        card.transform.DOLocalMoveY(card.transform.localPosition.y + 200, duration)
            .SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(duration); // アニメーションが終わるまで待機

        // カードリストから削除
        if (index < cards.Count && cards[index] == card)
        {
            cards.RemoveAt(index);
        }
    }

    public void TrashAllCard()
    {
        StartCoroutine(TrashAllCardWithAnimation());
    }

    public int GetSelectedIndex()
    {
        return select;
    }

    public int GetCardCount()
    {
        return cards.Count;
    }

    private IEnumerator TrashAllCardWithAnimation()
    {
        float duration = 0.4f; // アニメーション時間

        while (cards.Count > 0)
        {
            CardController card = cards[0];
            cards.RemoveAt(0);

            // 右にスワイプアニメーション
            card.transform.DOLocalMoveX(card.transform.localPosition.x + 500, duration)
                .SetEase(Ease.InOutQuad);

            yield return new WaitForSeconds(0.1f); // ちょっとずつズレてアニメーション

            discardArea.AddCard(card);
        }

        select = -1; // 選択リセット
    }
}
