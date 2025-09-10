using UnityEngine;

public class ArrastarCesta : MonoBehaviour
{
    public float velocidade = 5f;
    private Rigidbody rbCesta;
    private float alturaFixa;

    private float minX;
    private float maxX;

    void Start()
    {
        rbCesta = GetComponent<Rigidbody>();
        if (rbCesta == null)
            rbCesta = gameObject.AddComponent<Rigidbody>();

        rbCesta.useGravity = false;
        rbCesta.constraints = RigidbodyConstraints.FreezeRotation;

        alturaFixa = transform.position.y;

        CalcularLimites();
    }

    void FixedUpdate()
    {
        float eixoX = Input.GetAxis("Horizontal");

        Vector3 movimento = new Vector3(eixoX, 0, 0) * velocidade * Time.fixedDeltaTime;
        Vector3 novaPosicao = rbCesta.position + movimento;

        novaPosicao.y = alturaFixa;
        novaPosicao.z = rbCesta.position.z;

        novaPosicao.x = Mathf.Clamp(novaPosicao.x, minX, maxX);

        rbCesta.MovePosition(novaPosicao);
    }

    void CalcularLimites()
    {
        Camera cam = Camera.main;
        float distancia = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 limiteEsquerdo = cam.ViewportToWorldPoint(new Vector3(0, 0, distancia));
        Vector3 limiteDireito = cam.ViewportToWorldPoint(new Vector3(1, 0, distancia));
        minX = limiteEsquerdo.x + 0.5f;
        maxX = limiteDireito.x - 0.5f;
    }
}
