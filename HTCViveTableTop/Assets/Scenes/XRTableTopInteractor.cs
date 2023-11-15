using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit;

using Esri.HPFramework;
using Unity.Mathematics;

namespace Esri.ArcGISMapsSDK.Samples.Components
{
    public class XRTableTopInteractor : MonoBehaviour
    {
        [SerializeField] private XRRayInteractor leftInteractor;
        [SerializeField] private XRRayInteractor rightInteractor;
        [SerializeField] private ArcGISTabletopControllerComponent tableTop;
        [SerializeField] private ArcGISMapComponent arcGISMapComponent;
        [Min(1)] [SerializeField] private float radiusScalar = 1.0f;

        [SerializeField] private XRNode rightInputSource;
        private Vector2 rightInputAxis;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UnityEngine.XR.InputDevice rightDevice = InputDevices.GetDeviceAtXRNode(rightInputSource);
            rightDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out rightInputAxis);

            rightInteractor.TryGetCurrent3DRaycastHit(out RaycastHit rightHit);
            if (rightHit.collider)
            {
                if (rightInputAxis.y > Mathf.Abs(0.3f))
                {
                    Debug.Log("Test");
                    tableTop.Radius += tableTop.Radius * rightInputAxis.y * radiusScalar;
                }
            }
        }

        private bool OnMapHit()
        {
/*            if (leftInteractor != null)
            {
                leftInteractor.TryGetCurrent3DRaycastHit(out RaycastHit leftHit);

                if (leftHit.collider)
                {
                    if (leftHit.collider.name.ToLower().Contains("arcgis"))
                    {
                        return true;
                    }
                }
            }*/

            if (rightInteractor != null)
            {
                rightInteractor.TryGetCurrent3DRaycastHit(out RaycastHit rightHit);

                if (rightHit.collider)
                {
                    if (rightHit.collider.name.ToLower().Contains("arcgis"))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

}