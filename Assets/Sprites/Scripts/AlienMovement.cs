using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMovement : MonoBehaviour
{
    public float moveSpeed;  // Velocidad del enemigo
    public float changeDirectionInterval;  // Tiempo para cambiar de direcci�n
    public Animator animator;  // Referencia al componente Animator

    private Vector2 moveDirection;  // Direcci�n de movimiento
    private float timeToChangeDirection;  // Tiempo restante para cambiar direcci�n
    private Rigidbody2D rb;  // Referencia al Rigidbody2D del alien

    // M�todo Start se ejecuta al iniciar el juego
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Asignar Rigidbody2D
        timeToChangeDirection = changeDirectionInterval;  // Iniciar tiempo
        ChooseNewDirection();  // Escoger una direcci�n inicial
    }

    // M�todo Update se ejecuta una vez por cada frame
    void Update()
    {
        // Reducir el temporizador
        timeToChangeDirection -= Time.deltaTime;

        // Si es tiempo de cambiar de direcci�n
        if (timeToChangeDirection <= 0)
        {
            ChooseNewDirection();
            timeToChangeDirection = changeDirectionInterval;
        }

        // Actualizar la animaci�n de caminar
        bool isMoving = moveDirection != Vector2.zero;
        animator.SetBool("isWalking", isMoving);
    }

    private void FixedUpdate()
    {
        // Aplicar movimiento al Rigidbody2D
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    // Escoge una nueva direcci�n aleatoria
    private void ChooseNewDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        moveDirection = new Vector2(randomX, randomY).normalized;
    }

}
