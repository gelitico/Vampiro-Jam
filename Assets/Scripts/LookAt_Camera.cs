using UnityEngine;

public class LookAt_Camera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destino = Camera.main.transform.position;

        
        destino.y = transform.position.y;

        
        transform.LookAt(destino);
    }
}
