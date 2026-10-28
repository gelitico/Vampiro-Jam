using UnityEngine;

public class Enemy_Fly : MonoBehaviour
{
    public Transform Player;//posicion rotacion y escala del jugador
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
            GameObject jugadorBuscado = GameObject.FindWithTag("Player");//buscamos cualquier obj con el tag palyer
            
            if (jugadorBuscado != null)
            {
                Player = jugadorBuscado.transform;//si lo encuentra guarda el componente transform del del tag en player
            }
        }

        // solo si encuentra al jug se mueve hacia el
        if (Player != null)
        {
            // movimiento hacia el personaje        
            Vector3 targetPosition = new Vector3(Player.position.x, Player.position.y + alturaanyadida, Player.position.z);//creamos posicion de destino(target position) con las coordenadas del jugador
        
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, flightSpeed * Time.deltaTime);//movre towards mueve al enemigo desde su posicion actual, a la posicion de destino 
        }
    }
}