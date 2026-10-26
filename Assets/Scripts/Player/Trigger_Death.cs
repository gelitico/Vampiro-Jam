using UnityEngine;
using UnityEngine.SceneManagement;

public class Trigger_Death : MonoBehaviour
{
    public string deathSceneName = "Death_Scene";

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
            Cursor.visible = true;                  
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(deathSceneName);
           
            Destroy(other.gameObject);
            
        }
    }
}
