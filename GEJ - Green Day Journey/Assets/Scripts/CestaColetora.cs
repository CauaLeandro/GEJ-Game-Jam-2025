using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CestaColetora : MonoBehaviour
{
    public TextMeshProUGUI contadorTexto;
    public Image telaEscura;
    public string proximaCena;
    public float duracaoFade = 1f;
    public MonoBehaviour spawner;

    private int contador = 0;
    private bool transicaoIniciada = false;

    void Start()
    {
        AtualizarTexto();
        if (telaEscura != null)
            telaEscura.color = new Color(0, 0, 0, 0); // transparente
    }

    void OnTriggerEnter(Collider other)
    {
        if (transicaoIniciada)
            return;

        if (other.gameObject.CompareTag("Cenoura"))
        {
            Destroy(other.gameObject);
            contador++;
            AtualizarTexto();

            if (contador >= 25)
                StartCoroutine(FadeESwitchScene());
        }
        else if (other.gameObject.CompareTag("Pedra"))
        {
            Destroy(other.gameObject);
            contador--;
            if (contador < 0)
                contador = 0;
            AtualizarTexto();
        }
    }

    void AtualizarTexto()
    {
        if (contadorTexto != null)
            contadorTexto.text = "Cenouras: " + contador;
    }

    System.Collections.IEnumerator FadeESwitchScene()
    {
        transicaoIniciada = true;

        if (spawner != null)
            spawner.enabled = false;

        float tempo = 0f;
        Color corInicial = telaEscura.color;
        Color corFinal = new Color(0, 0, 0, 1); // preta

        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            telaEscura.color = Color.Lerp(corInicial, corFinal, tempo / duracaoFade);
            yield return null;
        }

        SceneManager.LoadScene(proximaCena);
    }
}