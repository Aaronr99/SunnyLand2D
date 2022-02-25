using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    [SerializeField]
    private float timer;
    [SerializeField]
    private float jumpForce;

    protected override void Initialize()
    {
        base.Initialize();
        // empieza moviendose para la derecha
        direction = 1;
        // Rango de tiempo donde puede realizar el salto
        timer = Random.Range(2.5f, 4.5f);
    }
    protected override void Patroll()
    {
        // Si no hay suelo delante
        RaycastHit2D ray = Physics2D.Raycast(transform.position + Vector3.right * direction * 4.5f, Vector2.down, 1f, Utility.groundLayer);
        if (!ray)
        {
            direction *= -1;
        }
        timer -= Time.deltaTime;
        // Si se acabo el tiempo de espera
        if (timer <= 0)
        {
            // Rota segun la direccion del salto
            Rotate();
            anim.SetTrigger("jump");
            // Salta
            rBody.velocity = new Vector2(direction * 1.4f, 2.5f) * jumpForce;
            // Setear un timer nuevo
            timer = Random.Range(2.5f, 4.5f);
        }
    }


}
