using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    public Transform transform;
    public float ShootStrength;
    public GameObject AjoProjectile;
    private InputAction shootAction;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shootAction = InputSystem.actions.FindAction("Attack");
        shootAction.started += OnAttackButtonPressed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Fire()
    {
        print("shootin");
        GameObject newProjectile = Instantiate(AjoProjectile, transform.position, Quaternion.identity);

        Rigidbody rigidbody = newProjectile.GetComponent<Rigidbody>();
        rigidbody.AddForce(transform.forward*ShootStrength, ForceMode.Impulse);
    }
    private void OnAttackButtonPressed(InputAction.CallbackContext _)
    {
        Fire();
    }
    private void OnDestroy()
    {
        shootAction.started-=OnAttackButtonPressed;
    }
}
