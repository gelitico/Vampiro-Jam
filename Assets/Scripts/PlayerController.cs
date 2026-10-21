using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerController : MonoBehaviour
{
    public float WalkingSpeed;
    public float JumpImpulse;
    public float Gravity;
    public float CameraSensitivity;
    public float CameraAngleMax;

    private Camera mainCamera;
    private CharacterController characterController;
    
    private InputAction moveInput;
    private InputAction jumpInput;
    private bool Jumping = false;
    
    private float MoveSpeed;
    private Vector3 Direction;

    private float LookAngle;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        moveInput = InputSystem.actions.FindAction("Move");
        jumpInput = InputSystem.actions.FindAction("Jump");

        jumpInput.started += Jump;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        MoveSpeed = WalkingSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVector = moveInput.ReadValue<Vector2>();
        Vector2 mouseDelta = new Vector2(Mouse.current.delta.x.ReadValue(), Mouse.current.delta.y.ReadValue());

        if(!characterController.isGrounded)
            Jumping=false;
       
        HandleMovement(moveVector);
        HandleLooking(mouseDelta);
    }

    private void HandleMovement(Vector2 moveVector)
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float oldY = Direction.y;

        Vector2 newSpeed = new Vector2(moveVector.y * MoveSpeed, moveVector.x * MoveSpeed);
        Direction = (forward*newSpeed.x) + (right*newSpeed.y);
        Direction.y = oldY;

        newSpeed = new Vector2(moveVector.y * MoveSpeed, moveVector.x * MoveSpeed);

        Direction = (forward * newSpeed.x) + (right * newSpeed.y);
        
        if(Jumping && characterController.isGrounded)
        {
            Direction.y = JumpImpulse;   
        }
        else
        Direction.y = oldY;

        //Gravedad
        if(!characterController.isGrounded)
        {
            Direction.y -= Gravity * Time.deltaTime;
        }

        characterController.Move(Direction * Time.deltaTime);
    }

    private void Jump(InputAction.CallbackContext _)
    {
        Jumping=true;
    }

    private void HandleLooking(Vector2 mouseDelta)
    {
        LookAngle += -mouseDelta.y * CameraSensitivity;
        LookAngle = Mathf.Clamp(LookAngle, -CameraAngleMax, CameraAngleMax);

        //rotacion de la camara en el eje X
        mainCamera.transform.localRotation = Quaternion.Euler(LookAngle, 0, 0);

        //rotacion del personaje entero en el eje Y
        transform.rotation *= Quaternion.Euler(0, mouseDelta.x * CameraSensitivity, 0);
    }
}
