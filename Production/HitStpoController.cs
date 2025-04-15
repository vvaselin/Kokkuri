using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStpoController : MonoBehaviour
{
    public static HitStpoController Instance;
    private GameObject CAMERA;

    private void Awake()
    {
        CAMERA = GameObject.Find("Main Camera");
        Instance = this;
    }

    public void Stop(float _ftime, float shakeIntensity = 0.2f, float shakeDuration = 0.2f)
    {
        StartCoroutine(HitStopAction(_ftime, shakeIntensity, shakeDuration));
    }

    public void KokkuriStop(float _ftime, float shakeIntensity = 0.2f, float shakeDuration = 0.2f)
    {
        StartCoroutine(KokkuriHitStopAction(_ftime, shakeIntensity, shakeDuration));
    }

    public void SlowMotion(float slowTimeScale, float duration)
    {
        StartCoroutine(SlowMotionAction(slowTimeScale, duration));
    }



    private IEnumerator HitStopAction(float _ftime, float shakeIntensity, float shakeDuration)
    {
        Time.timeScale = 0.0f;

        if (CAMERA != null)
        {
            StartCoroutine(CameraShake(shakeIntensity, shakeDuration));
        }

        if (CutInController.Instance != null)
        {
            CutInController.Instance.AttackAction();
        }


        yield return new WaitForSecondsRealtime(_ftime);

        Time.timeScale = 1.0f;
    }

    private IEnumerator KokkuriHitStopAction(float _ftime, float shakeIntensity, float shakeDuration)
    {
        Time.timeScale = 0.0f;

        if (CAMERA != null)
        {
            StartCoroutine(CameraShake(shakeIntensity, shakeDuration));
        }

        if (CutInController.Instance != null)
        {
            CutInController.Instance.DamageAction();
        }

        yield return new WaitForSecondsRealtime(_ftime);

        Time.timeScale = 1.0f;
    }

    private IEnumerator SlowMotionAction(float slowTimeScale, float duration)
    {
        float originalTimeScale = 1.0f;
        Time.timeScale = slowTimeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = originalTimeScale;
    }


    private IEnumerator CameraShake(float intensity, float duration)
    {
        if (CAMERA == null) yield break;

        Transform camTransform = CAMERA.transform;
        Vector3 originalCameraPosition = camTransform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float offsetX = Random.Range(-intensity, intensity);
            float offsetY = Random.Range(-intensity, intensity);

            camTransform.position = originalCameraPosition + new Vector3(offsetX, offsetY, 0);
            elapsedTime += Time.unscaledDeltaTime;

            yield return null; // 次のフレームまで待つ
        }

        camTransform.position = originalCameraPosition; // カメラ位置を元に戻す
    }
}
