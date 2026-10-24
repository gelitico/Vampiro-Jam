using UnityEngine;
using UnityEngine.InputSystem;

public class ProjectileSpawner : MonoBehaviour
{
    public Transform puntoDeDisparo;
    public float ShootStrength;
    public GameObject AjoProjectile;
    private InputAction shootAction;

    public bool canShoot = false;

    private ControlManoUI manoUI;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootAction = InputSystem.actions.FindAction("Attack");
        shootAction.started += OnAttackButtonPressed;

        manoUI = FindObjectOfType<ControlManoUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Fire()
    {
        print("shootin");
        GameObject newProjectile = Instantiate(AjoProjectile, puntoDeDisparo.position, Quaternion.identity);

        Rigidbody rigidbody = newProjectile.GetComponent<Rigidbody>();
        rigidbody.AddForce(puntoDeDisparo.forward*ShootStrength, ForceMode.Impulse);

        if (manoUI != null)
        {
            manoUI.ActivarAccion("lanzar");
        }
    }
    private void OnAttackButtonPressed(InputAction.CallbackContext _)
    {
        if(canShoot)
        Fire();
    }
    private void OnDestroy()
    {
        shootAction.started-=OnAttackButtonPressed;
    }

    
}
