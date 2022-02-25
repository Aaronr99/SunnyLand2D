using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinShow : MonoBehaviour
{
    [SerializeField]
    private GameObject winMenu;
    private bool alreadyWin;
    [SerializeField]
    private GameObject leverNedeedMsg;
    // Si colisiona con el player emerge el menu de ganar
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si se cumplen todas las condiciones gana
        if (collision.gameObject.tag.Equals("Player") && !alreadyWin && GameplayManager.Instance.IsLeverUP)
        {
            GameplayManager.Instance.CantPause = true;
            alreadyWin = true;
            winMenu.SetActive(true);
        }
        // Si la palanca no esta activa muestra un mensaje
        else if (collision.gameObject.tag.Equals("Player") && !GameplayManager.Instance.IsLeverUP)
        {
            leverNedeedMsg.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player") && !GameplayManager.Instance.IsLeverUP)
        {
            leverNedeedMsg.SetActive(false);
        }
    }
}
