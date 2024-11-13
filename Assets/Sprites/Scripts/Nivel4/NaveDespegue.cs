using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaveDespegue : MonoBehaviour
{
    public Animator naveAnimator; // Asigna el Animator de la nave
    public float velocidadDespegue; // Velocidad con la que la nave se mueve hacia arriba
    public float alturaMaxima = 10f; // Altura máxima del despegue

    private bool despegando = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Comprueba si el objeto que colisiona es el personaje
        if (collision.CompareTag("Player"))
        {
            // Activa la animación de despegue
            if (naveAnimator != null)
            {
                naveAnimator.SetTrigger("Despegar");
            }

            // Inicia el movimiento de la nave
            despegando = true;
        }
    }

    void Update()
    {
        // Si está despegando, mueve la nave hacia arriba
        if (despegando)
        {
            transform.Translate(Vector3.up * velocidadDespegue * Time.deltaTime);

            // Detiene el movimiento al alcanzar la altura máxima
            if (transform.position.y >= alturaMaxima)
            {
                despegando = false;
            }
        }
    }
}
