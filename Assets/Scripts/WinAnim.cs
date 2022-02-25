using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script que va junto con una copia del sprite principal para hacer
// una pequeña animacion al momento de ganar
public class WinAnim : MonoBehaviour
{
    private float speed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float acceleration;

    [SerializeField]
    private Transform target;
    public Transform Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }
    [SerializeField]
    private Animator anim;
    private void FixedUpdate()
    {
        // Aceleracion
        if (speed < maxSpeed)
        {
            speed += acceleration * Time.deltaTime;
        }
        // Mientras que no llegue al target
        if (Vector2.Distance(transform.position, target.position) > 0.25f)
        {
            anim.SetBool("moving", true);
            // Para asegurarse que esten la misma altura
            Vector2 tarPos = new Vector2(target.position.x, transform.position.y);
            // Se mueve hacia el target
            transform.position = Vector2.MoveTowards(transform.position, tarPos, speed * Time.deltaTime);
        }
    }
}
