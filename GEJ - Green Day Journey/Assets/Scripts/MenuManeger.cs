using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Fun��o para carregar a cena do jogo
    public void PlayGame()
    {
        SceneManager.LoadScene("Game"); // Nome da cena
    }

    // Fun��o para sair do jogo
    public void QuitGame()
    {
        Debug.Log("Saiu do jogo!"); // Aparece no editor
        Application.Quit(); // Fecha o jogo no build
    }
}
