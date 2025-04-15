using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RewardPopup : MonoBehaviour
{
    [SerializeField] private Transform optionParent;
    [SerializeField] private GameObject optionUIPrefab;
    [SerializeField] private StatusManager statusManager;

    private List<RewardController> optionUIs = new List<RewardController>();
    private int currentIndex = -1;
    private float inputCooldown = 0.2f;
    private float lastInputTime;

    private Kokkuri inputAction;

    public int CurrentIndex { get => currentIndex; set => currentIndex = value; }

    private void Awake()
    {
        inputAction = new Kokkuri();
        inputAction.Enable();
    }

    public void ShowOptions(List<RewardData> rewards)
    {
        currentIndex = -1;
        // ä˘ë∂ÇÃUIçÌèú
        foreach (Transform child in optionParent)
        {
            Destroy(child.gameObject);
        }

        optionUIs.Clear();

        // UIê∂ê¨
        foreach (var reward in rewards)
        {
            RewardController ui = Instantiate(optionUIPrefab, optionParent).GetComponent<RewardController>();
            ui.Init(reward);
            optionUIs.Add(ui);
        }

        gameObject.SetActive(true);
        UpdateSelection();
    }

    public void UpdateRewardPopup()
    {
        float input = inputAction.Player.Look.ReadValue<float>();
        float scroll = Input.mouseScrollDelta.y;

        if (Time.time - lastInputTime > inputCooldown)
        {
            if (input > 0.3f || scroll < -0.05f)
            {
                CurrentIndex = Mathf.Min(CurrentIndex + 1, optionUIs.Count - 1);
                lastInputTime = Time.time;
                UpdateSelection();
            }
            else if (input < -0.3f || scroll > 0.05f)
            {
                CurrentIndex = Mathf.Max(CurrentIndex - 1, 0);
                lastInputTime = Time.time;
                UpdateSelection();
            }
        }
    }

    private void UpdateSelection()
    {
        for (int i = 0; i < optionUIs.Count; i++)
        {
            optionUIs[i].transform.localScale = i == CurrentIndex ? Vector3.one * 1.2f : Vector3.one;
        }
    }

    public void ConfirmSelection()
    {
        var selectedReward = optionUIs[CurrentIndex].GetReward();
        statusManager.ApplyReward(selectedReward);
        gameObject.SetActive(false);
    }


}
