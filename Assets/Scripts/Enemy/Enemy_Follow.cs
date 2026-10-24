using UnityEngine;
using UnityEngine.AI;

public class Enemy_Follow : MonoBehaviour
{

    public NavMeshAgent enemy;
    public Transform Player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            GameObject jugadorBuscado = GameObject.FindWithTag("Player");
            
            if (jugadorBuscado != null)
            {
                Player = jugadorBuscado.transform;
            }
        }

        // 2. SOLO SI YA ENCONTRÓ AL JUGADOR, SE MUEVE HACIA ÉL
        if (Player != null)
        {
            // Movimiento hacia el personaje        
            enemy.SetDestination(Player.position);
        }
    }
}