using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class CarneData
{
    public Button carneButton;
    public Slider slider;
    public GameObject painelQueimou;

    [Header("Sprites")]
    public Image carneImage;
    public Sprite spriteNormal;
    public Sprite spriteVirada;
    public Sprite spriteQueimada;
    public Sprite spritePronta;

    [Header("Áudio")]
    public AudioClip somProntaParaVirar;
    public AudioClip somQueimando;
    public AudioClip somProntaCozida;

    [Header("Tempo de Queimar (segundos)")]
    public float tempoQueimarMin = 2f;
    public float tempoQueimarMax = 5f;

    [HideInInspector] public AudioSource audioSource;

    [HideInInspector] public float tempoAtual = 0f;
    [HideInInspector] public bool noMaximo = false;
    [HideInInspector] public float tempoMaximo = 0f;
    [HideInInspector] public int ladosVirados = 0;
    [HideInInspector] public bool terminou = false;

    [HideInInspector] public float tempoParaCozinhar = 0f;
    [HideInInspector] public float tempoQueimar = 0f;

    [HideInInspector] public Image fillImage;
    [HideInInspector] public bool somQueimandoTocado = false;
    [HideInInspector] public bool somProntaTocado = false;
}

public class CarneController : MonoBehaviour
{
    public CarneData[] carnes;
    public Image fadeImage;
    public string proximaCena = "Game";
    public float fadeDuration = 2f;
    public float proporcaoAviso = 0.3f;

    [Header("Intervalo de tempo de cozimento por lado")]
    public float tempoMinimoCozinhar = 3f;
    public float tempoMaximoCozinhar = 7f;

    private bool jogoAcabou = false;

    void Start()
    {
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0;
            fadeImage.color = c;
        }

        foreach (var carne in carnes)
        {
            carne.slider.minValue = 0;
            carne.slider.maxValue = 1;
            carne.slider.value = 0;

            carne.tempoParaCozinhar = Random.Range(tempoMinimoCozinhar, tempoMaximoCozinhar);
            carne.tempoQueimar = Random.Range(carne.tempoQueimarMin, carne.tempoQueimarMax);

            if (carne.slider.fillRect != null)
                carne.fillImage = carne.slider.fillRect.GetComponent<Image>();

            if (carne.carneImage != null)
                carne.carneImage.sprite = carne.spriteNormal;

            carne.audioSource = carne.carneImage.gameObject.AddComponent<AudioSource>();

            carne.carneButton.onClick.AddListener(() => VirarCarne(carne));
        }
    }

    void Update()
    {
        if (jogoAcabou) return;

        foreach (var carne in carnes)
        {
            if (carne.terminou) continue;

            // Atualiza slider
            if (!carne.noMaximo)
            {
                carne.tempoAtual += Time.deltaTime;
                carne.slider.value = carne.tempoAtual / carne.tempoParaCozinhar;

                if (carne.slider.value >= 1f && !carne.somProntaTocado)
                {
                    carne.noMaximo = true;
                    carne.tempoMaximo = 0f;

                    if (carne.audioSource != null && carne.somProntaParaVirar != null)
                        carne.audioSource.PlayOneShot(carne.somProntaParaVirar);

                    carne.somProntaTocado = true;
                }
            }
            else
            {
                carne.tempoMaximo += Time.deltaTime;

                float restante = carne.tempoQueimar - carne.tempoMaximo;
                if (restante <= carne.tempoQueimar * proporcaoAviso && carne.fillImage != null)
                {
                    float t = Mathf.PingPong(Time.time * 6f, 1f);
                    carne.fillImage.color = Color.Lerp(Color.white, Color.red, t);

                    if (!carne.somQueimandoTocado && carne.audioSource != null && carne.somQueimando != null)
                    {
                        carne.audioSource.PlayOneShot(carne.somQueimando);
                        carne.somQueimandoTocado = true;
                    }
                }

                if (carne.tempoMaximo >= carne.tempoQueimar)
                {
                    carne.painelQueimou.SetActive(true);
                    if (carne.carneImage != null)
                        carne.carneImage.sprite = carne.spriteQueimada;

                    jogoAcabou = true;
                }
            }
        }

        if (!jogoAcabou && TodasCarnesFinalizadas())
        {
            jogoAcabou = true;
            StartCoroutine(FazerFadeETrocarCena());
        }
    }

    void VirarCarne(CarneData carne)
    {
        if (jogoAcabou || carne.terminou) return;

        if (carne.noMaximo)
        {
            carne.ladosVirados++;
            carne.tempoAtual = 0f;
            carne.slider.value = 0f;
            carne.noMaximo = false;
            carne.tempoParaCozinhar = Random.Range(tempoMinimoCozinhar, tempoMaximoCozinhar);
            carne.somProntaTocado = false;
            carne.somQueimandoTocado = false;

            if (carne.fillImage != null)
                carne.fillImage.color = Color.white;

            if (carne.ladosVirados >= 2)
            {
                carne.terminou = true;
                if (carne.audioSource != null && carne.somProntaCozida != null)
                    carne.audioSource.PlayOneShot(carne.somProntaCozida);

                if (carne.carneImage != null)
                    carne.carneImage.sprite = carne.spritePronta;
            }
            else
            {
                if (carne.carneImage != null)
                    carne.carneImage.sprite = carne.spriteVirada;
            }

            // Ajusta novo tempo de queimar
            carne.tempoQueimar = Random.Range(carne.tempoQueimarMin, carne.tempoQueimarMax);
        }
        else
        {
            carne.tempoAtual = 0f;
            carne.slider.value = 0f;
        }
    }

    bool TodasCarnesFinalizadas()
    {
        foreach (var carne in carnes)
        {
            if (!carne.terminou) return false;
        }
        return true;
    }

    private IEnumerator FazerFadeETrocarCena()
    {
        if (fadeImage != null)
        {
            float tempo = 0f;
            Color c = fadeImage.color;

            while (tempo < fadeDuration)
            {
                tempo += Time.deltaTime;
                c.a = Mathf.Lerp(0, 1, tempo / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
        }

        SceneManager.LoadScene(proximaCena);
    }
}
