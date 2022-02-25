using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si colisiona con el player lo mata
        if (collision.gameObject.tag.Equals("Player"))
        {
            // Hace 5 de daño
            collision.gameObject.GetComponent<PlayerController>().TakeDmg(0, 5);
        }
    }
}
