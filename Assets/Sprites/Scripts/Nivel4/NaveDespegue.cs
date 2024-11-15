using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NaveDespegue : MonoBehaviour
{
    public Animator naveAnimator; // Asigna el Animator de la nave
    public float velocidadDespegue; // Velocidad con la que la nave se mueve hacia arriba
    public float alturaMaxima = 10f; // Altura máxima del despegue
    public GameObject player; // Referencia al personaje
    public Canvas finDelJuegoCanvas; // Referencia al Canvas de fin del juego


    private bool despegando = false;

    void Start()
    {
        if (finDelJuegoCanvas != null)
        {
            finDelJuegoCanvas.gameObject.SetActive(false); // Asegúrate de que el Canvas esté desactivado al inicio
        }
    }

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

            // Oculta al personaje (o desactiva su GameObject)
            if (player != null)
            {
                player.SetActive(false); // Desactiva al personaje
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
                MostrarFinDelJuego();
                despegando = false;
            }
        }
    }

    void MostrarFinDelJuego()
    {
        if (finDelJuegoCanvas != null)
        {
            finDelJuegoCanvas.gameObject.SetActive(true); // Muestra el Canvas
        }
    }

    // Método para reiniciar el nivel o ir al primer nivel
    public void CargarPrimerNivel()
    {
        SceneManager.LoadScene(1); // Cambia el "0" al índice o nombre de la escena del primer nivel
    }
}
