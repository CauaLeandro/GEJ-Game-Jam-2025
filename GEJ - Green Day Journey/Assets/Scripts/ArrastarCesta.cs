using UnityEngine;

public class ArrastarCesta : MonoBehaviour
{
    public float velocidade = 5f;

    // Limites do movimento
    public float minX = -5f;
    public float maxX = 5f;
    public float minZ = -5f;
    public float maxZ = 5f;

    private Rigidbody rbCesta;
    private float alturaFixa;

    void Start()
    {
        rbCesta = GetComponent<Rigidbody>();
        if (rbCesta == null)
            rbCesta = gameObject.AddComponent<Rigidbody>();

        rbCesta.useGravity = false;
        rbCesta.constraints = RigidbodyConstraints.FreezeRotation;

        alturaFixa = transform.position.y; // mantém altura fixa
    }

    void FixedUpdate()
    {
        float eixoX = Input.GetAxis("Horizontal"); // setas ou A/D
        float eixoZ = Input.GetAxis("Vertical");   // setas ou W/S

        Vector3 movimento = new Vector3(eixoX, 0, eixoZ) * velocidade * Time.fixedDeltaTime;
        Vector3 novaPosicao = rbCesta.position + movimento;

        // Limita a posição dentro dos limites definidos
        novaPosicao.x = Mathf.Clamp(novaPosicao.x, minX, maxX);
        novaPosicao.z = Mathf.Clamp(novaPosicao.z, minZ, maxZ);
        novaPosicao.y = alturaFixa;

        rbCesta.MovePosition(novaPosicao);
    }
}
