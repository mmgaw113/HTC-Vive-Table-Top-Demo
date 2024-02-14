using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wave.Native;
/*using Wave.OpenXR.CompositionLayer;
using Wave.OpenXR.Toolkit.CompositionLayer.Passthrough;*/
//using Wave.OpenXR.Toolkit.Samples;

namespace Wave.OpenXR.Toolkit.CompositionLayer.Samples.Passthrough
{
    public class EnablePassthrough : MonoBehaviour
    {
/*        private int activePassthroughID = 0;
        private LayerType currentActiveLayerType = LayerType.Underlay;*/


        private void Start()
        {
            Interop.WVR_ShowPassthroughUnderlay(true);
            Interop.WVR_SetPassthroughImageFocus(WVR_PassthroughImageFocus.View);
            Interop.WVR_SetPassthroughImageQuality(WVR_PassthroughImageQuality.QualityMode);
            /*            if (activePassthroughID == 0)
                        {
                            StartPassthrough();
                        }

                        SetPassthroughToUnderlay();*/
        }
        private void Update()
        {

        }

/*        public void SetPassthroughToUnderlay()
        {
            if (activePassthroughID != 0)
            {
                CompositionLayerPassthroughAPI.SetPassthroughLayerType(activePassthroughID, LayerType.Underlay);
                currentActiveLayerType = LayerType.Underlay;
            }
        }

        void StartPassthrough()
        {
            activePassthroughID = CompositionLayerPassthroughAPI.CreatePlanarPassthrough(currentActiveLayerType, OnDestroyPassthroughFeatureSession);
        }

        void OnDestroyPassthroughFeatureSession(int passthroughID)
        {
            CompositionLayerPassthroughAPI.DestroyPassthrough(passthroughID);
            activePassthroughID = 0;
        }*/
    }
}