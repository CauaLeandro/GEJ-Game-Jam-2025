using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Função para carregar a cena do jogo
    public void PlayGame()
    {
        SceneManager.LoadScene("Game"); // Nome da cena
    }

    // Função para sair do jogo
    public void QuitGame()
    {
        Debug.Log("Saiu do jogo!"); // Aparece no editor
        Application.Quit(); // Fecha o jogo no build
    }
}
