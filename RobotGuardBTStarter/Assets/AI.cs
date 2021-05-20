using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using Panda;

public class AI : MonoBehaviour
{
    public Transform player;
    public Transform bulletSpawn;
    public Slider healthBar;   
    public GameObject bulletPrefab;

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
        InvokeRepeating("UpdateHealth",5,0.5f);
    }

    void Update()
    {
        Vector3 healthBarPos = Camera.main.WorldToScreenPoint(this.transform.position);// criação da barra de vida
        healthBar.value = (int)health;//-----
        healthBar.transform.position = healthBarPos + new Vector3(0,60,0);
    }

    void UpdateHealth()// metodo de vida.
    {
       if(health < 100)// condição de vida.
        health ++;// contador adição de vida 
    }

    void OnCollisionEnter(Collision col)// metodo de colisão
    {
        if(col.gameObject.tag == "bullet")
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
    public void MoveToDestination()// metodo de movimentação para o destino
    { if (Task.isInspected)
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        { Task.current.Succeed();

        }
    }


}

