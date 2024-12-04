using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float temp;
    [SerializeField] GameObject hitExplosion;

    void Update()
    {
        // actualizar mi temporizador
        temp -= Time.deltaTime;
        if (temp < 0)
            Destroy(gameObject);
        
        // actualiza la posición
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // instanciamos la explosión
        Instantiate(hitExplosion, transform.position, Quaternion.identity);

        // destruimos el disparo
        Destroy(gameObject);
    }
}
