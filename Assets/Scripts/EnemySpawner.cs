using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    const float MIN_X = -3.5f;
    const float MAX_X = 3.5f;
    [SerializeField] float interval;
    [SerializeField] float delay;
    [SerializeField] GameObject enemy;

    void Start()
    {
        StartCoroutine(nameof(EnemySpawn));
    }

    IEnumerator EnemySpawn()
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            // generamos una nueva nave enemiga
            Vector3 position = new Vector3(Random.Range(MIN_X, MAX_X), transform.position.y, 0);
            Instantiate(enemy, position, Quaternion.identity);

            // hacemos una pausa
            yield return new WaitForSeconds(interval);
        }
    }
}
