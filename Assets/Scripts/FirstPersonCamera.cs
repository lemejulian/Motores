using UnityEngine;
using UnityEngine.InputSystem;

public class PrimeraPersona : MonoBehaviour
{
    [Header("Movimiento")]
    public float movementSpeed = 2f;
    public float gravity = -9.8f;

    [Header("Cámara")]
    public Transform cameraTransform;
    public float sensitivity = 0.5f;
    public float minLimit = -80f;
    public float maxLimit = 80f;

    private PlayerInputAction _inputAction;
    private CharacterController _characterController;

    private Vector2 _movement;
    private Vector3 _velocity;
    private Vector2 _look;

    private float _currentRotationY;

    private void Awake()
    {
        _inputAction = new PlayerInputAction();
        _characterController = GetComponent<CharacterController>();

        // 🔹 Si no asignaste la cámara en el Inspector, usa la principal automáticamente
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        _inputAction.Player.Enable();

        // Movimiento
        _inputAction.Player.Move.performed += SetMovement;
        _inputAction.Player.Move.canceled += SetMovement;

        // Mirar
        _inputAction.Player.Look.performed += SetLook;
        _inputAction.Player.Look.canceled += SetLook;
    }

    private void SetMovement(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    private void SetLook(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        Movement();
        Look();
    }

    private void Movement()
    {
        // Movimiento horizontal
        Vector3 move = transform.right * _movement.x + transform.forward * _movement.y;
        _characterController.Move(move * movementSpeed * Time.deltaTime);

        // Gravedad
        if (_characterController.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void Look()
    {
        // Normalizar movimiento del mouse
        Vector2 mouseNormalized = _look * sensitivity;

        // Rotación vertical (clamp para no girar demasiado)
        _currentRotationY = Mathf.Clamp(_currentRotationY - mouseNormalized.y, minLimit, maxLimit);
        cameraTransform.localRotation = Quaternion.Euler(_currentRotationY, 0, 0);

        // Rotación horizontal
        transform.Rotate(Vector3.up * mouseNormalized.x);
    }
}
