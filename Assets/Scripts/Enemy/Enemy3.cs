using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy
{
    // Si el recorrido es ciclico o no
    [SerializeField]
    private bool ciclic;
    [SerializeField]
    private List<Transform> nodeList;
    // Primero en cargarse en la lista primero en salir en la Queue
    private Queue<Vector3> path;
    private Vector3 actualTarget;
    private float speed;
    [SerializeField]
    private int maxSpeed;
    [SerializeField]
    private int acceleration;
    protected override void Initialize()
    {
        base.Initialize();
        CreatePath(nodeList);
        // Setea la primera posicion target
        actualTarget = path.Dequeue();
        // Setea su direccion y rota el sprite
        SetDirection();
        Rotate();
    }
    // Convierte la lista en una Queue
    private void CreatePath(List<Transform> pNodes)
    {
        path = new Queue<Vector3>();
        foreach (Transform n in pNodes)
        {
            path.Enqueue(n.position);
        }
    }

    protected override void Patroll()
    {
        // Aceleracion
        if (speed < maxSpeed)
        {
            speed += acceleration * Time.deltaTime;
        }
        // Se mueve
        transform.position = Vector3.MoveTowards(transform.position, actualTarget, Time.deltaTime * speed);
        // Si esta lo suficientemente cerca del target
        if (Vector3.Distance(transform.position, actualTarget) < 0.1f)
        {
            // Se reduce la speed para dejar una idea mas clara del patron
            speed /= 4;
            if (path.Count == 0)
            {
                // Si es ciclico el recorrido es el mismo solo que el enemigo
                // tendra que volver al inicio
                if (ciclic)
                {
                    CreatePath(nodeList);
                }
                // Si es patrulla se pasa la lista invertida para que 
                // vaya desde el final hasta al principio
                else
                {
                    List<Transform> tempList = nodeList;
                    tempList.Reverse();
                    CreatePath(tempList);
                }
                // Tambien se reduce la speed a 0 si no es ciclico
                if(!ciclic)
                    speed = 0;
            }
            // Al final se setea siempre el nuevo objetivo
            actualTarget = path.Dequeue();
            SetDirection();
            Rotate();
        }
    }

    // Se setea la direccion segun la diferencia entra la pos actual 
    // y la del target
    private void SetDirection()
    {
        if ((transform.position - actualTarget).x < 0)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }
}
