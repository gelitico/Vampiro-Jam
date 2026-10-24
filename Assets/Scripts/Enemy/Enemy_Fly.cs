using UnityEngine;

public class Enemy_Fly : MonoBehaviour
{
    public Transform Player;
    public float flightSpeed = 5f; 
    public float alturaanyadida = 1f;

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
            Vector3 targetPosition = new Vector3(Player.position.x, Player.position.y + alturaanyadida, Player.position.z);
        
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, flightSpeed * Time.deltaTime);
        }
    }
}