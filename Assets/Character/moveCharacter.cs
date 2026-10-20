using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCharacter : MonoBehaviour
{
    public InputActionAsset inputActions;
    InputActions moveAction;
    CharacterController controller;

    public float MoveSpeed;

    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        //devuelve un Vector2 con el la direccion en la que mueves al personaje
        if(inputActions!=null)
        {
            moveAction = inputActions.FindAction("Move");
        }

        // devuelve un puntero a el componente character controller del personaje (con su x,y, ...)
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(inputActions!=null)
        {
            Vector2 inputValue = moveAction.ReadValue<Vector2>();
            Vector3 direction = (inputValue.x, 0, inputValue.y);

            if(controller!=null)
            {
                controller.Move(direction * MoveSpeed * Time.deltaTime);
            }

            if(transform!=null)
            {
                transform.LookAt(transform.position+direction);
            }
        }
    }





}