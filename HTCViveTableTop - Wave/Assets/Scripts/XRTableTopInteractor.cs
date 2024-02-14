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
    [Header("Table Top Components")]
    [SerializeField] private ArcGISMapComponent arcGISMapComponent;
    private Vector3 dragStartPoint = Vector3.zero;
    private double4x4 dragStartWorldMatrix;
    [SerializeField] private HPRoot hpRoot;
    private bool isDragging = false;
    [SerializeField] private ArcGISTabletopControllerComponent tableTop;
    [SerializeField] private float radiusScalar = 1.0f;
    [SerializeField] private GameObject tableTopGO;
    [SerializeField] private GameObject tableTopWrapper;
    
    [Header("Hand Input")]
    [SerializeField] private XRNode rightInputSource;
    [SerializeField] private XRRayInteractor rightInteractor;
    private Vector2 rightInputAxis;
    private bool triggerPressed;

    // Update is called once per frame
    private void Update()
    {
        tableTop.Width = Mathf.Clamp((float)tableTop.Width, 1000.0f, 4500000.0f);
        InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(rightInputSource);
        rightDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightInputAxis);
        rightDevice.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed);
        rightInteractor.TryGetCurrent3DRaycastHit(out RaycastHit rightHit);

        if (rightHit.collider.name.Contains("ArcGIS"))
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

        var speed = tableTop.Width / radiusScalar;
        // More zoom means smaller extent
        tableTop.Width -= zoom * speed;
    }

    public void ZoomInMap()
    {
        if (!isDragging)
        {
            var Speed = tableTop.Width / radiusScalar;
            // More zoom means smaller extent
            tableTop.Width += -1.0 * Speed;
        }
    }

    public void ZoomOutMap()
    {
        if (!isDragging)
        {
            var Speed = tableTop.Width / radiusScalar;
            // More zoom means smaller extent
            tableTop.Width -= -1.0 * Speed;
        }
    }

    public void RotateMapLeft()
    {
        if (!isDragging)
        {
            tableTopWrapper.transform.Rotate(Vector3.up * Time.deltaTime * 45, Space.Self);
        }
    }

    public void RotateMapRight()
    {
        if (!isDragging)
        {
            tableTopWrapper.transform.Rotate(Vector3.up * Time.deltaTime * -45, Space.Self);
        }
    }
}