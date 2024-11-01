using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scriptable Objects/Create UI Elements SO")]
public class UIElementsSO : ScriptableObject
{
	public TextMeshProUGUI MoveAmountText;
    public GameObject BackgroundGO;
    public GameObject GameOverPanelGO;
    public Button PlayAgainButton;
    public Button QuitButton;
    public Image RestartImage;
    public GameObject GameStartPanelGO;
    public Button PlayButton;
    public Button QuitButton2;
}
