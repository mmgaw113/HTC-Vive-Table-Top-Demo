using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Map;
using Esri.Unity;
using UnityEngine;


public class LoadData : MonoBehaviour
{
    [SerializeField] private ArcGISMapComponent arcGISMapComponent;
    string apikey;

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

        arcGISMapComponent.View.Map.Layers.Add(buildingLayer);
        arcGISMapComponent.View.Map.Layers.Add(newYorkBuildings);
    }
}
