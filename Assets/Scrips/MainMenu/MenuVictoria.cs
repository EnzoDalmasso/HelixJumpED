using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuVictoria : MonoBehaviour
{
    public void MenuInicial()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SalirDelJuego()
    {
        Application.Quit();
        Debug.Log("Salir");
    }
}
