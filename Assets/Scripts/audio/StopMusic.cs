using UnityEngine;

public class StopMusic : MonoBehaviour
{
    void Awake() // automatico cuando aparece el obj del script en escena
    {
       
        GameObject musicObject = GameObject.Find("MusicManager"); 
        
        if (musicObject != null)
        {
            Destroy(musicObject); 
        }
    }
}