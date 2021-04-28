using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;

    [SerializeField] private XRRayInteractor _rayInteractor;
    
    [SerializeField] private LineRenderer _lineRenderer;
    
    [SerializeField] private TeleportationProvider provider;

    private InputAction thumbstick;

    private bool _isActive;

    // Start is called before the first frame update
    void Start()
    {
        _rayInteractor.enabled = false;

        var activate = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate");
        activate.Enable();
        var cancel = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Cancel");
        cancel.Enable();

        thumbstick = actionAsset.FindActionMap("XRI RightHand").FindAction("Move");
        thumbstick.Enable();

        activate.performed += OnTeleportActivate;
        cancel.performed += OnTeleportCancel;
    }

    private void OnTeleportActivate(InputAction.CallbackContext obj)
    {
        RayInteractorEnabled(true);
    }

    private void OnTeleportCancel(InputAction.CallbackContext obj)
    {
        RayInteractorEnabled(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (!_isActive)
            return;

        if (thumbstick.triggered)
            return;

        if (!_rayInteractor.GetCurrentRaycastHit(out RaycastHit hit))
        {
            RayInteractorEnabled(false);
            return;
        }

        var request = new TeleportRequest()
        {
            destinationPosition = hit.point,
        };

        provider.QueueTeleportRequest(request);
        RayInteractorEnabled(false);

    }

    private void RayInteractorEnabled(bool value)
    {
        _isActive = value;
        _rayInteractor.enabled = value;
        _lineRenderer.enabled = value;
    }

}