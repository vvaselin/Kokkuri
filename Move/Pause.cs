using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup pauseCanvas;

    [SerializeField]
    private Button Resume;
    [SerializeField]
    private Button End;

    [SerializeField]
    private Slider MasterVolumeSlider;
    [SerializeField]
    private Slider BGMVolumeSlider;
    [SerializeField]
    private Slider SEVolumeSlider;

    public Image fadePanel;

    public AudioMixer AudioMixer;

    List<Button> buttons = new List<Button>();

    private float originalTimeScale = 1.0f;

    public float fadeDuration = 1.0f;

    private void Start()
    {
        LoadAudioSettings();
    }

    private void LoadAudioSettings()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float seVolume = PlayerPrefs.GetFloat("SEVolume", 1f);

        float masterDB = Mathf.Log10(Mathf.Max(masterVolume, 0.0001f)) * 20f;
        float bgmDB = Mathf.Log10(Mathf.Max(bgmVolume, 0.0001f)) * 20f;
        float seDB = Mathf.Log10(Mathf.Max(seVolume, 0.0001f)) * 20f;

        AudioMixer.SetFloat("Master", masterDB);
        AudioMixer.SetFloat("BGM", bgmDB);
        AudioMixer.SetFloat("SE", seDB);

        MasterVolumeSlider.value = masterVolume;
        BGMVolumeSlider.value = bgmVolume;
        SEVolumeSlider.value = seVolume;
    }
    public void UpdatePause()
    {
        
    }

    public void PauseGame()
    {
        originalTimeScale = Time.timeScale;

        Time.timeScale = 0;
        pauseCanvas.alpha = 1;
        pauseCanvas.interactable = true;
        pauseCanvas.blocksRaycasts = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = originalTimeScale;
        CutInController.Instance.DOComplete();
        pauseCanvas.alpha = 0;
        pauseCanvas.interactable = false;
        pauseCanvas.blocksRaycasts = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        originalTimeScale = 1.0f;
        CutInController.Instance.DOComplete();
        StartCoroutine(FadeOutAndLoadScene());
    }

    public void EndGaeme()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }

    public IEnumerator FadeOutAndLoadScene()
    {
        fadePanel.enabled = true;                 // パネルを有効化
        float elapsedTime = 0.0f;                 // 経過時間を初期化
        Color startColor = fadePanel.color;       // フェードパネルの開始色を取得
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1.0f); // フェードパネルの最終色を設定

        // フェードアウトアニメーションを実行
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;                        // 経過時間を増やす
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);  // フェードの進行度を計算
            fadePanel.color = Color.Lerp(startColor, endColor, t); // パネルの色を変更してフェードアウト
            yield return null;                                     // 1フレーム待機
        }

        fadePanel.color = endColor;  // フェードが完了したら最終色に設定
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // シーンをロードしてメニューシーンに遷移
    }

    public void SetMasterVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        AudioMixer.SetFloat("Master", dB);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetBGMVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        AudioMixer.SetFloat("BGM", dB);
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSEVolume(float volume)
    {
        float dB = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        AudioMixer.SetFloat("SE", dB);
        PlayerPrefs.SetFloat("SEVolume", volume);
    }

}
