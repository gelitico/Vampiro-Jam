using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Enemy_Collide : MonoBehaviour
{
    public ProjectileSpawner projectileSpawner;
    public string deathSceneName = "Death_Scene";
    private bool nearAmmo;

    private Camera camaraPrincipal; 

    public LayerMask capaInteractuable; 
    public float distanciaMaximaRayo = 3f;

    private GameObject cajaAmmoCercana;

    private ControlManoUI manoUI;
    private bool estabaMirandoAmmo = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            print("DEAD");
            
            Cursor.visible = true;                  
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(deathSceneName);
            Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "Ammo")
        {
            nearAmmo = true;
            cajaAmmoCercana = other.gameObject; // Guardamos la caja para saber cuál destruir
            print("press e to grab garlic ammo");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag=="Ammo")
        {
            nearAmmo = false;
            cajaAmmoCercana = null;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camaraPrincipal = Camera.main;

        manoUI = FindObjectOfType<ControlManoUI>();
    }

    // Update is called once per frame
    void Update()
    {
        bool mirandoAmmoAhora = false;

        if (nearAmmo)
        {
            mirandoAmmoAhora = EstaMirandoAlObjeto();

           
            if (mirandoAmmoAhora && !estabaMirandoAmmo)
            {
                if (manoUI != null)
                {
                    manoUI.ActivarAccion("lanzar");
                }
            }
        }

        estabaMirandoAmmo = mirandoAmmoAhora;

         if(nearAmmo && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EstaMirandoAlObjeto())
            {

                if (manoUI != null)
                {
                    manoUI.ActivarAccion("coger");
                }
                
                if (projectileSpawner != null)
                {
                    projectileSpawner.canShoot = true;
                    print("shooting enabled");
                }

                // Si tenemos la referencia de la caja, la borramos del mapa porque ya la hemos cogido
                if (cajaAmmoCercana != null)
                {
                    Destroy(cajaAmmoCercana);
                }
                // Reseteamos las variables de control al desaparecer la caja
                nearAmmo = false;
                cajaAmmoCercana = null;
            }
        }
    }

    private bool EstaMirandoAlObjeto()
    {
        if (camaraPrincipal == null) return false;

        Ray rayo = camaraPrincipal.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(rayo, out hit, distanciaMaximaRayo, capaInteractuable))
        {
            if (hit.transform.CompareTag("Ammo"))
            {
                return true; 
            }
        }
        return false;
    }
}