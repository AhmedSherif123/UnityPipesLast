using Esri.HPFramework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Esri.ArcGISMapsSDK.Renderer.Renderables
{
	internal class RenderableProvider : IRenderableProvider
	{
		private readonly Dictionary<GameObject, IRenderable> gameObjectToRenderableMap = new();
		private readonly Dictionary<uint, IRenderable> activeRenderables = new();
		private readonly List<IRenderable> freeRenderables = new();

		private bool areMeshCollidersEnabled = false;
		private readonly GameObject unused = null;
		private readonly GameObject parent;

		// Map runtime layer IDs (the unpredictable numbers) -> sequential small indexes (0,1,2,...)
		private readonly Dictionary<uint, uint> runtimeToSequentialLayerMap = new();
		private uint nextSequentialLayerIndex = 0;

		public IReadOnlyDictionary<uint, IRenderable> Renderables => activeRenderables;

		public bool AreMeshCollidersEnabled
		{
			get => areMeshCollidersEnabled;
			set
			{
				if (areMeshCollidersEnabled != value)
				{
					areMeshCollidersEnabled = value;

					foreach (var activeRenderable in activeRenderables.Values)
					{
						activeRenderable.IsMeshColliderEnabled = value;
					}
				}
			}
		}

		public IEnumerable<IRenderable> TerrainMaskingMeshes =>
			Renderables.Values.Where(sc => sc.IsVisible && sc.MaskTerrain);

		public RenderableProvider(int initSize, GameObject parent, bool areMeshCollidersEnabled)
		{
			this.parent = parent;
			this.areMeshCollidersEnabled = areMeshCollidersEnabled;

			unused = new GameObject("UnusedPoolGOs")
			{
				hideFlags = HideFlags.DontSaveInEditor
			};
			unused.transform.SetParent(parent.transform, false);

			for (var i = 0; i < initSize; i++)
			{
				var renderable = new Renderable(CreateGameObject(i));
				renderable.RenderableGameObject.transform.SetParent(unused.transform, false);
				freeRenderables.Add(renderable);
			}
		}

		// 'layerId' parameter is the runtime layer id from Esri. We'll map it to a sequential id.
		public IRenderable CreateRenderable(uint id, uint layerId)
		{
			IRenderable renderable;

			if (freeRenderables.Count > 0)
			{
				renderable = freeRenderables[0];
				renderable.IsVisible = false;
				freeRenderables.RemoveAt(0);
			}
			else
			{
				renderable = new Renderable(CreateGameObject(activeRenderables.Count + freeRenderables.Count));
			}

			// Map runtime layerId -> sequential index (0,1,2,...)
			if (!runtimeToSequentialLayerMap.TryGetValue(layerId, out var sequentialLayerId))
			{
				sequentialLayerId = nextSequentialLayerIndex;
				runtimeToSequentialLayerMap[layerId] = sequentialLayerId;
				nextSequentialLayerIndex++;
			}

			renderable.RenderableGameObject.transform.SetParent(parent.transform, false);
			renderable.IsMeshColliderEnabled = areMeshCollidersEnabled;
			renderable.Name = $"Layer_{sequentialLayerId}_Renderable_{id}";
			renderable.LayerId = sequentialLayerId;

			// ✅ Set Unity Layer
			if (layerId > 0)
			{
				// Set to ARView layer (index 3)
				renderable.RenderableGameObject.layer = 3;
			}
			else
			{
				// Default layer
				renderable.RenderableGameObject.layer = 0;
			}

			activeRenderables.Add(id, renderable);
			gameObjectToRenderableMap.Add(renderable.RenderableGameObject, renderable);

			return renderable;
		}

		public void DestroyRenderable(uint id)
		{
			if (!activeRenderables.TryGetValue(id, out var activeRenderable))
				return;

			activeRenderable.RenderableGameObject.transform.SetParent(unused.transform, false);
			activeRenderable.IsVisible = false;
			activeRenderable.Mesh = null;

			gameObjectToRenderableMap.Remove(activeRenderable.RenderableGameObject);
			activeRenderables.Remove(id);
			freeRenderables.Add(activeRenderable);
		}

		public void Release()
		{
			foreach (var activeRenderable in activeRenderables.Values)
			{
				activeRenderable.Destroy();
			}

			foreach (var freeRenderable in freeRenderables)
			{
				freeRenderable.Destroy();
			}

			activeRenderables.Clear();
			freeRenderables.Clear();

			if (unused)
			{
				if (Application.isEditor)
				{
					Object.DestroyImmediate(unused);
				}
				else
				{
					Object.Destroy(unused);
				}
			}

			// clear mapping and reset sequence
			runtimeToSequentialLayerMap.Clear();
			nextSequentialLayerIndex = 0;
		}

		private static GameObject CreateGameObject(int id)
		{
			var gameObject = new GameObject("ArcGISGameObject" + id)
			{
				hideFlags = HideFlags.None
			};
			gameObject.SetActive(true);

			var renderer = gameObject.AddComponent<MeshRenderer>();
			renderer.shadowCastingMode = ShadowCastingMode.TwoSided;
			renderer.enabled = true;

			gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<HPTransform>();
			gameObject.AddComponent<MeshCollider>();
			gameObject.GetComponent<MeshCollider>().enabled = true;

			return gameObject;
		}

		public IRenderable GetRenderableFrom(GameObject gameObject)
		{
			gameObjectToRenderableMap.TryGetValue(gameObject, out var renderable);
			return renderable;
		}
	}
}
