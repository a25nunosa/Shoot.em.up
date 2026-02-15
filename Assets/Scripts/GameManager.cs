using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;

    int lives = 3;
    int score = 0;
    int maxScore = 0;

    [SerializeField]
    TextMeshProUGUI txtScore;

    [SerializeField]
    TextMeshProUGUI txtMaxScore;

    [SerializeField]
    TextMeshProUGUI txtMessage1;

    [SerializeField]
    TextMeshProUGUI txtMessage2;

    //Array paara las imágenes que marcan las vidas
    [SerializeField]
    GameObject[] imgLives;

    // Método estático para obtener la instancia del GameManager
    public static GameManager GetInstance()
    {
        return instance;
    }

    private void OnGUI()
    {
        for (int i = 0; i < imgLives.Length; i++)
        {
            imgLives[i].SetActive(i < lives);
        }
        txtScore.text = string.Format("{0,4:D4}", score);
    }

    // Función Awake se ejecuta cuando se instancia el objeto
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Evitar que el objeto se destruya al cambiar de escena
        }
        else if (instance != this)
        {
            // Si ya existe una instancia, destruimos el nuevo GameManager para mantener la singularidad
            Destroy(gameObject);
        }
    }

    void Start()
    {
        txtMessage1.gameObject.SetActive(false);
        txtMessage2.gameObject.SetActive(false);
    }

    void Update()
    {
        if (lives == 0)
        {
            txtMessage1.gameObject.SetActive(true);
            txtMessage2.gameObject.SetActive(true);
            // Destruye los objetos instanciados por spawner o nosotros
            DestroyAllWithTag("AsteroidBig");
            DestroyAllWithTag("Enemy");
            DestroyAllWithTag("Shoot");
            if (score > maxScore)
            {
                maxScore = score;
                txtMaxScore.text = string.Format("{0,4:D4}", maxScore);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Reiniciamos el juego
                lives = 3;
                score = 0;
                txtMessage1.gameObject.SetActive(false);
                txtMessage2.gameObject.SetActive(false);
            }
        }
    }

    public void ReduceLife()
    {
        lives--;
        Debug.Log("Vidas restantes: " + lives);
    }

    public void AddScore(int puntuacion)
    {
        score += puntuacion;
        // Controla la logica de date una vida extra. Al llegar a la mitad de la puntuación máxima
        if (score == 5000 && lives < 3)
        {
            lives++;
        }
    }

    // Destruye todos los GameObjects con la tag especificada
    void DestroyAllWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
}
