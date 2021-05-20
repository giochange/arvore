using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour {

	float speed = 20.0F;// variavel de velocidade
    float rotationSpeed = 120.0F;// varivel de rotação
    public GameObject bulletPrefab;// game object prefabe da bala
    public Transform bulletSpawn;// responsavel por spawn na fase

    void Update() {
        float translation = Input.GetAxis("Vertical") * speed;// realização da movimentação
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;// realização da rotação
        translation *= Time.deltaTime;// mutiplicação de movimentação pelo delta time
        rotation *= Time.deltaTime;//mutiplicação de rotação pelo delta time
        transform.Translate(0, 0, translation);// setagem de qual eixo sera movimentado
        transform.Rotate(0, rotation, 0);// setagem de qual eixo vai rotacionar

        if(Input.GetKeyDown("space"))// condição ao clicar space
        {
            GameObject bullet = GameObject.Instantiate(bulletPrefab, bulletSpawn.transform.position, bulletSpawn.transform.rotation);//instaciament do objeto dentro da fase
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward*2000);// adição de fisica no projetil
        }
    }
}
