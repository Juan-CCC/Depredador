using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienLava : MonoBehaviour
{
    public float jumpForce;          // Fuerza del salto
    public float detectionRange;      // Rango de detección para el personaje
    public Transform lavaSurface;          // Nivel de la superficie de la lava
    private Rigidbody2D rb;
    private bool isPlayerNearby = false;   // Bandera para verificar si el personaje está cerca

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Opcional: Puedes iniciar un salto inicial si quieres que el enemigo haga algo al comenzar
        StartCoroutine(CheckAndJump());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;  // Detecta cuando el personaje entra en el rango
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;  // Detecta cuando el personaje sale del rango
        }
    }

    private IEnumerator CheckAndJump()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); // Obtener el SpriteRenderer del enemigo

        while (true)
        {
            yield return null; // Espera un frame

            // Verifica si el personaje está cerca antes de saltar
            if (isPlayerNearby)
            {
                // Activa el sprite para que sea visible
                Debug.Log("activando sprite");
                spriteRenderer.enabled = true;

                // Aplica una fuerza hacia arriba para que el enemigo salte
                //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                // Aplica una fuerza hacia arriba para que el enemigo salte
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

                // Espera hasta que el enemigo alcance el punto máximo de salto y comience a caer
                yield return new WaitUntil(() => rb.velocity.y <= 0);
                Debug.Log("El enemigo ha alcanzado el punto máximo y comienza a caer");

                // Espera hasta que el enemigo vuelva a la superficie de la lava
                /*yield return new WaitUntil(() => transform.position.y <= lavaSurface.position.y);
                Debug.Log("El enemigo ha alcanzado la superficie de la lava");*/

                // Espera hasta que el enemigo vuelva a la superficie de la lava
                /*while (transform.position.y > lavaSurface.position.y)
                {
                    Debug.Log("Posición del enemigo: " + transform.position.y + " | Posición de la superficie de la lava: " + lavaSurface.position.y);
                    yield return null; // Espera un frame mientras el enemigo cae
                }

                Debug.Log("El enemigo ha alcanzado la superficie de la lava. Desactivando sprite.");*/

                // Espera hasta que el enemigo alcance una altura específica (en este caso, y <= 0)
                yield return new WaitUntil(() => transform.position.y <= -3);
                Debug.Log("El enemigo ha alcanzado la altura de desaparición. Desactivando sprite.");


                // Resetea la velocidad vertical para simular la caída
                //rb.velocity = new Vector2(rb.velocity.x, -jumpForce);

                // Desactiva el sprite para que sea invisible
                Debug.Log("Desactivando sprite");
                spriteRenderer.enabled = false;

                // Espera un breve momento antes de verificar nuevamente
                yield return new WaitForSeconds(1.0f);  // Ajusta el tiempo para el siguiente salto
            }
        }
    }
}
