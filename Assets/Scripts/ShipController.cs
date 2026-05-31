using UnityEngine;
using System.Collections;
public class ShipController : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private Vector3 endPosition; // Posición final de la nave al inicio
    [SerializeField] private float duration; // Duración de la transición al inicio
    private bool active = false; // Variable para determinar si se puede realizar alguna acción
    [SerializeField] private float force = 5f; // Fuerza del movimiento
    [SerializeField] int blinkNum;

    // Referencia al prefab del disparo
    [SerializeField] GameObject shootPrefab;

    // Distancia desde el centro de la nave hasta la posición donde se creará el disparo
    [SerializeField] float shootOffset = 0.5f;

    private Rigidbody2D rb; // Referencia al componente Rigidbody
    [SerializeField] GameObject explosion;
    Vector3 initialPosition; // Posición inicial de la nave

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
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine("StartPlayer");
        initialPosition = transform.position;
    }

        private void FixedUpdate()
    {
        if (active)
            CheckMove();
    }
    // Variable para determinar si la nave está activa y puede disparar


    void Update()
    {
        // Comprobar si la nave está activa y se ha pulsado la tecla de disparo (barra espaciadora)
        if (active && Input.GetKeyDown(KeyCode.Space))
        {
            // Calcular la posición donde se creará el disparo (un poco por delante de la nave)
            Vector3 shootPosition = transform.position + Vector3.up * shootOffset;

            // Crear el disparo en la posición calculada y sin rotación
            Instantiate(shootPrefab, shootPosition, Quaternion.identity);
        }
    }

    private void CheckMove()
    {
        // Obtenemos la dirección del movimiento en los ejes horizontal y vertical
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        direction.Normalize(); // Normalizamos el vector para que tenga magnitud 1

        // Aplicamos una fuerza en la dirección obtenida
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

        IEnumerator StartPlayer()
    {
        Material mat = GetComponent<SpriteRenderer>().material;
        Color color = mat.color;
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        Vector3 initialPosition = transform.position;
        float t = 0, t2 = 0;
        
        while (t < duration)
        {
            t += Time.deltaTime;
            Vector3 newPosition = Vector3.Lerp(initialPosition, endPosition, t / duration);
            transform.position = newPosition;

            t2 += Time.deltaTime;
            float newAlpha = blinkNum * (t2 / duration);
            if (newAlpha > 1)
            {
                t2 = 0;
            }
            color.a = newAlpha;
            mat.color = color;

            yield return null;
        }

        color.a = 1;
        mat.color = color;
        collider.enabled = true;
        active = true;
    }

    private void OnCollisionEnter2D(Collision2D other) {
    string tag = other.gameObject.tag;
    if (tag == "enemy" || tag == "asteroid") {
        DestroyShip();
        if (gameManager != null)
        {
            gameManager.Updatelives();
        }
        else
        {
            Debug.LogError("ShipController: GameManager reference is not assigned.", this);
        }
    }
    }

    void DestroyShip(){
    // Desactivar comportamiento
    active = false;
    // Instanciar la animación de la explosión
    Instantiate(explosion, transform.position, Quaternion.identity);
    // Resetear posición de la nave
    transform.position = initialPosition;
    // Reiniciar la nave
    StartCoroutine("StartPlayer");
}

}