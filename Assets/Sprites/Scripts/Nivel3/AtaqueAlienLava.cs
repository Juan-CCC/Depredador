using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueAlienLava : MonoBehaviour
{
    public int damageAmount = 5;       // Cantidad de daño que el enemigo inflige
    public Animator animator;           // Referencia al Animator del enemigo
    private bool hasAttacked = false;   // Bandera para evitar daño repetido en una sola colisión
    private bool isAttacking = false;   // Bandera para verificar si está en ataque

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si la colisión es con el personaje
        if (collision.gameObject.CompareTag("Player") && !hasAttacked)
        {
            // Intenta obtener el script de vida del personaje
            VidaPredator playerHealth = collision.gameObject.GetComponent<VidaPredator>();

            if (playerHealth != null)
            {
                // Inflige daño al personaje
                playerHealth.TakeDamage(damageAmount);

                // Activa la animación de ataque
                if (animator != null)
                {
                    animator.SetBool("ataque", true);
                }

                // Marca que el ataque ya se ha realizado en esta colisión
                hasAttacked = true;
                isAttacking = true;

                // Restablece el ataque después de un breve momento
                StartCoroutine(ResetAttack());
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Verifica si el personaje ya no está en contacto
        if (collision.gameObject.CompareTag("Player") && isAttacking)
        {
            // Desactiva la animación de ataque
            if (animator != null)
            {
                animator.SetBool("ataque", false); // Detiene la animación de ataque
            }

            isAttacking = false;
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(1.0f); // Ajusta el tiempo según el intervalo de ataque deseado
        hasAttacked = false;
    }
}
