using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Layers;
using Esri.GameEngine.Map;
using Esri.HPFramework;
using Esri.Unity;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Esri.ArcGISMapsSDK.Samples.Components;

public class XRTableTopInteractor : MonoBehaviour
{
    [SerializeField] private ArcGISMapComponent arcGISMapComponent;
    private Vector3 dragStartPoint = Vector3.zero;
    private double4x4 dragStartWorldMatrix;
    [SerializeField] private HPRoot hpRoot;
    private bool isDragging = false;
    [SerializeField] private ArcGISTabletopControllerComponent tableTop;
    [SerializeField] private float radiusScalar = 1.0f;
    [SerializeField] private GameObject tableTopGO;
    [SerializeField] private GameObject tableTopWrapper;
    
    //Hand & Input
    [SerializeField] private XRNode leftInputSource;
    [SerializeField] private XRNode rightInputSource;
    [SerializeField] private XRRayInteractor leftInteractor;
    [SerializeField] private XRRayInteractor rightInteractor;
    private Vector2 leftInputAxis;
    private Vector2 rightInputAxis;
    private float leftGripAxis;
    private bool leftPrimaryButton;
    private bool leftSecondaryButton;
    private float leftTriggerAxis;
    private bool triggerPressed;

    // Update is called once per frame
    private void Update()
    {
        tableTop.Radius = Mathf.Clamp((float)tableTop.Radius, 1000.0f, 4500000.0f);
        UnityEngine.XR.InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(rightInputSource);
        UnityEngine.XR.InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(leftInputSource);
        leftDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out leftInputAxis);
        leftDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out leftPrimaryButton);
        leftDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out leftSecondaryButton);
        leftDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.grip, out leftGripAxis);
        leftDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.trigger, out leftTriggerAxis);

        rightDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out rightInputAxis);
        rightDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out triggerPressed);
        rightInteractor.TryGetCurrent3DRaycastHit(out RaycastHit rightHit);

        //move up and down
        if (leftInputAxis.y > 0.3f)
        {
            tableTopGO.transform.Translate(Vector3.up * Time.deltaTime * 5 * leftInputAxis.y);
        }
        else
        {
            tableTopGO.transform.Translate(Vector3.up * Time.deltaTime * 5 * leftInputAxis.y);
        }

        //move forward and backward
        if (leftInputAxis.y > 0.3f)
        {
            tableTopGO.transform.Translate(Vector3.forward * Time.deltaTime * 5 * leftInputAxis.x);
        }
        else
        {
            tableTopGO.transform.Translate(Vector3.forward * Time.deltaTime * 5 * leftInputAxis.x);
        }

        //rotate
        if (leftGripAxis != 0)
        {
            tableTopWrapper.transform.Rotate(Vector3.up * Time.deltaTime * 45, Space.Self);
        }
        else if (leftTriggerAxis != 0)
        {
            tableTopWrapper.transform.Rotate(Vector3.up * Time.deltaTime * -45, Space.Self);
        }

        if (leftSecondaryButton)
        {
            //Scale Up
            tableTopGO.transform.localScale += new Vector3(5 * Time.deltaTime, 5 * Time.deltaTime, 5 * Time.deltaTime);
        }
        else if (leftPrimaryButton)
        {
            //Scale Down
            tableTopGO.transform.localScale += new Vector3(-5 * Time.deltaTime, -5 * Time.deltaTime, -5 * Time.deltaTime);
        }

        if (rightHit.collider)
        {
            if (rightInputAxis.y != 0.0f)
            {
                var zoom = Mathf.Sign(rightInputAxis.y);
                ZoomMap(zoom);
            }

            if (triggerPressed && !isDragging)
            {
                StartPointDrag();
            }
            else if (triggerPressed && isDragging)
            {
                UpdatePointDrag();
            }
            else if (!triggerPressed && isDragging)
            {
                EndPointDrag();
            }
        }
    }

    private void StartPointDrag()
    {
        Vector3 dragCurrentPoint;
        var dragStartRay = new Ray(rightInteractor.rayOriginTransform.position, rightInteractor.rayEndPoint - rightInteractor.rayOriginTransform.position);
        tableTop.Raycast(dragStartRay, out dragCurrentPoint);
        isDragging = true;
        dragStartPoint = dragCurrentPoint;
        // Save the matrix to go from Local space to Universe space
        // As the origin location will be changing during drag, we keep the transform we had when the action started
        dragStartWorldMatrix = math.mul(math.inverse(hpRoot.WorldMatrix), tableTop.transform.localToWorldMatrix.ToDouble4x4());
    }

    private void UpdatePointDrag()
    {
        if (isDragging)
        {
            var updateRay = new Ray(rightInteractor.rayOriginTransform.position, rightInteractor.rayEndPoint - rightInteractor.rayOriginTransform.position);

            Vector3 dragCurrentPoint;
            tableTop.Raycast(updateRay, out dragCurrentPoint);

            var diff = dragStartPoint - dragCurrentPoint;
            var newExtentCenterCartesian = dragStartWorldMatrix.HomogeneousTransformPoint(diff.ToDouble3());
            var newExtentCenterGeographic = arcGISMapComponent.View.WorldToGeographic(new double3(newExtentCenterCartesian.x, newExtentCenterCartesian.y, newExtentCenterCartesian.z));

            tableTop.Center = newExtentCenterGeographic;
        }
    }

    private void EndPointDrag()
    {
        isDragging = false;
    }

    private void ZoomMap(float zoom)
    {
        if (zoom == 0)
        {
            return;
        }

        var speed = tableTop.Radius / radiusScalar;
        // More zoom means smaller extent
        tableTop.Radius -= zoom * speed;
    }
}