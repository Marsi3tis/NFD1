using UnityEngine;

public class StarWarsScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 20f;

    void Update()
    {
        // Moves the text straight "up" along the tilted canvas
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);
    }
}