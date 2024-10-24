using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoPredator : MonoBehaviour
{
    [SerializeField] private Transform controladorDisparo;
    [SerializeField] private GameObject bala;

    [SerializeField] private Animator animator; // Referencia al Animator
    [SerializeField] private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer del personaje

    public bool PuedeDisparar { get; set; } = false; // Variable para controlar si puede disparar

    private void Update()
    {
        if (PuedeDisparar && Input.GetKeyDown(KeyCode.V))
        {
            Disparo();
        }
    }

    private void Disparo()
    {
        // Ejecutar la animación de disparo
        if (animator != null)
        {
            animator.SetTrigger("disparo");
        }

        // Instanciar la bala
        //Instantiate(bala, controladorDisparo.position, controladorDisparo.rotation);

        // Determinar la dirección de disparo según la orientación del personaje
        Vector3 direccionDisparo = spriteRenderer.flipX ? Vector3.left : Vector3.right;

        // Instanciar la bala y ajustar su dirección
        GameObject nuevaBala = Instantiate(bala, controladorDisparo.position, Quaternion.identity);
        nuevaBala.GetComponent<Lazer>().SetDireccion(direccionDisparo);

    }
}
