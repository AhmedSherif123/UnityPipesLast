// Copyright 2023 Esri.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//

// ArcGISMapsSDK

using Esri.GameEngine.Layers;
using Esri.Unity;

// Unity

using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Rendering;

// System

using System;
using System.Runtime.InteropServices;
using UnityEngine.UI;

// This sample code demonstrates how to find and manipulate layer attributes
// You can use layer attributes to target unique properties of your data and customize the rendering of that data
// In this example, we demonstrate how to find buildings in New York City that match specific criteria and recolor them for visual acuity and focus
// The initial setup for the map is found in ArcGISMapsSDK/Samples/Scripts/APISample/SampleAPIMapCreator.cs
[ExecuteAlways]
public class AttributesComponent : MonoBehaviour
{
	// We define this enum and property to demonstrate the two Attribute mode types
	public enum AttributeType
	{
		None,
		Object_ID,
		
	};

	[SerializeField]
	private AttributeType layerAttribute = AttributeType.None;

	private AttributeType lastLayerAttribute;
	private Esri.GameEngine.Attributes.ArcGISAttributeProcessor attributeProcessor;
	private SampleAPIMapCreator sampleMapCreator;
	
	//[SerializeField] private Toggle building;
	[SerializeField] private Toggle ObjectID;
    [SerializeField] private Toggle none;
    [SerializeField] private Material PipesColor;
	
	private void Awake()
	{
		sampleMapCreator = GetComponent<SampleAPIMapCreator>();

		if (!sampleMapCreator)
		{
			Debug.LogError("SampleAPIMapCreator not found");
			return;
		}

		sampleMapCreator.OnSetLayerAttributes += new SampleAPIMapCreator.SetLayerAttributesEventHandler(Setup3DAttributes);
	}

	private void Start()
	{
		if (layerAttribute == AttributeType.None)
		{
			none.isOn = true;
		}
		else if (layerAttribute == AttributeType.Object_ID)
		{
			ObjectID.isOn = true;
		}
		// else
		// {
		// 	building.isOn = true;
		// }
		
		// building.onValueChanged.AddListener(delegate(bool active)
		// {
		// 	if (active)
		// 	{
		// 		layerAttribute = AttributeType.BuildingName;
		// 		construction.isOn = false;
		// 		none.isOn = false;
		// 	}
		// });
		
		ObjectID.onValueChanged.AddListener(delegate(bool active)
		{
			if (active)
			{
				layerAttribute = AttributeType.Object_ID;
				ObjectID.isOn = false;
				none.isOn = false;
			}
		});
		
		none.onValueChanged.AddListener(delegate(bool active)
		{
			if (active)
			{
				layerAttribute = AttributeType.None;
				ObjectID.isOn = false;
			//	building.isOn = false;
			}
		});
	}

	private void Update()
	{
		if (layerAttribute != lastLayerAttribute)
		{
			 sampleMapCreator.CreateArcGISMap();
			lastLayerAttribute = layerAttribute;
		}
		
		if (layerAttribute == AttributeType.None)
		{
            none.isOn = true;
            
			ObjectID.isOn = false;
		}
		else if (layerAttribute == AttributeType.Object_ID)
        {
          
            none.isOn = false;
            
			ObjectID.isOn = true;
		}
		// else
		// {
		// 	building.isOn = true;
		// }
	}

	// We initialize the attribute processor with this method
	private void Setup3DAttributes(ArcGIS3DObjectSceneLayer buildingLayer)
	{
		if (buildingLayer == null)
		{
			return;
		}

		if (layerAttribute == AttributeType.Object_ID)
		{
			Setup3DAttributesFloatAndIntegerType(buildingLayer);
		}
		// else if (layerAttribute == AttributeType.BuildingName)
		// {
		// 	Setup3DAttributesOtherType(buildingLayer);
		// }
	}

    // This function is an example of how to use attributes WITHOUT the attribute processor
    private void Setup3DAttributesFloatAndIntegerType(Esri.GameEngine.Layers.ArcGIS3DObjectSceneLayer layer)
{
	// Set attributes to visualize
	var layerAttributes = ArcGISImmutableArray<string>.CreateBuilder();
	layerAttributes.Add("OBJECTID_1");
	layer.SetAttributesToVisualize(layerAttributes.MoveToArray());

	// Assign material
	layer.MaterialReference = PipesColor;

	// ✅ Debug OBJECTID_1 values
	attributeProcessor = new Esri.GameEngine.Attributes.ArcGISAttributeProcessor();
	attributeProcessor.ProcessEvent += (inputAttributes, renderAttributes) =>
	{
		if (inputAttributes.Size > 0)
		{
			var objectIdAttr = inputAttributes.At(0);
			unsafe
			{
				var buffer = objectIdAttr.Data;
				var ptr = NativeArrayUnsafeUtility.GetUnsafeReadOnlyPtr(buffer);
				int* data = (int*)ptr;
                int attrSize = objectIdAttr.Data.Length / sizeof(int);

				for (int i = 0; i < attrSize; i++)
				{
					Debug.Log($"[OBJECTID_1] Element {i}: {data[i]}");
				}
			}
		}
	};

	var attrBuilder = ArcGISImmutableArray<string>.CreateBuilder();
	attrBuilder.Add("OBJECTID_1");

	var renderAttrDesc = ArcGISImmutableArray<Esri.GameEngine.Attributes.ArcGISVisualizationAttributeDescription>.CreateBuilder();
	renderAttrDesc.Add(new Esri.GameEngine.Attributes.ArcGISVisualizationAttributeDescription(
		"OBJECTID_1", Esri.GameEngine.Attributes.ArcGISVisualizationAttributeType.Int32));

	layer.SetAttributesToVisualize(attrBuilder.MoveToArray(), renderAttrDesc.MoveToArray(), attributeProcessor);
}



