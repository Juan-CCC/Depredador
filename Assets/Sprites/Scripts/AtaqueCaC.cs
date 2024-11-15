using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtaqueCaC : MonoBehaviour
{
    [SerializeField] private Transform controladorGolpe;
    [SerializeField] private float radioGolpe;
    [SerializeField] private float danoGolpe;
    //[SerializeField] private float tiempoEntreAtaques;
    //[SerializeField] private float tiempoSiguienteAtaque;


    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator> ();
    }

    private void Update()
    {
        /*if (tiempoSiguienteAtaque >0)
        {
            tiempoSiguienteAtaque -= Time.deltaTime;
        }*/

        if (Input.GetKeyDown(KeyCode.X)/* && tiempoSiguienteAtaque <= 0*/)
        {
            Golpe();
            //tiempoSiguienteAtaque = tiempoEntreAtaques;
        }
    }

    private void Golpe()
    {
        animator.SetTrigger("golpe");

        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorGolpe.position, radioGolpe);
        foreach (Collider2D colisionador in objetos)
        {
            if (colisionador.CompareTag("Enemigo"))
            {
                colisionador.transform.GetComponent<Enemigo>().TomarDano(danoGolpe);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorGolpe.position, radioGolpe);
    }
}
