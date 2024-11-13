using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VidaPredator : MonoBehaviour
{
    public int maxHealth; // Vida m�xima del personaje
    [SerializeField] public int currentHealth; // Vida actual del personaje
    public Animator animator; // Referencia al Animator del personaje

    public AudioSource audioSource; // Componente AudioSource para reproducir sonido
    public AudioClip deathSound; // Clip de sonido de muerte

    public GameObject gameOverPanel; // Referencia al panel de Game Over

    [SerializeField] private BarraVida barraVida;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth; // Inicializar la vida al m�ximo al comenzar

        //barraVida.InicializarBarravida(maxHealth); //inicializa la vida maxima del personaje en la barra de vida

        if (barraVida == null)
        {
            Debug.LogError("Barra de Vida no est� asignada en el Inspector.");
        }
        else
        {
            barraVida.InicializarBarravida(maxHealth);
        }

        // Aseg�rate de que el AudioSource est� asignado
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

    // M�todo para recibir da�o
    public void TakeDamage(int damage)
    {
        currentHealth -= damage; // Reducir la vida
        Debug.Log("Personaje ha recibido da�o. Vida restante: " + currentHealth);

        barraVida.CambiarVidaActual(currentHealth);

        // Revisar si la vida ha llegado a cero
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // M�todo para activar la animaci�n de muerte
    private void Die()
    {
        animator.SetBool("muerte", true); // Activar animaci�n de muerte
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

        // Aqu� puedes a�adir l�gica adicional, como deshabilitar controles, etc.
    }

    /*private IEnumerator DestruirPersonaje()
    {
        // Espera la duraci�n de la animaci�n de muerte
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        // Destruir el objeto del personaje
        Destroy(gameObject);
    }*/

    // M�todo para reiniciar la escena actual
    public void ReiniciarJuego()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reiniciar la escena actual
    }
}
