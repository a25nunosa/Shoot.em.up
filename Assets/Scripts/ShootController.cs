using UnityEngine;

public class ShootController : MonoBehaviour
{
    // Velocidad de los disparos
    [SerializeField]
    float speed;

    [SerializeField]
    GameObject hit; // Efecto de impacto al colisionar

    // Tiempo que duran los disparos antes de autodestruirse
    [SerializeField]
    float lifetime;

    void Start()
    {
        // Destruir el disparo después de un cierto tiempo
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Mover el disparo hacia arriba
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    // Método para destruir el disparo cuando sale de la pantalla
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameManager.GetInstance().AddScore(50); // Añadir puntos al destruir un enemigo
            Instantiate(hit, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        // Debug.Log("Colisión con disparo");
    }
}
