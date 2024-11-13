using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueAlienLava : MonoBehaviour
{
    public int damageAmount = 5;       // Cantidad de da�o que el enemigo inflige
    public Animator animator;           // Referencia al Animator del enemigo
    private bool hasAttacked = false;   // Bandera para evitar da�o repetido en una sola colisi�n
    private bool isAttacking = false;   // Bandera para verificar si est� en ataque

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si la colisi�n es con el personaje
        if (collision.gameObject.CompareTag("Player") && !hasAttacked)
        {
            // Intenta obtener el script de vida del personaje
            VidaPredator playerHealth = collision.gameObject.GetComponent<VidaPredator>();

            if (playerHealth != null)
            {
                // Inflige da�o al personaje
                playerHealth.TakeDamage(damageAmount);

                // Activa la animaci�n de ataque
                if (animator != null)
                {
                    animator.SetBool("ataque", true);
                }

                // Marca que el ataque ya se ha realizado en esta colisi�n
                hasAttacked = true;
                isAttacking = true;

                // Restablece el ataque despu�s de un breve momento
                StartCoroutine(ResetAttack());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Verifica si el personaje ya no est� en contacto
        if (collision.gameObject.CompareTag("Player") && isAttacking)
        {
            // Desactiva la animaci�n de ataque
            if (animator != null)
            {
                animator.SetBool("ataque", false); // Detiene la animaci�n de ataque
            }

            isAttacking = false;
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1.0f); // Ajusta el tiempo seg�n el intervalo de ataque deseado
        hasAttacked = false;
    }
}
