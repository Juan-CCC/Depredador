using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioNivel : MonoBehaviour
{
    // Método que se activa al entrar en el trigger de la cueva
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tiene la etiqueta "Player"
        {
            // Cambia a la siguiente escena
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
