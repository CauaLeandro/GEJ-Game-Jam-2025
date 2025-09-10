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
    public AudioClip somQueimando;
    public AudioClip somCozida;

    [HideInInspector] public AudioSource audioSource;

    [HideInInspector] public float tempoAtual = 0f;
    [HideInInspector] public bool noMaximo = false;
    [HideInInspector] public float tempoMaximo = 0f;
    [HideInInspector] public int ladosVirados = 0;
    [HideInInspector] public bool terminou = false;

    [HideInInspector] public float tempoParaCozinhar = 0f;
    [HideInInspector] public float tempoQueimar = 0f;

    [HideInInspector] public Image fillImage;
    [HideInInspector] public bool somQueimandoAtivo = false;
}

public class CarneController : MonoBehaviour
{
    public CarneData[] carnes;
    public Image fadeImage;
    public string proximaCena = "Game";
    public float fadeDuration = 2f;
    public float proporcaoAviso = 0.3f;

    [Header("Tempo de cozinhar (por lado)")]
    public float tempoMinimoCozinhar = 3f;
    public float tempoMaximoCozinhar = 7f;

    [Header("Tempo de queimar após cozinhar")]
    public float tempoMinimoQueimar = 2f;
    public float tempoMaximoQueimar = 5f;

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
            carne.tempoQueimar = Random.Range(tempoMinimoQueimar, tempoMaximoQueimar);

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

            if (!carne.noMaximo)
            {
                carne.tempoAtual += Time.deltaTime;
                carne.slider.value = carne.tempoAtual / carne.tempoParaCozinhar;

                if (carne.slider.value >= 1f)
                {
                    carne.noMaximo = true;
                    carne.tempoMaximo = 0f;
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

                    if (!carne.somQueimandoAtivo)
                    {
                        carne.somQueimandoAtivo = true;
                        if (carne.somQueimando != null)
                            carne.audioSource.loop = true;
                        carne.audioSource.clip = carne.somQueimando;
                        carne.audioSource.Play();
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

        if (!carne.noMaximo)
        {
            // Penalidade: resetar o progresso da carne se clicar antes da hora
            carne.tempoAtual = 0f;
            carne.slider.value = 0f;
            carne.carneImage.sprite = carne.spriteNormal;
            return;
        }

        carne.ladosVirados++;
        carne.tempoAtual = 0f;
        carne.slider.value = 0f;
        carne.noMaximo = false;

        carne.tempoParaCozinhar = Random.Range(tempoMinimoCozinhar, tempoMaximoCozinhar);
        carne.tempoQueimar = Random.Range(tempoMinimoQueimar, tempoMaximoQueimar);

        carne.somQueimandoAtivo = false;
        carne.audioSource.Stop();

        if (carne.fillImage != null)
            carne.fillImage.color = Color.white;

        if (carne.ladosVirados >= 2)
        {
            carne.terminou = true;

            if (carne.carneImage != null)
                carne.carneImage.sprite = carne.spritePronta;

            if (carne.audioSource != null && carne.somCozida != null)
                carne.audioSource.PlayOneShot(carne.somCozida);
        }
        else
        {
            if (carne.carneImage != null)
                carne.carneImage.sprite = carne.spriteVirada;
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
