using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public int bestScore;
    public int currentScore;

    public int currentLevel = 0;

    public static GameManager singlenton;

    public AudioSource winAudio;

    public TextMeshProUGUI texetPlus1;

    public GameObject pauseCanvas;
    private bool isPaused = false;
    public GameObject pauseButton;


    private void Awake()
    {
        if(singlenton == null)
        {
            singlenton=this;
        }
        else if(singlenton!=this)
        {
            Destroy(gameObject);
        }

        bestScore = PlayerPrefs.GetInt("HighScore");
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
          
        //TogglePause();
       // }
    }

    public void NextLevel()
    {
        winAudio.Play();
        currentLevel++;

        HelixController helix = FindFirstObjectByType<HelixController>();

        // ULTIMO STAGE COMPLETADO → VICTORIA
        if (currentLevel >= helix.allStages.Count)
        {
            GoToVictoryMenu();
            return;
        }

        FindFirstObjectByType<BallController>().ResetBall();
        FindFirstObjectByType<HelixController>().LoadStage(currentLevel);
    }

    public void RestartLevel()
    {
        
        singlenton.currentScore = 0;
        FindFirstObjectByType<BallController>().ResetBall();
        FindFirstObjectByType<HelixController>().LoadStage(currentLevel);
    }

    public void AddScore(int scoreToAdd)
    {
        texetPlus1.GetComponent<Animation>().Play();
        currentScore += scoreToAdd;

        if(currentScore> bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("HighScore",currentScore);
        }
    }

    public void GoToVictoryMenu()
    {
        //PlayerPrefs.SetInt("LastScore", currentScore);

        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuVictoria");
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;   // PAUSA EL JUEGO
                                   // Acá activás tu menú de pausa (Canvas)
            pauseCanvas.SetActive(true);
            pauseButton.SetActive(false);
        }
        else
        {
            Time.timeScale = 1f;   // REANUDA
                                   // Acá ocultás el menú de pausa
            pauseCanvas.SetActive(false);
            pauseButton.SetActive(true);
        }
    }

    public void SalirPausa()
    {
        isPaused = false;// ESTO ES CLAVE
        Time.timeScale = 1f;// REANUDA
                               // Acá ocultás el menú de pausa
        pauseCanvas.SetActive(false);
        pauseButton.SetActive(true);
    }

    public void MenuInicial()
    {

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void SalirDelJuego()
    {
        Application.Quit();
        Debug.Log("Salir");
    }

    public void OnPause()
    {
        TogglePause();
    }
}
