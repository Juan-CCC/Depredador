using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMovement : MonoBehaviour
{
    public float moveSpeed;  // Velocidad del enemigo
    public float changeDirectionInterval;  // Tiempo para cambiar de dirección
    public Animator animator;  // Referencia al componente Animator

    private Vector2 moveDirection;  // Dirección de movimiento
    private float timeToChangeDirection;  // Tiempo restante para cambiar dirección
    private Rigidbody2D rb;  // Referencia al Rigidbody2D del alien

    // Método Start se ejecuta al iniciar el juego
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Asignar Rigidbody2D
        timeToChangeDirection = changeDirectionInterval;  // Iniciar tiempo
        ChooseNewDirection();  // Escoger una dirección inicial
    }

    // Método Update se ejecuta una vez por cada frame
    void Update()
    {
        // Reducir el temporizador
        timeToChangeDirection -= Time.deltaTime;

        // Si es tiempo de cambiar de dirección
        if (timeToChangeDirection <= 0)
        {
            ChooseNewDirection();
            timeToChangeDirection = changeDirectionInterval;
        }

        // Actualizar la animación de caminar
        bool isMoving = moveDirection != Vector2.zero;
        animator.SetBool("isWalking", isMoving);
    }

    private void FixedUpdate()
    {
        // Aplicar movimiento al Rigidbody2D
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    // Escoge una nueva dirección aleatoria
    private void ChooseNewDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        moveDirection = new Vector2(randomX, randomY).normalized;
    }

}
