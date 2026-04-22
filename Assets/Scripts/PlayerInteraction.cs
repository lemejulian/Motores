using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float reachDistance = 3f;
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private Transform holdPosition;

    private GameObject heldObject = null;
    private Camera cam;

    private PlayerInputAction controls;

    void Awake()
    {
        cam = GetComponentInChildren<Camera>();
        controls = new PlayerInputAction();
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.Interactions.performed += OnInteract;
    }

    void OnDisable()
    {
        controls.Disable();
        controls.Player.Interactions.performed -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (heldObject == null) TryPickUp();
        else DropObject();
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
        }
    }

    void DropObject()
    {
        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        heldObject.transform.SetParent(null);
        heldObject = null;
    }

}
