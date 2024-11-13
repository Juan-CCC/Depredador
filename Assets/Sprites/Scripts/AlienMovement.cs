using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMovement : MonoBehaviour
{
    public float walkSpeed; // Velocidad al caminar
    public float runSpeed; // Velocidad al correr
    public float detectionRange; // Rango de detecci�n del personaje
    public float changeDirectionInterval;  // Tiempo para cambiar de direcci�n
    public Animator animator;  // Referencia al componente Animator
    public LayerMask groundLayer;  // Capa del suelo
    public float attackCooldownTime; // Tiempo entre ataques

    private Vector2 moveDirection;  // Direcci�n de movimiento
    private float timeToChangeDirection;  // Tiempo restante para cambiar direcci�n
    private float lastAttackTime; // Controla el tiempo entre ataques
    private Rigidbody2D rb;  // Referencia al Rigidbody2D del alien
    private GameObject player; // Referencia al personaje
    private bool facingRight = true; // Indica si est� mirando a la derecha
    private bool isChasing = false; // Indica si est� persiguiendo al personaje
    private bool isAttacking = false; //Indica si el enemigo esta atacando

    // M�todo Start se ejecuta al iniciar el juego
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Asignar Rigidbody2D
        player = GameObject.FindGameObjectWithTag("Player"); // Encontrar al personaje por etiqueta
        timeToChangeDirection = changeDirectionInterval;  // Iniciar tiempo
        ChooseNewDirection();  // Escoger una direcci�n inicial
    }

    // M�todo Update se ejecuta una vez por cada frame
    void Update()
    {
        if (!isAttacking)
        {
            // Detectar si el personaje est� dentro del rango de detecci�n
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= detectionRange)
            {
                isChasing = true; // Iniciar persecuci�n
            }
            else
            {
                isChasing = false; // Dejar de perseguir
            }

            // Cambiar direcci�n si no est� persiguiendo al personaje
            if (!isChasing)
            {
                timeToChangeDirection -= Time.deltaTime;
                if (timeToChangeDirection <= 0)
                {
                    ChooseNewDirection();
                    timeToChangeDirection = changeDirectionInterval;
                }
            }

            // Actualizar animaci�n: correr o caminar seg�n el estado
            if (isChasing)
            {
                Debug.Log("El enemigo est� persiguiendo al personaje.");
                animator.SetBool("run", true);
                animator.SetBool("walk", false);
            }
            else
            {
                animator.SetBool("run", false);
                animator.SetBool("walk", moveDirection != Vector2.zero);
            }
        }
        
    }

    private void FixedUpdate()
    {
        if (!isAttacking && IsGrounded())
        {
            if (isChasing)
            {
                // Perseguir al personaje con mayor velocidad
                // Calcular direcci�n solo en el eje X para mantener al enemigo en el suelo
                Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
                Vector2 horizontalDirection = new Vector2(directionToPlayer.x, 0); // Solo moverse en X
                rb.MovePosition(rb.position + horizontalDirection * runSpeed * Time.fixedDeltaTime);

                // Ajustar la orientaci�n del enemigo al personaje
                if ((horizontalDirection.x > 0 && !facingRight) || (horizontalDirection.x < 0 && facingRight))
                {
                    Flip();
                }
            }
            else
            {
                // Moverse aleatoriamente cuando no persigue
                Vector2 newPosition = rb.position + moveDirection * walkSpeed * Time.fixedDeltaTime;
                rb.MovePosition(newPosition);

                // Ajustar la orientaci�n al moverse aleatoriamente
                if ((moveDirection.x > 0 && !facingRight) || (moveDirection.x < 0 && facingRight))
                {
                    Flip();
                }
            }
        }
    }

    // Escoge una nueva direcci�n aleatoria
    private void ChooseNewDirection()
    {
        // Generar un valor aleatorio para X (-1, 0, 1)
        float randomX = Random.Range(-1, 2); // Puede ser -1 (izquierda), 0 (quieto) o 1 (derecha)
        if (randomX == 0) randomX = Random.Range(-1, 2); // Evitar que se quede quieto

        moveDirection = new Vector2(randomX, 0).normalized; // Movimiento solo en X
    }

    // Cambia la orientaci�n del enemigo invirtiendo el scale.x
    private void Flip()
    {
        facingRight = !facingRight; // Cambiar la direcci�n actual
        Vector3 localScale = transform.localScale;
        localScale.x *= -1; // Invertir el eje X
        transform.localScale = localScale;
    }


    // Verifica si el enemigo est� tocando el suelo usando Raycast
    private bool IsGrounded()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - 0.5f);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 1f, groundLayer);
        return hit.collider != null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isChasing /*&& Time.time >= lastAttackTime + attackCooldownTime*/)
        {
            Debug.Log("Enemigo ha colisionado con el jugador y est� en modo de ataque.");

            isAttacking = true;
            //isChasing = false;
            animator.SetBool("attack", true); // Activar animaci�n de ataque
            animator.SetBool("run", false);
            animator.SetBool("walk", false);

            // Asegurarse de que el enemigo permanezca en el suelo al atacar
            rb.velocity = Vector2.zero;  // Detener cualquier movimiento en Y
            Vector2 groundedPosition = new Vector2(transform.position.x, rb.position.y);
            rb.MovePosition(groundedPosition);

            /*lastAttackTime = Time.time; // Actualizar el �ltimo tiempo de ataque

            // Infligir da�o al personaje
            VidaPredator vidaPredator = collision.gameObject.GetComponent<VidaPredator>();
            if (vidaPredator != null)
            {
                vidaPredator.TakeDamage(5); // Asume que cada ataque inflige 20 puntos de da�o
                Debug.Log("Da�o infligido al jugador."); // Confirmaci�n de que se aplica el da�o
            }
            else
            {
                Debug.Log("Componente VidaPredator no encontrado en el jugador.");
            }*/

            // Llamar a una funci�n que desactive el estado de ataque despu�s de la animaci�n
            //StartCoroutine(AttackCooldown());

            StartCoroutine(ApplyDamageOverTime(collision.gameObject));
        }
    }

    private IEnumerator ApplyDamageOverTime(GameObject player)
    {
        VidaPredator vidaPredator = player.GetComponent<VidaPredator>();
        while (isAttacking && vidaPredator != null && vidaPredator.currentHealth > 0)
        {
            vidaPredator.TakeDamage(5); // Ajusta el da�o aqu�
            
            Debug.Log("Da�o infligido al jugador.");
            yield return new WaitForSeconds(attackCooldownTime); // Espera antes de infligir el pr�ximo da�o
        }

        animator.SetBool("attack", false); // Desactiva la animaci�n de ataque cuando termine el da�o
        isAttacking = false;
    }

    /*private IEnumerator AttackCooldown()
    {
        // Esperar a que termine la animaci�n de ataque (ajusta el tiempo seg�n la duraci�n de la animaci�n)
        yield return new WaitForSeconds(0.2f); // Breve pausa para que el enemigo permanezca en la animaci�n de ataque
        //animator.SetBool("attack", false); // Desactivar animaci�n de ataque
        yield return new WaitForSeconds(attackCooldownTime - 0.2f); // Esperar hasta el siguiente ataque permitido

        //isAttacking = false;
        //isChasing = true; // Reanudar la persecuci�n
    }*/

}
