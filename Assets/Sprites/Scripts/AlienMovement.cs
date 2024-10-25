using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMovement : MonoBehaviour
{
    public float walkSpeed; // Velocidad al caminar
    public float runSpeed; // Velocidad al correr
    public float detectionRange; // Rango de detección del personaje
    public float changeDirectionInterval;  // Tiempo para cambiar de dirección
    public Animator animator;  // Referencia al componente Animator
    public LayerMask groundLayer;  // Capa del suelo

    private Vector2 moveDirection;  // Dirección de movimiento
    private float timeToChangeDirection;  // Tiempo restante para cambiar dirección
    private Rigidbody2D rb;  // Referencia al Rigidbody2D del alien
    private GameObject player; // Referencia al personaje
    private bool facingRight = false; // Indica si está mirando a la derecha
    private bool isChasing = false; // Indica si está persiguiendo al personaje

    // Método Start se ejecuta al iniciar el juego
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Asignar Rigidbody2D
        player = GameObject.FindGameObjectWithTag("Player"); // Encontrar al personaje por etiqueta
        timeToChangeDirection = changeDirectionInterval;  // Iniciar tiempo
        ChooseNewDirection();  // Escoger una dirección inicial
    }

    // Método Update se ejecuta una vez por cada frame
    void Update()
    {
        // Detectar si el personaje está dentro del rango de detección
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true; // Iniciar persecución
        }
        else
        {
            isChasing = false; // Dejar de perseguir
        }

        // Cambiar dirección si no está persiguiendo al personaje
        if (!isChasing)
        {
            timeToChangeDirection -= Time.deltaTime;
            if (timeToChangeDirection <= 0)
            {
                ChooseNewDirection();
                timeToChangeDirection = changeDirectionInterval;
            }
        }

        // Actualizar animación: correr o caminar según el estado
        if (isChasing)
        {
            Debug.Log("El enemigo está persiguiendo al personaje.");
            animator.SetBool("run", true);
            animator.SetBool("walk", false);
        }
        else
        {
            animator.SetBool("run", false);
            animator.SetBool("walk", moveDirection != Vector2.zero);
        }
    }

    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            if (isChasing)
            {
                // Perseguir al personaje con mayor velocidad
                Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
                rb.MovePosition(rb.position + directionToPlayer * runSpeed * Time.fixedDeltaTime);

                // Ajustar la orientación del enemigo al personaje
                if ((directionToPlayer.x > 0 && !facingRight) || (directionToPlayer.x < 0 && facingRight))
                {
                    Flip();
                }
            }
            else
            {
                // Moverse aleatoriamente cuando no persigue
                Vector2 newPosition = rb.position + moveDirection * walkSpeed * Time.fixedDeltaTime;
                rb.MovePosition(newPosition);

                // Ajustar la orientación al moverse aleatoriamente
                if ((moveDirection.x > 0 && !facingRight) || (moveDirection.x < 0 && facingRight))
                {
                    Flip();
                }
            }
        }
    }

    // Escoge una nueva dirección aleatoria
    private void ChooseNewDirection()
    {
        // Generar un valor aleatorio para X (-1, 0, 1)
        float randomX = Random.Range(-1, 2); // Puede ser -1 (izquierda), 0 (quieto) o 1 (derecha)
        if (randomX == 0) randomX = Random.Range(-1, 2); // Evitar que se quede quieto

        moveDirection = new Vector2(randomX, 0).normalized; // Movimiento solo en X
    }

    // Cambia la orientación del enemigo invirtiendo el scale.x
    private void Flip()
    {
        facingRight = !facingRight; // Cambiar la dirección actual
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Invertir el eje X
        transform.localScale = localScale;
    }


    // Verifica si el enemigo está tocando el suelo usando Raycast
    private bool IsGrounded()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 1f, groundLayer);
        return hit.collider != null;
    }

}
