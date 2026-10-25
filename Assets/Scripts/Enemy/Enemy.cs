using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public int vidas;
    private int currentHealth;
  
    private Change_Mats Change_Mat;

    public AudioSource audioDaño;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = vidas;
        Change_Mat = GetComponent<Change_Mats>();
        if (audioDaño == null)
        {
            audioDaño = GetComponent<AudioSource>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //muerte
        if(currentHealth <=0){
            Destroy(gameObject);
            Time.timeScale = 0f;
            Cursor.visible = true;                  
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Win_Scene");
           
           
        }
    }
    public void TakeDamage(int amount)
    {
        currentHealth-=amount;

        if (Change_Mat != null)
        {
        Change_Mat.ActivarChangeMat();
        }

        ReproducirSonidoDaño();
    }

    private void ReproducirSonidoDaño()
    {
        if (audioDaño != null)
        {
            
            if (audioDaño.clip != null)
            {
                audioDaño.PlayOneShot(audioDaño.clip);
            }
        }
    }
}
 