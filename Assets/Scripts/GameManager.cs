using UnityEngine;

using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

       const int LIVES = 3;
   [SerializeField] TextMeshProUGUI txtScore;
   [SerializeField] TextMeshProUGUI txtMaxScore; 
   [SerializeField] TextMeshProUGUI txtMessage; 
    //Array paara las imágenes que marcan las vidas 
    [SerializeField] GameObject[] imgLives;
    
    [Header("Extra Life Settings")]
    [Tooltip("Puntos necesarios para ganar 1 vida extra (repite cada X puntos).")]
    [SerializeField] int pointsPerExtraLife = 1000;
 
    int score;
    int maxScore; 
      //Inicializamos las vidas a la constante 
    int lives = LIVES; 
    
        // Siguiente umbral de puntuación para dar vida extra
        int nextExtraLifeAt;

    public void AddPointP1()
    {
        score++;
        if (txtScore != null)
            txtScore.text = string.Format("{0,4:D4}", score);

        CheckForExtraLife();
    }

    void CheckForExtraLife()
    {
        if (pointsPerExtraLife <= 0)
            return;

        while (score >= nextExtraLifeAt)
        {
            AddLife(1);
            nextExtraLifeAt += pointsPerExtraLife;
        }
    }

    // Añade vidas (respetando el número máximo de imágenes en imgLives si está asignado)
    public void AddLife(int amount = 1)
    {
        if (amount <= 0) return;

        int maxPossibleLives = imgLives != null && imgLives.Length > 0 ? imgLives.Length : 99;
        lives = Mathf.Min(lives + amount, maxPossibleLives);
    }

     private void OnGUI()
    {
        if (imgLives != null)
        {
            for (int i = 0; i < imgLives.Length; i++)
            {
                if (imgLives[i] != null)
                    imgLives[i].SetActive(i < lives);
            }
        }

        if (txtScore != null)
            txtScore.text = string.Format("{0,4:D4}", score);
    }

    void Update()
    {
        if (lives == 0)
        {
            txtMessage.gameObject.SetActive(true);

            // Destruye los objetos instanciados por spawner o nosotros
            DestroyAllWithTag("asteroid");
            DestroyAllWithTag("enemy");
            DestroyAllWithTag("shoot");
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
                txtMessage.gameObject.SetActive(false);

            }
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        txtMessage.gameObject.SetActive(false);
        // Inicializar valores UI y umbral de vida extra
        if (txtScore != null)
            txtScore.text = string.Format("{0,4:D4}", score);
        if (txtMaxScore != null)
            txtMaxScore.text = string.Format("{0,4:D4}", maxScore);

        nextExtraLifeAt = pointsPerExtraLife > 0 ? pointsPerExtraLife : int.MaxValue;

    }

        // Método para actualizar las vidas
    public void Updatelives()
    {
        lives--;
    }



    

     void DestroyAllWithTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

}