using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    const float DESTROY_HEIGHT = -6;

    [SerializeField] float speed;
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject shootEnemy;
    [SerializeField] float shootDelay;
    [SerializeField] float shootProb;

    void Start()
    {
        StartCoroutine(nameof(Shoot));
    }

    void Update()
    {
        // trasladamos el objeto
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < DESTROY_HEIGHT)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DestroyEnemy();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DestroyEnemy();
    }

    void DestroyEnemy()
    {
        GameManager.GetInstance().AddScore(gameObject.tag);

        // instanciamos la animación de la explosión
        Instantiate(explosion, transform.position, Quaternion.identity);
        
        // destruimos la nave
        Destroy(gameObject);
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootDelay);

            if (Random.Range(0f, 1f) < shootProb)
            {
                GameObject player = GameObject.FindWithTag("Player");

                if (player != null && (transform.position.x > player.transform.position.x - 0.5f) && (transform.position.x < player.transform.position.x + 0.5f))
                {
                    Instantiate(shootEnemy, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
