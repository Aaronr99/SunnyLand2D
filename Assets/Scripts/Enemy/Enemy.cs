using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase padre de todos los enemigos
public class Enemy : MonoBehaviour
{
    protected int direction;
    protected CapsuleCollider2D boxCollider;
    protected Animator anim;
    protected bool died;
    public GameObject deathFX;
    protected Rigidbody2D rBody;
    private void Start()
    {
        boxCollider = gameObject.GetComponent<CapsuleCollider2D>();
        anim = gameObject.GetComponent<Animator>();
        rBody = gameObject.GetComponent<Rigidbody2D>();
        Initialize();
    }

    protected virtual void Initialize() { }
    private void Update()
    {
        // Si no murio
        if (!died)
        {
            Patroll();
            return;
        }
    }

    protected virtual void Patroll() { }

    // Al solo tener una vida mueren de un solo golpe
    public virtual void TakeDamg()
    {
        died = true;
        // Se desactiva el collider para evitar dañar al jugador
        boxCollider.enabled = false;
        // Se instancian las particulas de puerte
        Instantiate(deathFX, transform.position, Quaternion.identity);
        // Se destruye al enemigo 
        Destroy(gameObject, 0.1f);
    }

    // Rota en la direccion que se esta mirando
    protected void Rotate()
    {
        if (direction == 1)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    // Hace daño al Player si lo toca
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            float dir = (collision.transform.position - transform.position).normalized.x;
            collision.gameObject.GetComponent<PlayerController>().TakeDmg(dir, 1);
        }
    }
}
