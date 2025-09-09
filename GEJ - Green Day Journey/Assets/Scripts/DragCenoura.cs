using UnityEngine;

public class DragCenoura : MonoBehaviour
{
    private bool segurando = false;
    private Vector3 offset;
    private Transform colisorDestino;
    private bool preso = false;

    void OnMouseDown()
    {
        if (preso) return;
        segurando = true;
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseUp()
    {
        if (!segurando) return;
        segurando = false;

        if (colisorDestino != null)
        {
            transform.position = colisorDestino.position;
            preso = true;
            MiniGameCesta.Instance.CenouraColocada();
        }
    }

    void Update()
    {
        if (segurando)
            transform.position = GetMouseWorldPos() + offset;
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f; 
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Colisor"))
            colisorDestino = other.transform;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Colisor"))
            colisorDestino = null;
    }
}