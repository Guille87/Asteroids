using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    const float DELAY = 0.25f;

    [SerializeField] AudioClip clip;

    void Start()
    {
        AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, 0.2f);
        Destroy(gameObject, DELAY);
    }
}
