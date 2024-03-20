using UnityEngine;
using UnityEngine.UI;

public class SphereRoll : MonoBehaviour
{
    public float rollForce = 10f;
    public float collisionForce = 20f;
    public float rollSpeed = 5f; // Speed at which the sphere rolls towards each other
    public Transform target;
    public Text messageBox; // Reference to the UI Text component to display messages
    public float separationDistance = 5.0f; // Manually set separation distance
    public float repulsionForce = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        rb.AddTorque(transform.forward * rollForce);
        rb.velocity = direction * rollSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();

            if (rb != null && otherRb != null)
            {
                // Calculate repulsion direction
                Vector3 repulsionDirection = (transform.position - collision.transform.position).normalized;

                // Apply repulsion forces in opposite directions
                rb.AddForce(repulsionDirection * repulsionForce, ForceMode.Impulse);
                otherRb.AddForce(-repulsionDirection * repulsionForce, ForceMode.Impulse);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Display message on screen
            if (messageBox != null)
            {
                messageBox.text = "The two balls are in touch";
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Display message on screen
            if (messageBox != null)
            {
                messageBox.text = "The balls have stopped colliding";
            }
        }
    }
}
