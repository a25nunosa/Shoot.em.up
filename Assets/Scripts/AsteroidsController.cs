using UnityEngine;

public class AsteroidsController : MonoBehaviour
{
    [SerializeField]
    float minSpeedY; // caída vertical

    [SerializeField]
    float maxSpeedY;

    [SerializeField]
    float minSpeedX; // movemento lateral

    [SerializeField]
    float maxSpeedX;

    [SerializeField]
    GameObject explosionPrefab;

    Rigidbody2D rb;

    const float DESTROY_Y = -7f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Velocidade aleatoria en X e Y
        float speedY = Random.Range(minSpeedY, maxSpeedY);
        float speedX = Random.Range(minSpeedX, maxSpeedX);

        rb.linearVelocity = new Vector2(speedX, -speedY);
    }

    void Update()
    {
        if (transform.position.y < DESTROY_Y)
        {
            Destroy(gameObject);
        }
    }

    // Removido la alternativa de un disparo por ser trigger no collision
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Explode();
        }
    }

    // Coherencia con el disparo es tipo trigger no collision normal
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Shoot"))
        {
            Explode();
        }
    }

    void Explode()
    {
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
