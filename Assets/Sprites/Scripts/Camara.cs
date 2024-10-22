using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public GameObject target;

    //posicion del jugador
    private float target_poseX;
    private float target_poseY;

    //posicion de la camara
    private float posX;
    private float posY;

    public float derechaMax;
    public float izquierdaMax;

    public float alturaMax;
    public float alturaMin;

    //Velocidad del desplazamiento de la camara
    public float speed;
    public bool encendida = true;

    void Awake()
    {
        posX = target_poseX + derechaMax;
        posY = target_poseY + alturaMin;
        transform.position = Vector3.Lerp(transform.position, new Vector3(posX, posY, -1), -1);
    }

    void Move_Cam()
    {
        if(encendida)
        {
            if (target)
            {
                target_poseX = target.transform.position.x;
                target_poseY = target.transform.position.y;

                if (target_poseX > derechaMax && target_poseX < izquierdaMax)
                {
                    posX = target_poseX;
                }
                if (target_poseY < alturaMax && target_poseY > alturaMin)
                {
                    posY = target_poseY;
                }
            }
            transform.position = Vector3.Lerp(transform.position, new Vector3(posX, posY, -1), speed * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move_Cam();
    }
}
