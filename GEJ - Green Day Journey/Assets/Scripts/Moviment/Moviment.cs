using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Vector3 movement;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>(); 
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        movement = new Vector3(moveX, 0f, moveZ).normalized;

        
        animator.SetFloat("Horizontal", moveX);
        animator.SetFloat("Vertical", moveZ);
        animator.SetFloat("Speed", movement.magnitude);
    }

    void FixedUpdate()
    {
        Vector3 moveOffset = movement * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveOffset);
    }
}
