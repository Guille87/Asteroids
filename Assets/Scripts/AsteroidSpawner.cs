using System.Collections;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] GameObject asteroid;
    [SerializeField] float minInterval;
    [SerializeField] float maxInterval;
    [SerializeField] float delay;

    Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;

        StartCoroutine(nameof(AsteroidSpawn));
    }

    IEnumerator AsteroidSpawn()
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            // generamos una nueva nave enemiga
            Instantiate(asteroid, initialPosition, Quaternion.identity);

            // hacemos una pausa
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
        }
    }
}
