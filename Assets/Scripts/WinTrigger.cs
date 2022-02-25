using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject winPlayer;
    [SerializeField]
    private CinemachineVirtualCamera VMcamera;
    [SerializeField]
    private Transform target;
    private bool alreadyWin;

    // Si colisiona con el player empieza a ejecutar la animacion de ganar
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals("Player") && !alreadyWin)
        {
            if (GameplayManager.Instance.IsLeverUP)
            {
                alreadyWin = true;
                // Instancia el modelo de la animacion
                GameObject winGO = Instantiate(winPlayer, collision.transform.position, Quaternion.identity);
                // Cambia el follow de la camara
                VMcamera.Follow = winGO.transform;
                winGO.GetComponent<WinAnim>().Target = target;
                // Destruye el Player viejo
                Destroy(collision.gameObject);
            }
        }
    }


}
