using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIView : MonoBehaviour
{
    public Image healthBarFill;
    public TextMeshProUGUI killText;
    public GameObject gameOverCanvas;
    public GameObject levelCompleteCanvas;

    public event Action RestartClicked;
    public event Action MainMenuClicked;

    public event Action NextLevelClicked;           
    public event Action LevelCompleteMainMenuClicked;

    public void UpdateHealth(float current, float max) => healthBarFill.fillAmount = current / max;
    public void UpdateKills(int count) => killText.text = "Убито: " + count;
    public void ShowGameOver() => gameOverCanvas.SetActive(true);
    public void ShowLevelComplete() => levelCompleteCanvas.SetActive(true); 

    public void RestartGame() => RestartClicked?.Invoke();
    public void LoadMainMenu() => MainMenuClicked?.Invoke();
    public void OnNextLevelButton() => NextLevelClicked?.Invoke(); 
    public void OnLevelCompleteMainMenuButton() => LevelCompleteMainMenuClicked?.Invoke();
}
