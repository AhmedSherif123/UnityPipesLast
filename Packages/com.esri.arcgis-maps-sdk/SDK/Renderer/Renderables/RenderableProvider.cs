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

        // Keep track of per-layer parents
        private readonly Dictionary<uint, GameObject> layerParents = new();
        private readonly Dictionary<uint, string> layerNames = new();
        private readonly Dictionary<uint, int> layerIndices = new();

        // ✅ Map original ArcGIS layer IDs to sequential IDs
        private readonly Dictionary<uint, uint> sequentialLayerIdMap = new();
        private uint nextSequentialLayerId = 1;

        public IReadOnlyDictionary<uint, IRenderable> Renderables => activeRenderables;

        public bool AreMeshCollidersEnabled
        {
            get => areMeshCollidersEnabled;
            set
            {
                if (areMeshCollidersEnabled != value)
                {
                    areMeshCollidersEnabled = value;
                    foreach (var activeRenderable in activeRenderables)
                    {
                        activeRenderable.Value.IsMeshColliderEnabled = value;
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

            // Pool container
            unused = new GameObject("UnusedPoolGOs")
            {
                hideFlags = HideFlags.None // show in hierarchy
            };
            unused.transform.SetParent(parent.transform, false);

            for (var i = 0; i < initSize; i++)
            {
                var renderable = new Renderable(CreateGameObject(i));
                renderable.RenderableGameObject.transform.SetParent(unused.transform, false);
                freeRenderables.Add(renderable);
            }
        }

        /// <summary>
        /// Register metadata for a layer (called when a new ArcGISLayer is added).
        /// Assign sequential internal layer IDs.
        /// </summary>
        public void RegisterLayer(uint originalLayerId, string layerName, int layerIndex)
        {
            if (!sequentialLayerIdMap.ContainsKey(originalLayerId))
            {
                sequentialLayerIdMap[originalLayerId] = nextSequentialLayerId++;
            }

            if (!layerNames.ContainsKey(originalLayerId))
            {
                layerNames[originalLayerId] = layerName;
                layerIndices[originalLayerId] = layerIndex;
            }
        }

        public IRenderable CreateRenderable(uint id, uint originalLayerId)
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
                renderable = new Renderable(
                    CreateGameObject(
                        activeRenderables.Count + freeRenderables.Count,
                        $"Layer_{originalLayerId}_Renderable_{id}"
                    )
                );
            }

            // ✅ Translate original ArcGIS layer ID to sequential ID
            if (!sequentialLayerIdMap.TryGetValue(originalLayerId, out uint sequentialId))
            {
                sequentialId = nextSequentialLayerId++;
                sequentialLayerIdMap[originalLayerId] = sequentialId;
            }

            // Figure out a safe name for the layer
            string safeLayerName = $"Layer_{sequentialId}";
            if (layerNames.TryGetValue(originalLayerId, out var lname) && !string.IsNullOrEmpty(lname))
            {
                safeLayerName = lname;
            }
            else if (layerIndices.TryGetValue(originalLayerId, out var lindex))
            {
                safeLayerName = $"Layer_{lindex}";
            }

            // Make sure this layer has a parent node in hierarchy
            if (!layerParents.TryGetValue(sequentialId, out var layerParent))
            {
                layerParent = new GameObject(safeLayerName)
                {
                    hideFlags = HideFlags.None
                };
                layerParent.transform.SetParent(parent.transform, false);
                layerParents[sequentialId] = layerParent;
            }

            renderable.RenderableGameObject.transform.SetParent(layerParent.transform, false);
            renderable.IsMeshColliderEnabled = areMeshCollidersEnabled;

            // Assign a descriptive name
            renderable.Name = $"{safeLayerName}_Renderable_{id}";
            renderable.LayerId = sequentialId;

            // Basemap = layerId 0, everything else goes to Unity layer 3
            if (sequentialId > 1)
            {
                renderable.RenderableGameObject.layer = 3;
            }

            activeRenderables.Add(id, renderable);
            gameObjectToRenderableMap.Add(renderable.RenderableGameObject, renderable);

            return renderable;
        }

        public void DestroyRenderable(uint id)
        {
            var activeRenderable = activeRenderables[id];

            activeRenderable.RenderableGameObject.transform.SetParent(unused.transform, false);
            activeRenderable.IsVisible = false;
            activeRenderable.Mesh = null;

            gameObjectToRenderableMap.Remove(activeRenderable.RenderableGameObject);
            activeRenderables.Remove(id);
            freeRenderables.Add(activeRenderable);
        }

        public void Release()
        {
            foreach (var activeRenderable in activeRenderables)
            {
                activeRenderable.Value.Destroy();
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

            foreach (var kv in layerParents)
            {
                if (kv.Value)
                {
                    if (Application.isEditor)
                        Object.DestroyImmediate(kv.Value);
                    else
                        Object.Destroy(kv.Value);
                }
            }

            layerParents.Clear();
            sequentialLayerIdMap.Clear();
            nextSequentialLayerId = 1;
        }

        private static GameObject CreateGameObject(int id, string customName = null)
        {
            string goName = string.IsNullOrEmpty(customName) ? $"ArcGISGameObject_{id}" : customName;

            var gameObject = new GameObject(goName)
            {
                hideFlags = HideFlags.None // show in hierarchy
            };

            gameObject.SetActive(false);

            var renderer = gameObject.AddComponent<MeshRenderer>();
            renderer.shadowCastingMode = ShadowCastingMode.TwoSided;
            renderer.enabled = true;

            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<HPTransform>();
            gameObject.AddComponent<MeshCollider>();

            return gameObject;
        }

        public IRenderable GetRenderableFrom(GameObject gameObject)
        {
            gameObjectToRenderableMap.TryGetValue(gameObject, out var renderable);
            return renderable;
        }
    }
}
