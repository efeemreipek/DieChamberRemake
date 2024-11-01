using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PrimeTween;
using UnityEngine.UI;
using System;

public class PlayerUI : MonoBehaviour
{
    public static event Action OnPlayAgain;

    [SerializeField] private UIElementsSO uiElementsSO;

    private CanvasGroup backgroundCanvasGroup;

    private void OnEnable()
    {
        if(uiElementsSO.MoveAmountText == null || uiElementsSO.BackgroundGO == null || uiElementsSO.GameOverPanelGO == null ||
           uiElementsSO.PlayAgainButton == null || uiElementsSO.QuitButton == null || uiElementsSO.RestartImage == null)
        {
            uiElementsSO.MoveAmountText = GameObject.FindGameObjectWithTag("MoveAmountText").GetComponent<TextMeshProUGUI>();
            uiElementsSO.BackgroundGO = GameObject.FindGameObjectWithTag("Background");
            uiElementsSO.GameOverPanelGO = GameObject.FindGameObjectWithTag("GameOverPanel");
            uiElementsSO.PlayAgainButton = GameObject.FindGameObjectWithTag("PlayAgainButton").GetComponent<Button>();
            uiElementsSO.QuitButton = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
            uiElementsSO.RestartImage = GameObject.FindGameObjectWithTag("RestartImage").GetComponent<Image>();

            uiElementsSO.MoveAmountText.gameObject.SetActive(true);

            backgroundCanvasGroup = uiElementsSO.BackgroundGO.GetComponent<CanvasGroup>();
            backgroundCanvasGroup.alpha = 0f;

            uiElementsSO.GameOverPanelGO.transform.localScale = Vector3.zero;

            uiElementsSO.BackgroundGO.SetActive(false);
            uiElementsSO.GameOverPanelGO.SetActive(false);
        }

        PlayerMovement.OnMoveAmountChanged += PlayerMovement_OnMoveAmountChanged;
        PlayerMovement.OnMoveAmountZero += PlayerMovement_OnMoveAmountZero;
        LevelSpawner.OnLevelRestarted += LevelSpawner_OnLevelRestarted;
        LevelRestarter.OnLevelRestartProgress += LevelRestarter_OnLevelRestartProgress;
    }

    private void OnDisable()
    {
        PlayerMovement.OnMoveAmountChanged -= PlayerMovement_OnMoveAmountChanged;
        PlayerMovement.OnMoveAmountZero -= PlayerMovement_OnMoveAmountZero;
        LevelSpawner.OnLevelRestarted -= LevelSpawner_OnLevelRestarted;
        LevelRestarter.OnLevelRestartProgress -= LevelRestarter_OnLevelRestartProgress;
    }

    private void LevelRestarter_OnLevelRestartProgress(float obj)
    {
        uiElementsSO.RestartImage.fillAmount = obj;
    }

    private void PlayerMovement_OnMoveAmountChanged(int obj)
    {
        uiElementsSO.MoveAmountText.text = obj.ToString("D2");
    }

    private void PlayerMovement_OnMoveAmountZero()
    {
        uiElementsSO.BackgroundGO.SetActive(true);
        backgroundCanvasGroup = uiElementsSO.BackgroundGO.GetComponent<CanvasGroup>();
        Tween.Alpha(backgroundCanvasGroup, 1f, 1f);

        uiElementsSO.GameOverPanelGO.SetActive(true);
        uiElementsSO.PlayAgainButton.onClick.AddListener(() => OnPlayAgain?.Invoke());
        uiElementsSO.QuitButton.onClick.AddListener(() => Application.Quit());

        Tween.Scale(uiElementsSO.GameOverPanelGO.transform, 1f, 0.5f, startDelay: 0.25f);
    }

    private void LevelSpawner_OnLevelRestarted()
    {
        backgroundCanvasGroup = uiElementsSO.BackgroundGO.GetComponent<CanvasGroup>();
        backgroundCanvasGroup.alpha = 0f;

        uiElementsSO.GameOverPanelGO.transform.localScale = Vector3.zero;

        uiElementsSO.PlayAgainButton.onClick.RemoveAllListeners();
        uiElementsSO.QuitButton.onClick.RemoveAllListeners();

        uiElementsSO.BackgroundGO.SetActive(false);
        uiElementsSO.GameOverPanelGO.SetActive(false);
    }
}
