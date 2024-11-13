using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMove2 : MonoBehaviour
{
    public float walkSpeed; // Velocidad al caminar
    public float runSpeed; // Velocidad al correr
    public float detectionRange; // Rango de detecci�n del personaje
    public float changeDirectionInterval;  // Tiempo para cambiar de direcci�n
    public Animator animator;  // Referencia al componente Animator
    public LayerMask groundLayer;  // Capa del suelo

    public float minX; // L�mite m�nimo de X
    public float maxX; // L�mite m�ximo de X

    public int damage = 10;  // Cantidad de da�o que el enemigo inflige
    public float attackCooldown = 2f;  // Tiempo de espera entre ataques

    private Vector2 moveDirection;  // Direcci�n de movimiento
    private float timeToChangeDirection;  // Tiempo restante para cambiar direcci�n
    private float nextAttackTime = 0f; // Tiempo hasta el siguiente ataque
    private Rigidbody2D rb;  // Referencia al Rigidbody2D del alien
    private GameObject player; // Referencia al personaje
    private VidaPredator playerHealth; // Referencia al script de vida del jugador
    private bool facingRight = true; // Indica si est� mirando a la derecha
    private bool isChasing = false; // Indica si est� persiguiendo al personaje

    // M�todo Start se ejecuta al iniciar el juego
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Asignar Rigidbody2D
        player = GameObject.FindGameObjectWithTag("Player"); // Encontrar al personaje por etiqueta

        if (player != null)
        {
            playerHealth = player.GetComponent<VidaPredator>();

            if (playerHealth == null)
            {
                Debug.LogError("El objeto Player no tiene el componente VidaPredator.");
            }
        }

        else
        {
            Debug.LogError("No se encontr� ning�n objeto con la etiqueta 'Player'.");
        }

        timeToChangeDirection = changeDirectionInterval;  // Iniciar tiempo
        ChooseNewDirection();  // Escoger una direcci�n inicial
    }

    // M�todo Update se ejecuta una vez por cada frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        isChasing = distanceToPlayer <= detectionRange;

        if (isChasing && Time.time >= nextAttackTime)
        {
            AttackPlayer(); // Llama a la funci�n de ataque
            nextAttackTime = Time.time + attackCooldown;
        }

        if (!isChasing)
        {
            timeToChangeDirection -= Time.deltaTime;
            if (timeToChangeDirection <= 0)
            {
                ChooseNewDirection();
                timeToChangeDirection = changeDirectionInterval;
            }
        }

        animator.SetBool("run", isChasing);
        animator.SetBool("walk", !isChasing && moveDirection != Vector2.zero);
    }

    private void FixedUpdate()
    {
        if (IsGrounded())
        {
            Vector2 currentPosition = rb.position; // Guardar la posici�n actual

            if (isChasing)
            {
                // Perseguir al jugador solo en el eje X
                Vector2 directionToPlayer = new Vector2(player.transform.position.x - transform.position.x, 0).normalized;
                Vector2 targetPosition = currentPosition + directionToPlayer * runSpeed * Time.fixedDeltaTime;

                // Limitar movimiento al �rea definida
                targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
                targetPosition.y = currentPosition.y;  // Mantener posici�n en Y constante

                rb.MovePosition(targetPosition);

                // Ajustar la orientaci�n al jugador
                if ((directionToPlayer.x > 0 && !facingRight) || (directionToPlayer.x < 0 && facingRight))
                {
                    Flip();
                }
            }
            else
            {
                // Movimiento aleatorio solo en el eje X
                Vector2 newPosition = currentPosition + moveDirection * walkSpeed * Time.fixedDeltaTime;

                // Limitar movimiento al �rea definida
                newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
                newPosition.y = currentPosition.y;  // Mantener posici�n en Y constante

                rb.MovePosition(newPosition);

                // Ajustar la orientaci�n al moverse aleatoriamente
                if ((moveDirection.x > 0 && !facingRight) || (moveDirection.x < 0 && facingRight))
                {
                    Flip();
                }
            }
        }
    }

    private void AttackPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage); // Llama al m�todo de da�o en el personaje
            Debug.Log("El enemigo ha hecho da�o al personaje.");
        }
    }

    private void ChooseNewDirection()
    {
        float randomX = Random.Range(-1, 2);
        if (randomX == 0) randomX = Random.Range(-1, 2);
        moveDirection = new Vector2(randomX, 0).normalized;
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private bool IsGrounded()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 1f, groundLayer);
        return hit.collider != null;
    }
}
