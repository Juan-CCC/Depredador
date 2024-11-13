using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VidaPredator : MonoBehaviour
{
    public int maxHealth; // Vida máxima del personaje
    [SerializeField] public int currentHealth; // Vida actual del personaje
    public Animator animator; // Referencia al Animator del personaje

    public AudioSource audioSource; // Componente AudioSource para reproducir sonido
    public AudioClip deathSound; // Clip de sonido de muerte

    public GameObject gameOverPanel; // Referencia al panel de Game Over

    [SerializeField] private BarraVida barraVida;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Inicializar la vida al máximo al comenzar

        //barraVida.InicializarBarravida(maxHealth); //inicializa la vida maxima del personaje en la barra de vida

        if (barraVida == null)
        {
            Debug.LogError("Barra de Vida no está asignada en el Inspector.");
        }
        else
        {
            barraVida.InicializarBarravida(maxHealth);
        }

        // Asegúrate de que el AudioSource esté asignado
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Desactivar el panel de Game Over al inicio
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

    }

    // Método para recibir daño
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reducir la vida
        Debug.Log("Personaje ha recibido daño. Vida restante: " + currentHealth);

        barraVida.CambiarVidaActual(currentHealth);

        // Revisar si la vida ha llegado a cero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método para activar la animación de muerte
    private void Die()
    {
        animator.SetBool("muerte", true); // Activar animación de muerte
        Debug.Log("El personaje ha muerto.");

        // Reproducir sonido de muerte
        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound); // Reproducir el sonido de muerte
        }

        // Mostrar el panel de Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Iniciar la corrutina para esperar antes de destruir el objeto
        //StartCoroutine(DestruirPersonaje());

        // Aquí puedes añadir lógica adicional, como deshabilitar controles, etc.
    }

    /*private IEnumerator DestruirPersonaje()
    {
        // Espera la duración de la animación de muerte
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Destruir el objeto del personaje
        Destroy(gameObject);
    }*/

    // Método para reiniciar la escena actual
    public void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reiniciar la escena actual
    }
}
