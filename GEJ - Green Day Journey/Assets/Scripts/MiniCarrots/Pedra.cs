using UnityEngine;

public class Pedra : MonoBehaviour
{
    public float velocidadeInicial = 5f;
    public float velocidadeMaxima = 30f;
    public float aceleracao = 5f;
    public float tempoVida = 10f;

    private Rigidbody rb;
    private float velocidadeAtual;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.useGravity = false;
        rb.linearDamping = 0f;
        rb.angularDamping = 0.05f;
        rb.mass = 1f;

        velocidadeAtual = velocidadeInicial;
        Destroy(gameObject, tempoVida);
    }

    void FixedUpdate()
    {
        if (velocidadeAtual < velocidadeMaxima)
        {
            velocidadeAtual += aceleracao * Time.fixedDeltaTime;
            if (velocidadeAtual > velocidadeMaxima)
                velocidadeAtual = velocidadeMaxima;
        }

        rb.MovePosition(rb.position + Vector3.down * velocidadeAtual * Time.fixedDeltaTime);
    }
}
