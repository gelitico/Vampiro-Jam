using UnityEngine;

public class Trigger_Superficie : MonoBehaviour
{
    private bool jugadorCerca = false; 
    private GameObject jugadorRef; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            jugadorCerca = true;
            jugadorRef = other.gameObject;
            print("va cagao ves");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;
            jugadorRef = null;
        }
    }
}
