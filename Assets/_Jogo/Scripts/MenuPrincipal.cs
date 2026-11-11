using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    private void Awake()
    {
        // Prepara o menu tanto na abertura quanto no retorno da partida.
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Jogar()
    {
        SceneManager.LoadScene("Instalacao");
    }
}