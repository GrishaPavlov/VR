using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;

    [SerializeField] private XRRayInteractor rayInteractor;

    // [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private TeleportationProvider provider;

    private InputAction _thumbstick;

    private bool _isActive;

    private InputAction _activate;
    private InputAction _cancel;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        rayInteractor.enabled = false;

        _activate = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Activate");
        _activate.Enable();
        _cancel = actionAsset.FindActionMap("XRI RightHand").FindAction("Teleport Mode Cancel");
        _cancel.Enable();

        _thumbstick = actionAsset.FindActionMap("XRI RightHand").FindAction("Move");
        _thumbstick.Enable();

        _activate.performed += OnTeleportActivate;
        _cancel.performed += OnTeleportCancel;
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
        if (!_isActive || _thumbstick.triggered)
            return;

        if (!rayInteractor.GetCurrentRaycastHit(out RaycastHit hit))
        {
            RayInteractorEnabled(false);
            return;
        }
        if (hit.collider.gameObject.layer == 6)
        {
            var request = new TeleportRequest()
            {
                destinationPosition = hit.point,
            };
            provider.QueueTeleportRequest(request);
        }
        RayInteractorEnabled(false);
    }

    private void RayInteractorEnabled(bool value)
    {
        _isActive = value;
        rayInteractor.enabled = value;
        // lineRenderer.enabled = value;
    }

    private void OnDisable()
    {
        _activate.performed -= OnTeleportActivate;
        _cancel.performed -= OnTeleportCancel;
    }
}