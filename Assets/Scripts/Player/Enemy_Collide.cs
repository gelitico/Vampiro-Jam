using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Enemy_Collide : MonoBehaviour
{
    public ProjectileSpawner projectileSpawner;
    public string deathSceneName = "Death_Scene";
    private bool nearAmmo;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            print("DEAD");
            
            Cursor.visible = true;                  
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(deathSceneName);
        }
        else if(other.gameObject.tag == "Ammo")
        {
            nearAmmo = true;
            print("press e to grab garlic ammo");
        }
        
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag=="Ammo")
        {
            nearAmmo = false;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
         if(Keyboard.current.eKey.wasPressedThisFrame && nearAmmo)
        {
            projectileSpawner.canShoot = true;
            print("shooting enabled");
        }
    }
}
