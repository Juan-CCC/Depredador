using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private float dano;

    private Vector3 direccion;

    // Update is called once per frame
    private void Update()
    {
        //transform.Translate(Vector2.right * velocidad * Time.deltaTime);

        // Mover la bala en la dirección establecida
        transform.Translate(direccion * velocidad * Time.deltaTime);
    }

    // Método para establecer la dirección de la bala
    public void SetDireccion(Vector3 nuevaDireccion)
    {
        direccion = nuevaDireccion.normalized; // Asegurar que la dirección esté normalizada
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemigo"))
        {
            other.GetComponent<Enemigo>().TomarDano(dano);
            Destroy(gameObject);
        }
    }
}
