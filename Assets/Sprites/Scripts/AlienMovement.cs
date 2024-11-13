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
    public float attackCooldownTime; // Tiempo entre ataques

    private Vector2 moveDirection;  // Dirección de movimiento
    private float timeToChangeDirection;  // Tiempo restante para cambiar dirección
    private float lastAttackTime; // Controla el tiempo entre ataques
    private Rigidbody2D rb;  // Referencia al Rigidbody2D del alien
    private GameObject player; // Referencia al personaje
    private bool facingRight = true; // Indica si está mirando a la derecha
    private bool isChasing = false; // Indica si está persiguiendo al personaje
    private bool isAttacking = false; //Indica si el enemigo esta atacando

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
        if (!isAttacking)
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
        
    }

    private void FixedUpdate()
    {
        if (!isAttacking && IsGrounded())
        {
            if (isChasing)
            {
                // Perseguir al personaje con mayor velocidad
                // Calcular dirección solo en el eje X para mantener al enemigo en el suelo
                Vector2 directionToPlayer = (player.transform.position - transform.position).normalized;
                Vector2 horizontalDirection = new Vector2(directionToPlayer.x, 0); // Solo moverse en X
                rb.MovePosition(rb.position + horizontalDirection * runSpeed * Time.fixedDeltaTime);

                // Ajustar la orientación del enemigo al personaje
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isChasing /*&& Time.time >= lastAttackTime + attackCooldownTime*/)
        {
            Debug.Log("Enemigo ha colisionado con el jugador y está en modo de ataque.");

            isAttacking = true;
            //isChasing = false;
            animator.SetBool("attack", true); // Activar animación de ataque
            animator.SetBool("run", false);
            animator.SetBool("walk", false);

            // Asegurarse de que el enemigo permanezca en el suelo al atacar
            rb.velocity = Vector2.zero;  // Detener cualquier movimiento en Y
            Vector2 groundedPosition = new Vector2(transform.position.x, rb.position.y);
            rb.MovePosition(groundedPosition);

            /*lastAttackTime = Time.time; // Actualizar el último tiempo de ataque

            // Infligir daño al personaje
            VidaPredator vidaPredator = collision.gameObject.GetComponent<VidaPredator>();
            if (vidaPredator != null)
            {
                vidaPredator.TakeDamage(5); // Asume que cada ataque inflige 20 puntos de daño
                Debug.Log("Daño infligido al jugador."); // Confirmación de que se aplica el daño
            }
            else
            {
                Debug.Log("Componente VidaPredator no encontrado en el jugador.");
            }*/

            // Llamar a una función que desactive el estado de ataque después de la animación
            //StartCoroutine(AttackCooldown());

            StartCoroutine(ApplyDamageOverTime(collision.gameObject));
        }
    }

    private IEnumerator ApplyDamageOverTime(GameObject player)
    {
        VidaPredator vidaPredator = player.GetComponent<VidaPredator>();
        while (isAttacking && vidaPredator != null && vidaPredator.currentHealth > 0)
        {
            vidaPredator.TakeDamage(5); // Ajusta el daño aquí
            
            Debug.Log("Daño infligido al jugador.");
            yield return new WaitForSeconds(attackCooldownTime); // Espera antes de infligir el próximo daño
        }

        animator.SetBool("attack", false); // Desactiva la animación de ataque cuando termine el daño
        isAttacking = false;
    }

    /*private IEnumerator AttackCooldown()
    {
        // Esperar a que termine la animación de ataque (ajusta el tiempo según la duración de la animación)
        yield return new WaitForSeconds(0.2f); // Breve pausa para que el enemigo permanezca en la animación de ataque
        //animator.SetBool("attack", false); // Desactivar animación de ataque
        yield return new WaitForSeconds(attackCooldownTime - 0.2f); // Esperar hasta el siguiente ataque permitido

        //isAttacking = false;
        //isChasing = true; // Reanudar la persecución
    }*/

}
