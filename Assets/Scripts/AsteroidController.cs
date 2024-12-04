using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    const float DESTROY_HEIGHT = -6;
    const int HITS_TO_DESTROY = 4;

    [SerializeField] float force;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject asteroid;

    Rigidbody2D rb;
    int hitCount;

    void Start()
    {
        if (tag == "AsteroidBig")
            LaunchBigAsteroid();
    }

    void Update()
    {
        if (transform.position.y < DESTROY_HEIGHT)
            Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            DestroyAsteroid();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (tag == "AsteroidBig")
        {
            ++hitCount;
            if (hitCount == HITS_TO_DESTROY)
            {
                LaunchSmallAsteroid();
                DestroyAsteroid();
            }
        }
        else
        {
            DestroyAsteroid();
        }
    }

    void DestroyAsteroid()
    {
        GameManager.GetInstance().AddScore(gameObject.tag);

        // instanciamos la animación de la explosión
        Instantiate(explosion, transform.position, Quaternion.identity);
        
        // destruimos la nave
        Destroy(gameObject);
    }

    void LaunchBigAsteroid()
    {
        Vector2 direction = new Vector2(1, -0.25f).normalized;

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * force, ForceMode2D.Impulse);
        rb.AddTorque(-.1f, ForceMode2D.Impulse);
    }

    void LaunchSmallAsteroid()
    {
        // posición y velocidad de padre
        Vector2 linearVelocity = rb.linearVelocity;
        float angularVelocity = rb.angularVelocity;
        Vector3 position = transform.position;


        // instanciar los asteroides pequeños
        for (int i = 0, s = 1; i < 2; i++, s *= -1)
        {
            GameObject smallAsteroid = Instantiate(asteroid, position, Quaternion.identity);
            Rigidbody2D rbSmall = smallAsteroid.GetComponent<Rigidbody2D>();
            rbSmall.linearVelocity = new Vector2(s * linearVelocity.x, linearVelocity.y);
            rbSmall.angularVelocity = angularVelocity;
        }
    }
}
