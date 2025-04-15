using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CutInController : MonoBehaviour
{
    public static CutInController Instance;

    [SerializeField] 
    private RectTransform cutInMask;

    [SerializeField]
    private RectTransform KokkuriNAME;

    [SerializeField]
    private RectTransform ClearMask;
    [SerializeField]
    private RectTransform GameOverMask;

    [SerializeField]
    private RectTransform StartMask;

    [SerializeField]
    private Volume postFXvolume;
    private Vignette vignette;
    private Color Black = new Color(0, 0, 0, 1);
    private Color Red = new Color(1, 0, 0, 1);
    private Color White = new Color(1, 1, 1, 1);


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        postFXvolume.profile.TryGet(out vignette);
    }

    public void CutInAction()
    {
        var sequence = DOTween.Sequence();

        sequence.AppendInterval(0.1f)
            .Append(cutInMask.DOSizeDelta(new Vector2(1920.0f, 100.0f), 0.1f))
            .AppendInterval(1.0f)
            .Append(cutInMask.DOSizeDelta(new Vector2(1920.0f, 0.0f), 0.2f));
    }

    public void StartCutIn()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(StartMask.DOSizeDelta(new Vector2(1920.0f, 0.0f), 0.1f));
    }

    public void CLEARcutInAction()
    {
        var sequence = DOTween.Sequence();

        sequence
            .Append(ClearMask.DOSizeDelta(new Vector2(1920.0f, 200.0f), 0.2f))
            .AppendInterval(2.0f)
            .Append(ClearMask.DOSizeDelta(new Vector2(1920.0f, 0.0f), 0.2f));
    }

    public void GameOvercutInAction()
    {
        var sequence = DOTween.Sequence();

        sequence
            .Append(GameOverMask.DOSizeDelta(new Vector2(1920.0f, 200.0f), 0.2f))
            .AppendInterval(2.0f)
            .Append(GameOverMask.DOSizeDelta(new Vector2(1920.0f, 0.0f), 0.2f));
    }

    public void ResetCutInAction()
    {
        var sequence = DOTween.Sequence();
        sequence
            .Append(StartMask.DOSizeDelta(new Vector2(1920f, 100f), 0.3f));
    }

    public void DamageAction()
    {
        if (vignette == null) return;
        var sequence = DOTween.Sequence();

        sequence.Append(DOVirtual.Float(0f, 1f, 0.3f, value =>
        {
            vignette.color.Override(Color.Lerp(Black, Red, value));
        }))
        .Append(DOVirtual.Float(0f, 1f, 0.2f, value =>
        {
            vignette.color.Override(Color.Lerp(Red, Black, value));
        }));
    }

    public void AttackAction()
    {
        if (vignette == null) return;
        var sequence = DOTween.Sequence();

        sequence.Append(DOVirtual.Float(0f, 1f, 0.3f, value =>
        {
            vignette.color.Override(Color.Lerp(Black, White, value));
        }))
        .Append(DOVirtual.Float(0f, 1f, 0.2f, value =>
        {
            vignette.color.Override(Color.Lerp(White, Black, value));
        }));
    }

    public void DOComplete()
    {
        cutInMask.DOComplete();
        KokkuriNAME.DOComplete();
        ClearMask.DOComplete();
        GameOverMask.DOComplete();
        StartMask.DOComplete();
        vignette.color.Override(Black);
    }
}
