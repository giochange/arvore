using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Panda;

public class AI : MonoBehaviour
{
    public Transform player;//transforme do player
    public Transform bulletSpawn;// transforme da bala spawnada
    public Slider healthBar;   // status bar de vida
    public GameObject bulletPrefab;//objeto bala

    NavMeshAgent agent;// agent navmash
    public Vector3 destination; // The movement destination.
    public Vector3 target;      // The position to aim to.
    float health = 100.0f;// variavel de vida
    float rotSpeed = 5.0f;// variavel de rotação

    float visibleRange = 80.0f;// alcance de perseguição
    float shotRange = 40.0f;// alcance de tiro

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();//pagada do componente navemash
        agent.stoppingDistance = shotRange - 5; //for a little buffer
        InvokeRepeating("UpdateHealth",5,0.5f);//repetição
    }

    void Update()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(this.transform.position);// criação da barra de vida
        healthBar.value = (int)health;//-----
        healthBar.transform.position = healthBarPos + new Vector3(0,60,0);//posição de mostragem da status bar
    }

    void UpdateHealth()// metodo de vida.
    {
       if(health < 100)// condição de vida.
        health ++;// contador adição de vida 
    }

    void OnCollisionEnter(Collision col)// metodo de colisão
    {
        if(col.gameObject.tag == "bullet")// codição de reduçao de vida
        {
            health -= 10;
        }
    }



    [Task]
    public void PickRandomDestination() // metodo de escolha aleatoria do destino
    { Vector3 dest = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
        agent.SetDestination(dest); Task.current.Succeed();
    }


    [Task]
    public void PickDestination(int x, int z)
    {
        Vector3 dest = new Vector3(x, 0, z);
        agent.SetDestination(dest);
        Task.current.Succeed();
    }


    [Task]
    public void MoveToDestination()// metodo de movimentação para o destino
    {
        if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            Task.current.Succeed();

        }
    }



    [Task]
    public void TargetPlayer()//metodo de alvo de objetivo-player
    {
        target = player.transform.position;//pegada da posição do player
        Task.current.Succeed();// validação
    }


    [Task]
    public bool Fire()//metodo de tiro
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab,//instanciamento da bala
            bulletSpawn.transform.position, bulletSpawn.transform.rotation);// direcionameto do tiro

        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 2000);// adição de fisica na bala
        return true;// rutorno do valor
    }



    [Task]
    public void LookAtTarget()//metodo de visualização de alvo
    { Vector3 direction = target - this.transform.position;// pegada de posição para direção-realizando uma subtração de um ponto b-a
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, //orientação de rotação
            Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);// velocidade de rotação
        if (Task.isInspected) Task.current.debugInfo = string.Format("angle={0}",//teste 
            Vector3.Angle(this.transform.forward, direction));//movimentação
        if (Vector3.Angle(this.transform.forward, direction) < 5.0f)//condição perante a movimentação
        {
            Task.current.Succeed();
        }
    }


    [Task]
    bool SeePlayer()// metodo de ver player
    {
        Vector3 distance = player.transform.position - this.transform.position;// setagem de distancia perante o player
        RaycastHit hit;// variavel raycast
        bool seeWall = false;// setagem do bool para falso
        Debug.DrawRay(this.transform.position, distance, Color.red);//test do reycast
        if (Physics.Raycast(this.transform.position, distance, out hit))//codição perante o raycast
        {
            if (hit.collider.gameObject.tag == "wall")// se o ray collidir com wall

            {
                seeWall = true;//ver wall iggual true

            }
        }

        if (Task.isInspected)
            Task.current.debugInfo = string.Format("wall={0}", seeWall);//visuaização do wall
        if (distance.magnitude < visibleRange && !seeWall) return true;
        else
            return false;
    }



    [Task]
    bool Turn ( float angle)// virada
    {
        var p = this.transform.position + Quaternion.AngleAxis(angle, Vector3.up) * this.transform.forward;
        target = p;
        return true;
    }

    
}

