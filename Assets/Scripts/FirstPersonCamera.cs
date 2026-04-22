using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PrimeraPersona : MonoBehaviour
{
    [Header("Movimiento")]
    public float movementSpeed = 2f;
    public float sprintSpeed = 8f;
    public float crouchSpeed = 1f;
    public float gravity = -9.8f;

    [Header("Sprint")]
    public float sprintDuration = 3f;
    public float sprintCooldown = 10f;

    [Header("Crouch Altura")]
    public float crouchHeight = 1f;

    [Header("Cámara")]
    public Transform cameraTransform;
    public float sensitivity = 0.5f;
    public float minLimit = -80f;
    public float maxLimit = 80f;
    
    [Header("Linterna")]
    public Light flashlight;
    private bool flashlightOn = false;

    private PlayerInputAction _inputAction;
    private CharacterController _characterController;

    private Vector2 _movement;
    private Vector2 _look;
    private Vector3 _velocity;

    private float _currentRotationY;

    private bool isSprinting;
    private bool canSprint = true;
    private bool isCrouching;

    private float originalHeight;
    private Vector3 originalCenter;

    private Animator PlayerAnim;

    private Vector2 newDirection;

    private void Awake()
    {
        _inputAction = new PlayerInputAction();
        _characterController = GetComponent<CharacterController>();

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

        originalHeight = _characterController.height;
        originalCenter = _characterController.center;

        // Movimiento
        _inputAction.Player.Move.performed += ctx => _movement = ctx.ReadValue<Vector2>();
        _inputAction.Player.Move.canceled += ctx => _movement = Vector2.zero;

        // Cámara
        _inputAction.Player.Look.performed += ctx => _look = ctx.ReadValue<Vector2>();
        _inputAction.Player.Look.canceled += ctx => _look = Vector2.zero;

        // Sprint
        _inputAction.Player.Sprint.started += OnSprint;

        // Crouch
        _inputAction.Player.Crouch.started += OnCrouch;
        _inputAction.Player.Crouch.canceled += OnCrouch;

        // Linterna
        _inputAction.Player.Flashlight.started += OnFlashlight;

        // CORREGIDO: Animator con mayúscula
        PlayerAnim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Movement();
        Look();
        AnimLogic();
    }

    private void Movement()
    {
        // Movimiento horizontal
        Vector3 move = transform.right * _movement.x + transform.forward * _movement.y;

        float speed = movementSpeed;
        if (isCrouching) speed = crouchSpeed;
        else if (isSprinting) speed = sprintSpeed;

        _characterController.Move(move * speed * Time.deltaTime);

        // Gravedad
        if (_characterController.isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);

        // CORREGIDO: usar _movement en lugar de variables inexistentes
        newDirection = new Vector2(_movement.x, _movement.y);
    }

    private void Look()
    {
        Vector2 mouse = _look * sensitivity;

        _currentRotationY = Mathf.Clamp(_currentRotationY - mouse.y, minLimit, maxLimit);
        cameraTransform.localRotation = Quaternion.Euler(_currentRotationY, 0, 0);

        transform.Rotate(Vector3.up * mouse.x);
    }

    private void OnSprint(InputAction.CallbackContext context)
    {
        if (canSprint && !isCrouching)
        {
            StartCoroutine(SprintRoutine());
        }
    }

    private void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isCrouching = true;
            _characterController.height = crouchHeight;
            _characterController.center = new Vector3(originalCenter.x, crouchHeight / 2f, originalCenter.z);
            Debug.Log("Agachado");
        }
        else if (context.canceled)
        {
            isCrouching = false;
            _characterController.height = originalHeight;
            _characterController.center = originalCenter;
            Debug.Log("De pie");
        }
    }

    private IEnumerator SprintRoutine()
    {
        canSprint = false;
        isSprinting = true;
        Debug.Log("Sprint activado");

        yield return new WaitForSeconds(sprintDuration);

        isSprinting = false;
        Debug.Log("Sprint terminado");

        yield return new WaitForSeconds(sprintCooldown);

        canSprint = true;
        Debug.Log("Sprint disponible otra vez");
    }

    private void OnFlashlight(InputAction.CallbackContext context)
    {
        flashlightOn = !flashlightOn; 
        if (flashlight != null)
            flashlight.enabled = flashlightOn;

        Debug.Log("Linterna: " + (flashlightOn ? "Encendida" : "Apagada"));
    }

    public void AnimLogic()
    {
        PlayerAnim.SetFloat("X", newDirection.x);
        PlayerAnim.SetFloat("Y", newDirection.y);
        // PlayerAnim.SetBool("isCrouching", isCrouching);//
    }
}
