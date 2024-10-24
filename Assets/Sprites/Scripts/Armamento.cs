using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armamento : MonoBehaviour
{
    public DisparoPredator disparoPredator; // Referencia al script de disparo
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().enabled = false;
            gameObject.transform.GetChild(0).gameObject.SetActive(true);

            // Habilitar disparo en el script del jugador
            if (disparoPredator != null)
            {
                disparoPredator.PuedeDisparar = true;
            }
            Destroy(gameObject, 0.5f);
        }
    }
}
