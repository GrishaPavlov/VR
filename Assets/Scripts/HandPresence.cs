using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

public class HandPresence : MonoBehaviour
{
    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject handModelPrefab;
    public InputActionProperty grip;
    public InputActionProperty trigger;
    
    private InputDevice _targetDevice;
    private GameObject _spawnedHandModel;
    private Animator _handAnimator;
    private void Start()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in  devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            _targetDevice = devices[0];

            _spawnedHandModel = Instantiate(handModelPrefab, transform);
            _handAnimator = _spawnedHandModel.GetComponent<Animator>();
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void UpdateHandAnimation()
    {
        _handAnimator.SetFloat("Grip", grip.action.ReadValue<float>());
        _handAnimator.SetFloat("Trigger", trigger.action.ReadValue<float>());
        Debug.Log(grip.action.ReadValue<float>());
        // if (_targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        // {
        //     _handAnimator.SetFloat("Trigger", triggerValue);
        // }
        // else
        // {
        //     _handAnimator.SetFloat("Trigger", 0);   
        // }
        // if (_targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        // {
        //     _handAnimator.SetFloat("Grip", gripValue);
        // }
        // else
        // {
        //     _handAnimator.SetFloat("Grip", 0);   
        // }
    }
    
    void Update()
    {
        UpdateHandAnimation();
    }
}
