using UnityEngine;

public class PersistAudio : MonoBehaviour
{
   
    

    void Awake()
    {
            DontDestroyOnLoad(gameObject);
        }
       
    
}
