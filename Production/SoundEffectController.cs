using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectController : MonoBehaviour
{
    public static SoundEffectController instance;

    public AudioClip CardDeal;
    public AudioClip CardSelect;
    public AudioClip Attack;
    public AudioClip LetterInput;
    public AudioClip K_LetterInput;
    public AudioClip Heal;
    public AudioClip Buff;
    public AudioClip Debuff;
    public AudioClip KokkuriAppear;

    AudioSource se;

    private float cooldown = 0.2f;
    private float lastPlayTime = -1f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        se = GetComponent<AudioSource>();
    }

    public void PlayCardDeal()
    {
        se.PlayOneShot(CardDeal);
    }

    public void PlayCardSelect()
    {
        se.PlayOneShot(CardSelect);
    }

    public void PlayK_LetterInput()
    {
        se.PlayOneShot(K_LetterInput);
    }

    public void PlayCardEffect(CardType cardType)
    {
        switch (cardType)
        {
            case CardType.Attack:
                se.PlayOneShot(Attack);
                break;
            case CardType.Heal:
                se.PlayOneShot(Heal);
                break;
            case CardType.Buff:
                se.PlayOneShot(Buff);
                break;
            case CardType.Debuff:
                se.PlayOneShot(Debuff);
                break;
        }
    }

    public void PlayLetterInput()
    {
        se.PlayOneShot(LetterInput);
    }

    public void PlayKokkuriAppear()
    {
        if (Time.time - lastPlayTime < cooldown) return;
        se.PlayOneShot(KokkuriAppear);
        lastPlayTime = Time.time;
    }
}
