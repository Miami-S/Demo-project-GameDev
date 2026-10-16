using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Панель паузы (весь этот объект)
    public GameObject pauseMenuUI;

    // Игрок в паузе или нет
    public static bool isPaused = false;

    void Start()
    {
        // На старте пауза выключена
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Update()
    {
        // По Escape включаем/выключаем паузу
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;

        // Прячем курсор обратно
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;

        // Показываем курсор, чтобы можно было жать по кнопкам
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isPaused = true;
    }


    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

}
