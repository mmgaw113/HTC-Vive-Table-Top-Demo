using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Map;
using Esri.Unity;
using UnityEngine;


public class LoadData : MonoBehaviour
{
    private ArcGISMapComponent arcGISMapComponent;

    private void Awake()
    {
        arcGISMapComponent = GetComponent<ArcGISMapComponent>();
    }

    void Start()
    {
        var buildingLayer = new Esri.GameEngine.Layers.ArcGISIntegratedMeshLayer(
            Application.persistentDataPath + "/Test_Boston_Subset.slpk",
            "Building Layer", 1.0f, true, "");
        var newYorkBuildings = new Esri.GameEngine.Layers.ArcGIS3DObjectSceneLayer(
            "https://tiles.arcgis.com/tiles/P3ePLMYs2RVChkJx/arcgis/rest/services/Buildings_NewYork_17/SceneServer",
            "Building Layer", 1.0f, true, "");

        if(buildingLayer != null)
        {
            arcGISMapComponent.View.Map.Layers.Add(buildingLayer);
        }

        if(newYorkBuildings != null)
        {
            arcGISMapComponent.View.Map.Layers.Add(newYorkBuildings);
        }
    }
}
