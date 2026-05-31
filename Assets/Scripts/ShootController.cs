using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    // Velocidad de los disparos
    [SerializeField] float speed;

    // Tiempo que duran los disparos antes de autodestruirse
    [SerializeField] float lifetime;

    [SerializeField] GameObject hit1;

    void Awake()
    {
        if (gameManager == null)
        {
            gameManager = GameManager.Instance;
            if (gameManager == null)
                gameManager = FindObjectOfType<GameManager>();
        }
    }

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

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("enemy")){
            if (hit1 != null)
            {
                Instantiate(hit1, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("ShootController: hit1 prefab is not assigned.", this);
            }

            if (gameManager != null)
            {
                gameManager.AddPointP1();
            }
            else
            {
                Debug.LogError("ShootController: GameManager reference is not assigned.", this);
            }

            Destroy(gameObject);
        }
    }

}