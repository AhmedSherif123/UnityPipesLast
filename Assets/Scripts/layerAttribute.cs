// ======================================================================================================================================================== 

// // Copyright 2023 Esri.
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
// //

// using Esri.GameEngine.Layers;
// using Esri.Unity;
// using Unity.Collections.LowLevel.Unsafe;
// using UnityEngine;
// using UnityEngine.Rendering;
// using System;
// using UnityEngine.UI;

// [ExecuteAlways]
// public class AttributesComponent : MonoBehaviour
// {
//     public enum AttributeType
//     {
//         None,
//         Object_ID,
//     };

//     [SerializeField] private AttributeType layerAttribute = AttributeType.None;

//     private AttributeType lastLayerAttribute;
//     private Esri.GameEngine.Attributes.ArcGISAttributeProcessor attributeProcessor;
//     private SampleAPIMapCreator sampleMapCreator;

//     [Header("UI")]
//     [SerializeField] private Toggle ObjectID;
//     [SerializeField] private Toggle none;

//     [Header("Shader Graph Material")]
//     [SerializeField] private Material PipesColor;

//     private int objectIDToggleCount = 0;

//     private void Awake()
//     {
//         sampleMapCreator = GetComponent<SampleAPIMapCreator>();

//         if (!sampleMapCreator)
//         {
//             Debug.LogError("[AttributesComponent] SampleAPIMapCreator not found on GameObject.");
//             return;
//         }

//         sampleMapCreator.OnSetLayerAttributes += Setup3DAttributes;
//     }

//     private void Start()
//     {
//         // Initialize toggles without triggering listeners
//         none.SetIsOnWithoutNotify(layerAttribute == AttributeType.None);
//         ObjectID.SetIsOnWithoutNotify(layerAttribute == AttributeType.Object_ID);

//         // ObjectID toggle
//         ObjectID.onValueChanged.AddListener(active =>
//         {
//             if (!active) return;
//             layerAttribute = AttributeType.Object_ID;

//             ObjectID.SetIsOnWithoutNotify(true);
//             none.SetIsOnWithoutNotify(false);

//             if (objectIDToggleCount == 0 &&
//                 sampleMapCreator != null &&
//                 sampleMapCreator.arcGIS3Lists != null &&
//                 sampleMapCreator.arcGIS3Lists.Count > 0)
//             {
//                 sampleMapCreator.RemoveArcGISLayer(sampleMapCreator.arcGIS3Lists[0]);
//                 Debug.Log("[AttributesComponent] Layer removed once (Object_ID).");
//                 objectIDToggleCount++;
//             }
//         });

//         // None toggle
//         none.onValueChanged.AddListener(active =>
//         {
//             if (!active) return;
//             layerAttribute = AttributeType.None;

//             none.SetIsOnWithoutNotify(true);
//             ObjectID.SetIsOnWithoutNotify(false);

//             if (sampleMapCreator != null &&
//                 sampleMapCreator.arcGIS3Lists != null &&
//                 sampleMapCreator.arcGIS3Lists.Count > 0)
//             {
//                 sampleMapCreator.RemoveArcGISLayer(sampleMapCreator.arcGIS3Lists[0]);
//                 Debug.Log("[AttributesComponent] Layer removed (None).");
//             }
//         });
//     }

//     private void Update()
//     {
//         if (sampleMapCreator == null) return;

//         // Rebuild map if attribute type changes
//         if (layerAttribute != lastLayerAttribute)
//         {
//             sampleMapCreator.CreateArcGISMap();
//             lastLayerAttribute = layerAttribute;
//         }

//         // Keep toggles visually consistent
//         none.SetIsOnWithoutNotify(layerAttribute == AttributeType.None);
//         ObjectID.SetIsOnWithoutNotify(layerAttribute == AttributeType.Object_ID);
//     }

//     private void Setup3DAttributes(ArcGIS3DObjectSceneLayer buildingLayer)
//     {
//         if (buildingLayer == null)
//         {
//             Debug.LogError("[Setup3DAttributes] Building layer is null.");
//             return;
//         }

//         if (layerAttribute == AttributeType.Object_ID)
//         {
//             Setup3DAttributesFloatAndIntegerType(buildingLayer);
//         }
//     }

