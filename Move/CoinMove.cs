using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;
using UnityEngine.InputSystem;

public class CoinMove : MonoBehaviour
{
    [SerializeField]
    private Transform Torii;

    [SerializeField]
    private CharacterController controller;
    private float defaultspeed = 1.0f;

    //ステータス
    [SerializeField]
    private StatusManager statusManager;

    [SerializeField]
    private ParticleSystem PlayerSpiritEffect;
    private bool PlayerIsMoving = false;
    [SerializeField]
    private ParticleSystem KokkuriSpiritEffect;

    //こっくりさん引っ張りタイマー
    private float pullInterval = 3.0f; //引っ張り間隔
    private float pullDuration = 1.0f;　//引っ張り継続時間
    private float pullTimer; //引っ張り感覚カウント
    private float pullDurationTimer; //引っ張り継続時間カウント
    private bool isKokkuriPulling = false;
    //引っ張り乱数用
    private float pullIntervalMax = 3.5f;
    private float pullIntervalMin = 1.5f;
    private float pullDurationMax = 2.0f;
    private float pullDurationMin = 1.0f;
    //振動用
    private float frequency = 100.0f; // 振動の速さ
    private float amplitude = 5.0f; // 振動の強さ
    //引っ張り方向
    private Vector3 targetPosition;

    private InputAction inputAction;


    //こっくりさんの行動パターン
    private enum KokkuriState
    {
        Idle,
        RandomPull,
        Vibrating,
        ToTarget
    }
    private KokkuriState kokkuriState;


    public void UpdateMove()
    {
        //こっくりさんのHPが0になったら引っ張りを終了
        if (statusManager.kokkuriStatusInstance.HP <= 0)
        {
            PullInit();
            isKokkuriPulling = false;
        }

        Vector3 playerInput = HandlePlayerMove();
        Vector3 kokkuriInput = HandleKokkuriMove();

        //引っ張り矢印の表示
        PlayerIsMoving = (playerInput!=Vector3.zero);
        //MoveArrow(playerInput, kokkuriInput);

        MoveSpiritEffect(playerInput, kokkuriInput);
        controller.Move((playerInput + kokkuriInput) * Time.deltaTime);

    }

    //初期化
    public void InitSetting()
    {
        Vector3 startPos = new Vector3(Torii.position.x, transform.position.y, Torii.position.z);
        transform.DOMove(startPos,0.2f);
        PullInit();
        isKokkuriPulling = false;
    }

    public void SetKokkuriPullSetting()
    {
        pullDurationMax = statusManager.kokkuriStatusInstance.pullDurationMax;
        pullDurationMin = statusManager.kokkuriStatusInstance.pullDurationMax - 2.0f;
        pullIntervalMax = statusManager.kokkuriStatusInstance.pullIntervalMax;
        pullIntervalMin = statusManager.kokkuriStatusInstance.pullIntervalMax - 1.0f;
    }

