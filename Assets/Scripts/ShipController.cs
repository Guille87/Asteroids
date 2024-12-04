using System.Collections;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    const float SHOOT_OFFSET = 0.5f;

    [SerializeField] GameObject shoot;
    [SerializeField] Vector3 endPosition;
    [SerializeField, Min(0.1f)] float duration;
    [SerializeField, Min(0)] float force;
    [SerializeField, Range(1, 10)] int blinkNum;
    [SerializeField] GameObject explosion;
    
    Rigidbody2D rb;
    bool isPlayerActive;
    GameManager game;

    Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
        rb = GetComponent<Rigidbody2D>();
        game = GameManager.GetInstance();
        StartCoroutine(nameof(StartPlayer));
    }

    void Update()
    {
        if (isPlayerActive)
            HandleShooting();
    }

    void FixedUpdate()
    {
        if (isPlayerActive)
            CheckMove();
    }

    void HandleShooting()
    {
        if (!game.isGamePaused() && Input.GetButtonDown("Fire1"))
        {
            Instantiate(shoot, 
                new Vector3(transform.position.x, transform.position.y + SHOOT_OFFSET, 0), 
                Quaternion.identity);
        }
    }

    void CheckMove()
    {
        // dirección de movimiento
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        // aplicamos una fuerza en esa dirección
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string tag = other.gameObject.tag;
        if (tag == "Enemy" || tag == "AsteroidBig" || tag == "AsteroidSmall")
        {
            DestroyShip();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            DestroyShip();
        }
    }

    void DestroyShip()
    {
        isPlayerActive = false;

        // instanciamos la animación de la explosión
        Instantiate(explosion, transform.position, Quaternion.identity);

        // actualizamos las vidas
        GameManager gm = GameManager.GetInstance();
        gm.LoseLife();

        // creamos una nueva nave
        if (!gm.isGameOver())
            Instantiate(gameObject, posicionInicial, Quaternion.identity);

        // destruimos nuestra nave
        Destroy(gameObject);
        // transform.position = posicionInicial;
        // StartCoroutine(nameof(StartPlayer));
    }

    IEnumerator StartPlayer()
    {
        // Desactivamos las colisiones
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        
        // posición inicial
        Vector3 initialPosition = transform.position;

        // referencia al material
        Material mat = GetComponent<SpriteRenderer>().material;
        Color color = mat.color;

        float blinkInterval = duration / (blinkNum * 2); // Tiempo de cada parpadeo (apagado y encendido)
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            Vector3 newPosition = Vector3.Lerp(initialPosition, endPosition, elapsedTime / duration);
            transform.position = newPosition;

            // parpadeo
            float blinkTime = elapsedTime % (blinkInterval * 2);
            color.a = blinkTime < blinkInterval ? 0 : 1;
            mat.color = color;

            yield return null;
        }

        // reseteamos el canal alpha
        color.a = 1;
        mat.color = color;

        // activamos las colisiones
        collider.enabled = true;

        // activamos la nave
        isPlayerActive = true;
    }
}
