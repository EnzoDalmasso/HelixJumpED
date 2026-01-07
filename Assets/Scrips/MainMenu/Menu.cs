using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
   public void EmpezarElJuego()
    {
        Time.timeScale = 1f;   // REANUDA
                               // Acá ocultás el menú de pausa

        //PARTIDA NUEVA → resetear scores
        PlayerPrefs.DeleteKey("HighScore");
        PlayerPrefs.Save();


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void SalirDelJuego()
    {
        Application.Quit();
        Debug.Log("Salir");
    }


}
