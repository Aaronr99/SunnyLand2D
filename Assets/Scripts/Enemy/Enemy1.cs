using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    [SerializeField]
    private float speed;
    protected override void Initialize()
    {
        base.Initialize();
        // empieza moviendose para la derecha
        direction = 1;
    }

    protected override void Patroll()
    {
        transform.position += new Vector3(direction, 0, 0) * speed * Time.deltaTime;
        // Si no hay suelo delante
        // o si hay otro enemigo delante
        if (!Physics2D.Raycast(transform.position + Vector3.right * direction * 1.4f, Vector2.down, 1.2f, Utility.groundLayer)
            || Physics2D.Raycast(transform.position + Vector3.right * direction * 1.4f, Vector2.down, 1.2f, Utility.enemyLayer))
        {
            // Cambia la direccion y rota el sprite
            direction *= -1;
            Rotate();
        }
    }
}