//     private void Setup3DAttributesFloatAndIntegerType(ArcGIS3DObjectSceneLayer layer)
//     {
//         if (layer == null)
//         {
//             Debug.LogError("[Setup3DAttributesFloatAndIntegerType] Layer is null.");
//             return;
//         }

//         // 🔸 ArcGIS attribute name
//         const string arcgisFieldName = "OBJECTID_1";

//         // 🔸 Shader Graph float property (must be a Float property in Shader Graph!)
//         const string shaderPropertyName = "_OBJECTID_1";

//         Debug.Log($"[Setup] ArcGIS Field: {arcgisFieldName}, Shader Property: {shaderPropertyName}");

//         // 1️⃣ Choose attribute to visualize
//         var attributeBuilder = ArcGISImmutableArray<string>.CreateBuilder();
//         attributeBuilder.Add(arcgisFieldName);
//         layer.SetAttributesToVisualize(attributeBuilder.MoveToArray());
//         Debug.Log("[Setup] Attribute to visualize set.");

//         // 2️⃣ Assign material
//         Material matInstance = new Material(PipesColor);
//         layer.MaterialReference = matInstance;

//         Debug.Log($"[Setup] Assigned material: {matInstance.name}");

//         bool hasProperty = matInstance.HasProperty(shaderPropertyName);
//         Debug.Log($"[Setup] Material has '{shaderPropertyName}': {hasProperty}");
//         if (!hasProperty)
//         {
//             Debug.LogError($"[Setup] Shader Graph must have a FLOAT property with reference '{shaderPropertyName}'.");
//         }

//         // 3️⃣ Processor: Fill per-feature render buffer
//         attributeProcessor = new Esri.GameEngine.Attributes.ArcGISAttributeProcessor();
//         attributeProcessor.ProcessEvent += (inputAttrs, renderAttrs) =>
//         {
//             if (inputAttrs.Size == 0 || renderAttrs.Size == 0)
//             {
//                 Debug.LogWarning("[Processor] No input or render attributes.");
//                 return;
//             }

//             var renderAttr = renderAttrs.At(0);
//             var renderData = renderAttr.Data.Reinterpret<float>(sizeof(byte));

//             var objectIdAttr = inputAttrs.At(0);
//             unsafe
//             {
//                 var ptr = NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr(objectIdAttr.Data);
//                 int* data = (int*)ptr;
//                 int attrSize = objectIdAttr.Data.Length / sizeof(int);
//                 int count = Mathf.Min(attrSize, renderData.Length);

//                 Debug.Log($"[Processor] Writing {count} values from {arcgisFieldName} to {shaderPropertyName}");

//                 for (int i = 0; i < count; i++)
//                 {
//                     renderData[i] = data[i];
// #if UNITY_EDITOR
//                     Debug.Log($"[Processor] {arcgisFieldName}[{i}] = {data[i]}");
// #endif
//                 }
//             }
//         };

//         // 4️⃣ Bind ArcGIS field → Shader property
//         var renderAttrDesc = ArcGISImmutableArray<Esri.GameEngine.Attributes.ArcGISVisualizationAttributeDescription>.CreateBuilder();
//         renderAttrDesc.Add(new Esri.GameEngine.Attributes.ArcGISVisualizationAttributeDescription(
//             shaderPropertyName,
//             Esri.GameEngine.Attributes.ArcGISVisualizationAttributeType.Float32
//         ));

//         var attrNames = ArcGISImmutableArray<string>.CreateBuilder();
//         attrNames.Add(arcgisFieldName);

//         Debug.Log("[Setup] Binding ArcGIS field to Shader property...");
//         layer.SetAttributesToVisualize(attrNames.MoveToArray(), renderAttrDesc.MoveToArray(), attributeProcessor);
//         Debug.Log("[Setup] Binding complete.");
//     }

//     private string DetectRenderPipeline()
//     {
//         if (GraphicsSettings.renderPipelineAsset != null)
//         {
//             string type = GraphicsSettings.renderPipelineAsset.GetType().ToString();
//             if (type.Contains("UniversalRenderPipelineAsset")) return "URP";
//             if (type.Contains("HDRenderPipelineAsset")) return "HDRP";
//         }
//         return "Legacy";
//     }
// }