    /*	// This function is an example of how to use attributes WITH the attribute processor
        private void Setup3DAttributesOtherType(Esri.GameEngine.Layers.ArcGIS3DObjectSceneLayer layer)
        {
            // We want to set up an array with the attributes we want to forward to the material
            // Because NAME is of type esriFieldTypeString/AttributeType.string, we will need to configure the AttributeProcessor to pass meaningful values to the material
            var layerAttributes = ArcGISImmutableArray<String>.CreateBuilder();
            layerAttributes.Add("NAME");

            // The attribute description is the buffer that is output to the material
            // Visualize the label "NAME" in the layer attributes as an input to the attribute processor
            // "IsBuildingOfInterest" describes how we choose to convert "NAME" into a usable type in the material
            // We give the material values we can use to render the models in a way we see fit
            // In this case, we are using "IsBuildingOfInterest" to output either a 0 or a 1 depending on if the buildings "NAME" is a name of interest
            var renderAttributeDescriptions = ArcGISImmutableArray<Esri.GameEngine.Attributes.ArcGISVisualizationAttributeDescription>.CreateBuilder();
            renderAttributeDescriptions.Add(new Esri.GameEngine.Attributes.ArcGISVisualizationAttributeDescription("IsBuildingOfInterest", Esri.GameEngine.Attributes.ArcGISVisualizationAttributeType.Float32));

            // The attribute processor does the work on the CPU of converting the attribute into a value that can be used with the material
            // Integers and floats can be processed the same way as other types, although it is not normally necessary
            attributeProcessor = new Esri.GameEngine.Attributes.ArcGISAttributeProcessor();

            attributeProcessor.ProcessEvent += (ArcGISImmutableArray<Esri.GameEngine.Attributes.ArcGISAttribute> layerNodeAttributes, ArcGISImmutableArray<Esri.GameEngine.Attributes.ArcGISVisualizationAttribute> renderNodeAttributes) =>
            {
                // Buffers will be provided in the same order they appear in the layer metadata
                // If layerAttributes contained an additional element, it would be at inputAttributes.At(1)
                var nameAttribute = layerNodeAttributes.At(0);

                // The outputVisualizationAttributes array expects that its data is indexed the same way as the attributeDescriptions above
                var isBuildingOfInterestAttribute = renderNodeAttributes.At(0);

                var isBuildingOfInterestBuffer = isBuildingOfInterestAttribute.Data;
                var isBuildingOfInterestData = isBuildingOfInterestBuffer.Reinterpret<float>(sizeof(byte));

                // Go over each attribute and if its name is one of the four buildings of interest set its "isBuildingOfInterest" value to 1, otherwise set it to 0
                ForEachString(nameAttribute, (string element, Int32 index) =>
                {
                    isBuildingOfInterestData[index] = IsBuildingOfInterest(element);
                });
            };

            // Pass the layer attributes, attribute descriptions and the attribute processor to the layer
            layer.SetAttributesToVisualize(layerAttributes.MoveToArray(), renderAttributeDescriptions.MoveToArray(), attributeProcessor);

            // In Unity, open this material in the Shader Graph to view its implementation
            // In general, you can use this function in other scripts to change the material that is used to render the buildings
            layer.MaterialReference = new Material(Resources.Load<Material>("Materials/" + "BuildingNameRenderer"));
        }

        // This function checks if the building contains a name we are interested in visualizing
        private int IsBuildingOfInterest(string element)
        {
            if (element == null)
            {
                return 0;
            }
            else if (element.Equals("Empire State Building") || element.Equals("Chrysler Building") || element.Equals("Tower 1 World Trade Ctr") ||
                    element.Equals("One Chase Manhattan Plaza"))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    */
    


    // ForEachString takes care of converting the attribute buffer into a readable string value
    private void ForEachString(Esri.GameEngine.Attributes.ArcGISAttribute attribute, Action<string, Int32> predicate)
    {
        unsafe
        {
            var buffer = attribute.Data;
            var unsafePtr = NativeArrayUnsafeUtility.GetUnsafePtr(buffer);
            var metadata = (int*)unsafePtr;

            var count = metadata[0];

            // First integer = number of string on this array
            // Second integer = sum(strings length)
            // Next N-values (N = value of index 0 ) = Length of each string

            IntPtr stringPtr = (IntPtr)unsafePtr + (2 + count) * sizeof(int);

            for (var i = 0; i < count; ++i)
            {
                string element = null;

                // If the length of the string element is 0, it means the element is null
                if (metadata[2 + i] > 0)
                {
                    element = Marshal.PtrToStringAnsi(stringPtr, metadata[2 + i] - 1);
                }

                predicate(element, i);

                stringPtr += metadata[2 + i];
            }
        }
    }

	// This function detects the rendering pipeline used by the project to choose from the pre-defined materials made for HDRP or URP
	private string DetectRenderPipeline()
	{
		if (GraphicsSettings.renderPipelineAsset != null)
		{
			var renderType = GraphicsSettings.renderPipelineAsset.GetType().ToString();

			if (renderType == "UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset")
			{
				return "URP";
			}
			else if (renderType == "UnityEngine.Rendering.HighDefinition.HDRenderPipelineAsset")
			{
				return "HDRP";
			}
		}

		return "Legacy";
	}
}