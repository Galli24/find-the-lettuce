using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour
{
    [HideInInspector]
    public float timeLeft;

    float slowSpeed = 6f;

    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 newScale = new Vector3(transform.localScale.x + Time.deltaTime / slowSpeed, transform.localScale.y + Time.deltaTime / slowSpeed, transform.localScale.z);
        transform.localScale = newScale;
        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0.5f)
        {
            Color newColor = spriteRenderer.material.color;
            newColor.a -= Time.deltaTime * 2;
            spriteRenderer.material.color = newColor;
            if (spriteRenderer.material.color.a <= 0)
                Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            return;
        else if (collision.gameObject.tag == "MoveableObject")
            return;
        else if (collision.gameObject.tag == "Uncollidable")
            return;
        if (collision.GetComponent<WaveEffect>() != null)
            collision.GetComponent<WaveEffect>().DoEffects();
    }
}