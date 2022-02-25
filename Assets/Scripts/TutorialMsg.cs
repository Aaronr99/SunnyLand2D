using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMsg : MonoBehaviour
{
    [SerializeField]
    private GameObject textBox;

    // Si el player entra en el rango del mensaje
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            textBox.SetActive(true);
        }
    }
    // Si el player sale
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            textBox.SetActive(false);
        }
    }
}