    Vector3 HandlePlayerMove()
    {
        //こっくりさん霊力使用中はプレイヤーも霊力必要
        float movespeed = (isKokkuriPulling)? 0.0f: defaultspeed;

        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.JoystickButton0))
        {
            if (statusManager.playerStatusInstance.Spirit > 0)
            {
                statusManager.playerStatusInstance.Spirit -= statusManager.playerStatusInstance.SpiritConsumeRatio * Time.deltaTime;
                if (statusManager.playerStatusInstance.Spirit < 0)
                {
                    statusManager.playerStatusInstance.Spirit = 0;
                }
                movespeed += statusManager.playerStatusInstance.pullSpeed;
            }
        }
        else if (statusManager.playerStatusInstance.Spirit < statusManager.playerStatusInstance.MaxSpirit)
        {
            statusManager.playerStatusInstance.Spirit += statusManager.playerStatusInstance.SpiritRecoveryRatio * Time.deltaTime;
            statusManager.playerStatusInstance.Spirit = Mathf.Min(
                statusManager.playerStatusInstance.Spirit,
                statusManager.playerStatusInstance.MaxSpirit
            );
        }
        
        return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * movespeed;
    }

    Vector3 HandleKokkuriMove()
    {
        Vector3 move = Vector3.zero;
        if (isKokkuriPulling)
        {
            if (pullDurationTimer > 0)
            {
                switch (kokkuriState)
                {
                    case KokkuriState.RandomPull:
                        move = targetPosition * statusManager.kokkuriStatusInstance.pullSpeed;
                        break;

                    case KokkuriState.Vibrating:
                        move = VibPosition() * statusManager.kokkuriStatusInstance.pullSpeed;
                        break;

                    case KokkuriState.ToTarget:
                        move = ToTargetPos() * statusManager.kokkuriStatusInstance.pullSpeed;
                        break;
                    case KokkuriState.Idle:
                        move = Vector3.zero;
                        break;
                }
                
                statusManager.kokkuriStatusInstance.Spirit -= statusManager.kokkuriStatusInstance.SpiritConsumeRatio * Time.deltaTime;
                if (statusManager.kokkuriStatusInstance.Spirit < 0)
                {
                    statusManager.kokkuriStatusInstance.Spirit = 0;
                }

                pullDurationTimer -= Time.deltaTime;
            }
            else
            {
                isKokkuriPulling = false;
            }
        }
        else
        {
            pullTimer -= Time.deltaTime;

            if (pullTimer <= 0 && statusManager.kokkuriStatusInstance.Spirit > 0)
            {
                StartKokkuriPull();
            }

            if (statusManager.kokkuriStatusInstance.Spirit < statusManager.kokkuriStatusInstance.MaxSpirit)
            {
                statusManager.kokkuriStatusInstance.Spirit += statusManager.kokkuriStatusInstance.SpiritRecoveryRatio * Time.deltaTime;
                statusManager.kokkuriStatusInstance.Spirit = Mathf.Min(
                    statusManager.kokkuriStatusInstance.Spirit,
                    statusManager.kokkuriStatusInstance.MaxSpirit
                );
            }
        }

        return move;
    }

    void StartKokkuriPull()
    {
        RandomWithWeight randomWithWeight = new RandomWithWeight();
        int action = randomWithWeight.Choose();

        switch (action)
        {
            case 0:
                Vector2 randomDirection2D = Random.insideUnitCircle.normalized;
                targetPosition = new Vector3(randomDirection2D.x, 0, randomDirection2D.y);

                kokkuriState = KokkuriState.RandomPull;
                break;

            case 1:
                kokkuriState = KokkuriState.Vibrating;
                break;

            case 2:
                SetKokkuriTarget(); // 目的の文字を取得して移動開始
                kokkuriState = KokkuriState.ToTarget;
                break;

        }
        PullInit();
    }

    public void StopKokkuriPull()
    {
        isKokkuriPulling = false;
        kokkuriState = KokkuriState.Idle;
    }

    public void PullInit()
    {
        isKokkuriPulling = true;
        pullDuration = Random.Range(pullDurationMin, pullDurationMax);
        pullDurationTimer = pullDuration;
        pullInterval = Random.Range(pullIntervalMin, pullIntervalMax);
        pullTimer = pullInterval;
    }

    Vector3 VibPosition()
    {
        Vector3 randomDirection = Random.onUnitSphere;
        Vector3 vibration = randomDirection * Mathf.Sin(Time.time * frequency) * amplitude;
        return new Vector3(vibration.x, 0, vibration.z);
    }

    void SetKokkuriTarget()
    {
        string nextLetter = GetComponent<LetterObserver>().GetKokkuriNextLetter();
        if (string.IsNullOrEmpty(nextLetter))
        {
            kokkuriState = KokkuriState.Idle;
            return;
        }

        GameObject targetLetter = FindNextTargetLetter();
        if (targetLetter != null)
        {
            targetPosition = targetLetter.transform.position;
        }
        else
        {
            kokkuriState = KokkuriState.Idle; // 目標がない場合、Idle状態に戻す
        }
    }

    Vector3 ToTargetPos()
    {
        if (targetPosition != Vector3.zero)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            return new Vector3(direction.x, 0, direction.z);
        }
        return Vector3.zero;
    }

    GameObject FindNextTargetLetter()
    {
        GameObject[] letters = GameObject.FindGameObjectsWithTag("Letter"); // すべての文字オブジェクトを取得
        float minDistance = float.MaxValue;
        GameObject nearestLetter = null;

        LetterObserver letterObserver = GetComponent<LetterObserver>();

        foreach (GameObject letter in letters)
        {
            CharManager charManager = letter.GetComponent<CharManager>();
            if (charManager != null)
            {
                string letterChar = charManager.GetCharacter();
                
                if (letterChar == letterObserver.GetKokkuriNextLetter()) // 次の文字と一致
                {
                    float distance = Vector3.Distance(transform.position, charManager.GetPos());
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestLetter = letter;
                    }
                }
            }
        }
        return nearestLetter;
    }

    void MoveSpiritEffect(Vector3 playerVec, Vector3 kokkuriVec)
    {
        PlayerSpiritEffect.transform.position = transform.position;
        KokkuriSpiritEffect.transform.position = transform.position;

        if (PlayerIsMoving)
        {
            float arrowScale1 = ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.JoystickButton0))&&statusManager.playerStatusInstance.Spirit>0) ? 2.0f : 1.0f;
            PlayerSpiritEffect.transform.GetChild(0).localScale = new Vector3(1.0f, 1.0f, arrowScale1);
            PlayerSpiritEffect.transform.GetChild(1).localScale = new Vector3(1.0f, 1.0f, arrowScale1);


            if (playerVec != Vector3.zero)
            {
                Quaternion targetRotation1 = Quaternion.LookRotation(playerVec);
                PlayerSpiritEffect.transform.rotation = Quaternion.Euler(-180, targetRotation1.eulerAngles.y, 0);
            }

            if (!PlayerSpiritEffect.isPlaying)
                StartSpiritEffect(PlayerSpiritEffect);
        }
        else if(!PlayerIsMoving && PlayerSpiritEffect.isPlaying)
        {
            PlayerSpiritEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        if (isKokkuriPulling)
        {
            if (kokkuriVec != Vector3.zero)
            {
                Quaternion targetRotation2 = Quaternion.LookRotation(kokkuriVec);
                KokkuriSpiritEffect.transform.rotation = Quaternion.Euler(-180, targetRotation2.eulerAngles.y, 0);
            }

            if (!KokkuriSpiritEffect.isPlaying)
                StartSpiritEffect(KokkuriSpiritEffect);
        }
        else if (!isKokkuriPulling && KokkuriSpiritEffect.isPlaying)
        {
            KokkuriSpiritEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    void StartSpiritEffect(ParticleSystem effect)
    {
        effect.Play();
    }

    public void StopSpiritEffect()
    {
        PlayerSpiritEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        KokkuriSpiritEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

}
