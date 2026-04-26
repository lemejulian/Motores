using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float reachDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform holdPosition;

    private GameObject heldObject = null;
    private Shot heldWeapon = null; 
    private Camera cam;

    private PlayerInputAction controls;

    [Header("UI")]
    public TextMeshProUGUI ammoText; // referencia al texto en el Canvas

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        controls = new PlayerInputAction();
    }

    void Start()
    {
        UpdateAmmoUI(); // inicializa el HUD correctamente al arrancar
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Interactions.performed += OnInteract;
        controls.Player.Shoot.performed += OnShoot;
        controls.Player.Reload.performed += OnReload;
    }

    void OnDisable()
    {
        controls.Player.Interactions.performed -= OnInteract;
        controls.Player.Shoot.performed -= OnShoot;
        controls.Player.Reload.performed -= OnReload;
        controls.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (heldObject == null) TryPickUp();
        else DropObject();
        UpdateAmmoUI();
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (heldWeapon != null)
        {
            heldWeapon.Shoot();
            UpdateAmmoUI();
        }
    }

    private void OnReload(InputAction.CallbackContext context)
    {
        if (heldWeapon != null)
        {
            heldWeapon.StartReload();
            UpdateAmmoUI();
        }
    }

    void TryPickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, reachDistance, interactableLayer))
        {
            heldObject = hit.collider.gameObject;
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            heldObject.transform.SetParent(holdPosition);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;

            heldWeapon = heldObject.GetComponent<Shot>();
        }
    }

    void DropObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        heldObject.transform.SetParent(null);
        heldObject = null;
        heldWeapon = null; 
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            if (heldWeapon != null)
                ammoText.text = heldWeapon.currentAmmo + " / " + heldWeapon.maxAmmo;
            else
                ammoText.text = ""; // vacío si no tenés arma
        }
    }
}

