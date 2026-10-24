using UnityEngine;

public class StopMusic : MonoBehaviour
{
    void Awake()
    {
       
        GameObject musicObject = GameObject.Find("MusicManager"); 
        
        if (musicObject != null)
        {
            Destroy(musicObject); 
        }
    }
}