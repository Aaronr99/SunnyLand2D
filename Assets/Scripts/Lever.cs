using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    // Imagen de la palanca activada
    [SerializeField]
    private Sprite activeImage;
    // Particulas para la animacion
    [SerializeField]
    private GameObject activationFX;

    private AudioSource soundSource;
    [SerializeField]
    private AudioClip leverClip;

    private SpriteRenderer spriteRender;

    private void Start()
    {
        soundSource = gameObject.GetComponent<AudioSource>();
        spriteRender = gameObject.GetComponent<SpriteRenderer>();
        if (GameplayManager.Instance.IsLeverUP)
        {
            spriteRender.sprite = activeImage;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {   
        // Si colisiona con el player y todavia no se levanto la palanca
        if (collision.gameObject.CompareTag("Player") && !GameplayManager.Instance.IsLeverUP)
        {
            // Levanta la palaca
            spriteRender.sprite = activeImage;
            // Hace su sonido
            soundSource.clip = leverClip;
            soundSource.Play();
            // Instancia la particula
            GameObject tempGO = Instantiate(activationFX, transform.position, Quaternion.identity);
            Destroy(tempGO, 0.6f);
            GameplayManager.Instance.LeverUp(new Vector2(transform.position.x, transform.position.y));
        }
    }
}
