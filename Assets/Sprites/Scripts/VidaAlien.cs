using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] private float vida;

    private Animator animator;

    // Variables de sonido
    public AudioSource audioSource; // Componente AudioSource para reproducir sonido
    public AudioClip deathSound; // Clip de sonido de muerte del enemigo

    private void Start()
    {
        animator = GetComponent<Animator>();

        // Asegurarse de que el AudioSource esté asignado
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void TomarDano(float dano)
    {
        vida -= dano;  // Restar el daño a la vida actual

        if (vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        animator.SetTrigger("muerte");

        // Reproducir sonido de muerte
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        Destroy(gameObject, 1f);  // Destruir el enemigo después de 1 segundo (opcional)
    }
}
