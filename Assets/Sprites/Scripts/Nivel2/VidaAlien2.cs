using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaAlien2 : MonoBehaviour
{
    [SerializeField] private float vida;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TomarDano(float dano)
    {
        vida -= dano;  // Restar el daño a la vida actual

        if (vida <= 0)
        {
            Muerte();
        }
    }

    private void Muerte()
    {
        animator.SetTrigger("muerte");
        Destroy(gameObject, 1f);  // Destruir el enemigo después de 1 segundo (opcional)
    }
}
