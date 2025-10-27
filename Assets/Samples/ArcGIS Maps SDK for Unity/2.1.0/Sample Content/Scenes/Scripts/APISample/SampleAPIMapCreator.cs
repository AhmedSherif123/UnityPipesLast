// Copyright 2021 Esri.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// You may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//

using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using Esri.GameEngine.Layers;
using Esri.Unity;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SampleAPIMapCreator : MonoBehaviour
{
	[SerializeField] private ArcGISMapComponent mapComponent;
	[SerializeField] private ArcGISCameraComponent cameraComponent;
	[SerializeField] private string APIKey = "";

	[Header("Scene Layers")]
	public List<ArcGIS3DObjectSceneLayer> arcGIS3Lists = new();

	private ArcGISPoint geographicCoordinates = new(29.97237, 30.94493, 3000, ArcGISSpatialReference.WGS84());

	public delegate void SetLayerAttributesEventHandler(ArcGIS3DObjectSceneLayer layer);
	public event SetLayerAttributesEventHandler OnSetLayerAttributes;

	// -------------------------------------------------------
	private void Start()
	{
		// ✅ If mapComponent is assigned, always initialize the map
		if (mapComponent != null)
		{
			CreateArcGISMap();
		}
		else
		{
			Debug.LogError("[SampleAPIMapCreator] ArcGISMapComponent is not assigned.");
		}
	}

	// -------------------------------------------------------
	public void CreateArcGISMap()
	{
		if (mapComponent == null || mapComponent.Map == null)
		{
			Debug.LogError("[SampleAPIMapCreator] Cannot create map. MapComponent or Map is null.");
			return;
		}

		var map = mapComponent.Map;

		// ✅ Optional: clear old layers if you are recreating the map
		map.Layers.RemoveAll();
		arcGIS3Lists.Clear();

		// ✅ Add new layers
		var pipesLayer = new ArcGIS3DObjectSceneLayer(
			"https://services3.arcgis.com/YhesYCzrdc1fSZZx/arcgis/rest/services/Pipes/SceneServer",
			"Pipes Layer",
			1.0f,
			true,
			string.Empty
		);
		if (map.Basemap.LoadStatus == Esri.GameEngine.ArcGISLoadStatus.Loaded)
		{
			map.Layers.Add(pipesLayer);
			arcGIS3Lists.Add(pipesLayer);

			// Fire the attribute event for the newly added layer
			OnSetLayerAttributes?.Invoke(pipesLayer);
		}
		// ✅ Assign the map to the view
		mapComponent.View.Map = map;

#if UNITY_EDITOR
		if (!Application.isPlaying && SceneView.lastActiveSceneView != null)
		{
			SceneView.lastActiveSceneView.pivot = cameraComponent.transform.position;
			SceneView.lastActiveSceneView.rotation = cameraComponent.transform.rotation;
		}
#endif
	}

	// -------------------------------------------------------
	public void RemoveArcGISLayer(ArcGIS3DObjectSceneLayer layer)
	{
		if (layer == null)
		{
			Debug.LogError("[SampleAPIMapCreator] Cannot remove null layer.");
			return;
		}

		if (mapComponent.Map.Layers.Contains(layer))
		{
			//mapComponent.Map.Layers.Remove(mapComponent.Map.Layers.IndexOf(layer));
			//arcGIS3Lists.Remove(layer);
			Debug.LogWarning($"[SampleAPIMapCreator] Layer '{layer.Name}' has been removed.");
		}
		else
		{
			Debug.LogError($"[SampleAPIMapCreator] Layer '{layer.Name}' not found in map.");
		}
	}
}
