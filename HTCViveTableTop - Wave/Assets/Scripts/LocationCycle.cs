using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Samples.Components;
using Esri.GameEngine.Geometry;
using Esri.GameEngine.Layers;
using Esri.GameEngine.Layers.BuildingScene;
using Esri.Unity;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationCycle : MonoBehaviour
{
    public enum Locations
    {
        Girona,
        MountEverest,
        NewYorkCity,
        SanFransisco
    }

    private ArcGISTabletopControllerComponent tableTopController;
    private ArcGISMapComponent arcGISMapComponent;
    [SerializeField] private Locations locations;
    private Locations nextLocation;
    private Locations previousLocation;

    [SerializeField] private Image locationImage;
    [SerializeField] private Sprite girona;
    [SerializeField] private Sprite mtEverest;
    [SerializeField] private Sprite NYC;
    [SerializeField] private Sprite sanFran;

    private ArcGISIntegratedMeshLayer gironaLayer;
    private ArcGIS3DObjectSceneLayer newYorkBuildings;
    private ArcGIS3DObjectSceneLayer sfBuildings;

    private void Awake()
    {
        tableTopController = GetComponent<ArcGISTabletopControllerComponent>();
        arcGISMapComponent = GetComponentInChildren<ArcGISMapComponent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        newYorkBuildings = new ArcGIS3DObjectSceneLayer(
            "https://tiles.arcgis.com/tiles/P3ePLMYs2RVChkJx/arcgis/rest/services/Buildings_NewYork_17/SceneServer",
            "NewYork", 1.0f, false, "");
        sfBuildings = new ArcGIS3DObjectSceneLayer(
            "https://tiles.arcgis.com/tiles/z2tnIkrLQ2BRzr6P/arcgis/rest/services/SanFrancisco_Bldgs/SceneServer",
            "SanFran", 1.0f, false, "");
        gironaLayer = new ArcGISIntegratedMeshLayer(
            "https://tiles.arcgis.com/tiles/z2tnIkrLQ2BRzr6P/arcgis/rest/services/Girona_Spain/SceneServer",
            "Girona", 1.0f, false, "");

        if (gironaLayer != null)
        {
            arcGISMapComponent.Map.Layers.Add(gironaLayer);
        }

        if (newYorkBuildings != null)
        {
            arcGISMapComponent.Map.Layers.Add(newYorkBuildings);
        }

        if (sfBuildings != null)
        {
            arcGISMapComponent.Map.Layers.Add(sfBuildings);
        }

        SetLocation();
    }

    public void SetGirona()
    {
        tableTopController.Center = new ArcGISPoint(314076.81132414174, 5157894.163259039, 0.0f, ArcGISSpatialReference.WebMercator());
        tableTopController.ElevationOffset = 0.0f;
        previousLocation = Locations.SanFransisco;
        nextLocation = Locations.MountEverest;
        locationImage.sprite = girona;
        gironaLayer.IsVisible = true;
        newYorkBuildings.IsVisible = false;
        sfBuildings.IsVisible = false;
    }

    public void SetEverest()
    {
        tableTopController.Center = new ArcGISPoint(9676446.737205295, 3247473.554732518, 0.0f, ArcGISSpatialReference.WebMercator());
        tableTopController.ElevationOffset = -3000.0f;
        previousLocation = Locations.Girona;
        nextLocation = Locations.NewYorkCity;
        locationImage.sprite = mtEverest;
        gironaLayer.IsVisible = false;
        newYorkBuildings.IsVisible = false;
        sfBuildings.IsVisible = false;
    }

    public void SetNYC()
    {
        tableTopController.Center = new ArcGISPoint(-8238310.235646995, 4970071.5791424215, 0.0f, ArcGISSpatialReference.WebMercator());
        tableTopController.ElevationOffset = 0.0f;
        previousLocation = Locations.MountEverest;
        nextLocation = Locations.SanFransisco;
        locationImage.sprite = NYC;
        gironaLayer.IsVisible = false;
        newYorkBuildings.IsVisible = true;
        sfBuildings.IsVisible = false;
    }

    public void SetSanFran()
    {
        tableTopController.Center = new ArcGISPoint(-13627665.271218061, 4547675.354340553, 0.0f, ArcGISSpatialReference.WebMercator());
        tableTopController.ElevationOffset = 0.0f;
        previousLocation = Locations.NewYorkCity;
        nextLocation = Locations.Girona;
        locationImage.sprite = sanFran;
        gironaLayer.IsVisible = false;
        newYorkBuildings.IsVisible = false;
        sfBuildings.IsVisible = true;
    }

    public void SetLocation()
    {
        if (locations == Locations.Girona)
        {
            SetGirona();
        }
        else if (locations == Locations.MountEverest)
        {
            SetEverest();
        }
        else if (locations == Locations.NewYorkCity)
        {
            SetNYC();
        }
        else if (locations == Locations.SanFransisco)
        {
            SetSanFran();
        }
    }

    public void NextLocation()
    {
        locations = nextLocation;
        SetLocation();
    }

    public void PreviousLocation()
    {
        locations = previousLocation;
        SetLocation();
    }
}
