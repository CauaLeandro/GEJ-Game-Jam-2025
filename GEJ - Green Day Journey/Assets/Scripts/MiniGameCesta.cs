using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MiniGameCesta : MonoBehaviour
{
    public static MiniGameCesta Instance;

    public Image telaEscura;
    public string proximaCena;
    public float duracaoFade = 1f;
    private int contador = 0;
    private int alvo = 8;
    private bool transicaoIniciada = false;

    public RectTransform[] prateleiras; // todas as prateleiras no inspector

    void Awake()
    {
        Instance = this;
        if (telaEscura != null)
            telaEscura.color = new Color(0, 0, 0, 0);
    }

    public void CenouraColocada()
    {
        if (transicaoIniciada) return;

        contador++;
        if (contador >= alvo)
            StartCoroutine(FadeESwitchScene());
    }

    System.Collections.IEnumerator FadeESwitchScene()
    {
        transicaoIniciada = true;

        float tempo = 0f;
        Color corInicial = telaEscura.color;
        Color corFinal = new Color(0, 0, 0, 1);

        while (tempo < duracaoFade)
        {
            tempo += Time.deltaTime;
            telaEscura.color = Color.Lerp(corInicial, corFinal, tempo / duracaoFade);
            yield return null;
        }

        SceneManager.LoadScene(proximaCena);
    }
}
