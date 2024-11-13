using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampa : MonoBehaviour
{
    public int damageAmount = 5; // Cantidad de da�o que los picos le quitar�n al personaje

    // Detecta cuando el personaje entra en el trigger de los picos
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Aseg�rate de que el personaje tiene la etiqueta "Player"
        {
            // Intenta obtener el script de vida del personaje
            VidaPredator vida = other.GetComponent<VidaPredator>();

            if (vida != null)
            {
                vida.TakeDamage(damageAmount); // Llama al m�todo para reducir la vida del personaje
            }
        }
    }
}
