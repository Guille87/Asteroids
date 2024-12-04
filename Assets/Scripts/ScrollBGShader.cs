using UnityEngine;

public class ScrollBGShader : MonoBehaviour
{
    [SerializeField] float speed;
    Renderer render;
    void Start()
    {
        render = GetComponent<Renderer>();
    }

    void Update()
    {
        // desplazamiento
        Vector2 offset = Vector2.up * speed * Time.time;
        render.material.mainTextureOffset = offset;
    }
}
