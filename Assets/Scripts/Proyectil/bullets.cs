using UnityEngine;

public class bullets : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            return;

        Enemy enemy = other.GetComponent<Enemy>();

        if(enemy != null)
            enemy.TakeDamage(1);


        //destruir el ajo si impacta
        Destroy(gameObject);      
    }
}
