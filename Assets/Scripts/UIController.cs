using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public GameUIView ui;
    public PlayerMovement movement;

    [SerializeField] private int killsToComplete = 5;
    [SerializeField] private string nextLevelSceneName = "Level2";

    private PlayerModel model;

    void Start()
    {
        model = new PlayerModel(100f, killsToComplete);

        // Model → View
        model.OnHealthChanged += ui.UpdateHealth;
        model.OnKillsChanged += ui.UpdateKills;
        model.OnPlayerDied += HandleDeath;
        model.OnLevelCompleted += HandleLevelComplete;

        // View → Controller (кнопки)
        ui.RestartClicked += HandleRestart;
        ui.MainMenuClicked += HandleMainMenu;
        ui.NextLevelClicked += HandleNextLevel;
        ui.LevelCompleteMainMenuClicked += HandleLevelCompleteMainMenu;

        ui.UpdateHealth(model.CurrentHealth, model.MaxHealth);
        ui.UpdateKills(model.Kills);
    }

    void OnDestroy()
    {
        if (model != null)
        {
            model.OnHealthChanged -= ui.UpdateHealth;
            model.OnKillsChanged -= ui.UpdateKills;
            model.OnPlayerDied -= HandleDeath;
        }
        if (ui != null)
        {
            ui.RestartClicked -= HandleRestart;
            ui.MainMenuClicked -= HandleMainMenu;
        }
    }

    public void TakeDamage(float damage) => model.TakeDamage(damage);
    public void AddKill()
    {
        Debug.Log("[Controller] AddKill called");
        model.AddKill(); 
    }

    void HandleDeath()
    {
        ui.ShowGameOver();
        PauseAndFreeCursor();
    }
    void HandleLevelComplete()                      
    {
        ui.ShowLevelComplete();
        PauseAndFreeCursor();
    }
    void PauseAndFreeCursor()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (movement != null) movement.enabled = false;
    }
    void HandleRestart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void HandleMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    void HandleNextLevel()                            
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene(nextLevelSceneName);
    }
    void HandleLevelCompleteMainMenu()           
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
