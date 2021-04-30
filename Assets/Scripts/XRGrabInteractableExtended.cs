using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableExtended : XRGrabInteractable
{
    public bool Offset = false;

    private Vector3 interactorPosition = Vector3.zero;
    private Quaternion interactorRotation = Quaternion.identity;

    protected override void OnSelectEntering(XRBaseInteractor interactor)
    {
        base.OnSelectEntering(interactor);
        if (Offset)
        {
            StoreInteractor(interactor);
            MatchAttachmentPoints(interactor);
        }
    }

    private void StoreInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = interactor.attachTransform.localPosition;
        interactorRotation = interactor.attachTransform.localRotation;
    }

    private void MatchAttachmentPoints(XRBaseInteractor interactor)
    {
        bool hasAttached = attachTransform != null;
        interactor.attachTransform.position = hasAttached ? attachTransform.position : transform.position;
        interactor.attachTransform.rotation = hasAttached ? attachTransform.rotation : transform.rotation;
    }

    protected override void OnSelectExiting(XRBaseInteractor interactor)
    {
        base.OnSelectExiting(interactor);
        if (Offset)
        {
            ResetAttachmentPoints(interactor);
            ClearInteractor(interactor);
        }
    }

    private void ResetAttachmentPoints(XRBaseInteractor interactor)
    {
        interactor.attachTransform.localPosition = interactorPosition;
        interactor.attachTransform.localRotation = interactorRotation;
    }

    private void ClearInteractor(XRBaseInteractor interactor)
    {
        interactorPosition = Vector3.zero;
        interactorRotation = quaternion.identity;
    }
}