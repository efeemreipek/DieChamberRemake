using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using PrimeTween;
using UnityEngine.EventSystems;

public class UIManager : Singleton<UIManager>
{
	public static event Action OnGameStarted;
	public static event Action OnPlayAgain;

	public GameObject BackgroundGO;
	public GameObject GameStartPanelGO;
	public GameObject GameOverPanelGO;
	public TextMeshProUGUI MoveAmountText;
	public TextMeshProUGUI InstructionsText;
	public Image RestartImage;
	public Button PlayButton;
	public Button QuitButton;
	public Button PlayAgainButton;
	public Button QuitButton2;

	private CanvasGroup backgroundCanvasGroup;

    private void OnEnable()
    {
        PlayerMovement.OnMoveAmountZero += PlayerMovement_OnMoveAmountZero;
        PlayerMovement.OnMoveAmountChanged += PlayerMovement_OnMoveAmountChanged;
        LevelRestarter.OnLevelRestartProgress += LevelRestarter_OnLevelRestartProgress;
        LevelSpawner.OnGameEnded += LevelSpawner_OnGameEnded;
        LevelSpawner.OnLevelLoaded += LevelSpawner_OnLevelLoaded;
    }

    private void OnDisable()
    {
        PlayerMovement.OnMoveAmountZero -= PlayerMovement_OnMoveAmountZero;
        PlayerMovement.OnMoveAmountChanged -= PlayerMovement_OnMoveAmountChanged;
        LevelRestarter.OnLevelRestartProgress -= LevelRestarter_OnLevelRestartProgress;
        LevelSpawner.OnGameEnded -= LevelSpawner_OnGameEnded;
        LevelSpawner.OnLevelLoaded -= LevelSpawner_OnLevelLoaded;
    }

    private void Start()
    {
		backgroundCanvasGroup = BackgroundGO.GetComponent<CanvasGroup>();

        GameOverPanelGO.SetActive(false);
		MoveAmountText.gameObject.SetActive(false);

        EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
    }

    private void LevelSpawner_OnLevelLoaded(LevelSO level)
    {
        StartCoroutine(Instruction(level.LevelInstruction));
    }

    private void LevelSpawner_OnGameEnded()
    {
        MoveAmountText.gameObject.SetActive(false);
        BackgroundGO.SetActive(true);
        GameStartPanelGO.SetActive(true);

        EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
    }

    private void PlayerMovement_OnMoveAmountZero()
    {
        BackgroundGO.SetActive(true);
        Tween.Alpha(backgroundCanvasGroup, 1f, 1f);

        GameOverPanelGO.SetActive(true);

        Tween.Scale(GameOverPanelGO.transform, 1f, 0.5f, startDelay: 0.25f);

        EventSystem.current.SetSelectedGameObject(PlayAgainButton.gameObject);
    }

    private void PlayerMovement_OnMoveAmountChanged(int obj)
    {
        MoveAmountText.text = $"REMAINING MOVES\r\n<size=180>{obj.ToString("D2")}</size>";
    }

    private void LevelRestarter_OnLevelRestartProgress(float obj)
    {
        RestartImage.fillAmount = obj;
    }

    private IEnumerator Instruction(string text)
    {
        InstructionsText.text = "";
        yield return new WaitForSeconds(0.1f);
        foreach(char c in text)
        {
            InstructionsText.text += c;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void Play()
	{
		BackgroundGO.SetActive(false);
        GameStartPanelGO.SetActive(false);
        Debug.Log("Before OnGameStarted");
		OnGameStarted?.Invoke();
        Debug.Log("After OnGameStarted");
        MoveAmountText.gameObject.SetActive(true);
    }
	public void Quit()
	{
		Application.Quit();
	}
	public void PlayAgain()
	{
        backgroundCanvasGroup.alpha = 0f;

        GameOverPanelGO.transform.localScale = Vector3.zero;

        BackgroundGO.SetActive(false);
        GameOverPanelGO.SetActive(false);

        OnPlayAgain?.Invoke();
    }
}
