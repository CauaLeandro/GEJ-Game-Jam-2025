using UnityEngine;
using UnityEngine.EventSystems;

public class DragCenoura : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool segurando = false;
    private bool preso = false;
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 posicaoInicial;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        posicaoInicial = rectTransform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (preso) return;
        segurando = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (preso) return;
        segurando = false;
        GameObject[] prateleiras = GameObject.FindGameObjectsWithTag("Colisor");

        foreach (GameObject go in prateleiras)
        {
            RectTransform prateleira = go.GetComponent<RectTransform>();
            Prateleira p = go.GetComponent<Prateleira>();

            if (RectTransformUtility.RectangleContainsScreenPoint(prateleira, Input.mousePosition, canvas.worldCamera))
            {
                if (p != null && !p.ocupada)
                {
                    rectTransform.position = prateleira.position;
                    preso = true;
                    p.ocupada = true;
                    MiniGameCesta.Instance.CenouraColocada();

                    TimerCenouras timer = FindObjectOfType<TimerCenouras>();
                    if (timer != null)
                        timer.AdicionarTempoPorCenoura();

                    return;
                }
            }
        }

        rectTransform.position = posicaoInicial;
    }

    void Update()
    {
        if (segurando && !preso)
        {
            Vector3 mousePos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out mousePos);
            rectTransform.position = mousePos;
        }
    }
}