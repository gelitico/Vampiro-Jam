using UnityEngine;

public class Enemy_Fly : MonoBehaviour
{
    public Transform Player;
    public float flightSpeed = 5f; 
    public float alturaanyadida = 1f;

    void Update()
    {
        
        
            Vector3 targetPosition = new Vector3(Player.position.x, Player.position.y+alturaanyadida, Player.position.z);

            
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, flightSpeed * Time.deltaTime);

        
    }
}