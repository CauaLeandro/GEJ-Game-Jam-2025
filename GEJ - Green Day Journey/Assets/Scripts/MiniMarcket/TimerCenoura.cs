using UnityEngine;
using TMPro;

public class TimerCenouras : MonoBehaviour
{
    public float tempoInicial = 30f;
    private float tempoRestante;
    public TMP_Text textoTimer;
    public GameObject canvasGameOver;
    public float bonusPorCenoura = 5f;
    private bool rodando = true;

    void Start()
    {
        tempoRestante = tempoInicial;
        if (canvasGameOver != null)
            canvasGameOver.SetActive(false);
    }

    void Update()
    {
        if (!rodando) return;

        tempoRestante -= Time.deltaTime;
        if (tempoRestante <= 0f)
        {
            tempoRestante = 0f;
            rodando = false;
            if (canvasGameOver != null)
                canvasGameOver.SetActive(true);
        }

        if (textoTimer != null)
            textoTimer.text = Mathf.CeilToInt(tempoRestante).ToString();
    }

    public void AdicionarTempoPorCenoura()
    {
        tempoRestante += bonusPorCenoura;
    }
}