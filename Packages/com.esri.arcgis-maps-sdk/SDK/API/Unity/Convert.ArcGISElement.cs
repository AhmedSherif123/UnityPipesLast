// COPYRIGHT 1995-2022 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts and Legal Services Department
// 380 New York Street
// Redlands, California, 92373
// USA
//
// email: contracts@esri.com
using System;
using System.Reflection;

namespace Esri.Unity
{
	internal static partial class Convert
	{
		internal static object FromArcGISElement(Standard.ArcGISElement element)
		{
			var type = element.ObjectType;

			object result;

			switch (type)
			{
				case Standard.ArcGISElementType.DatumTransformation:
					result = element.GetValueAsDatumTransformation();
					break;
				case Standard.ArcGISElementType.Dictionary:
					result = element.GetValueAsDictionary();
					break;
				case Standard.ArcGISElementType.Float64:
					result = element.GetValueAsFloat64();
					break;
				case Standard.ArcGISElementType.GEAttribute:
					result = element.GetValueAsGEAttribute();
					break;
				case Standard.ArcGISElementType.GEBuildingSceneLayerAttributeStatistics:
					result = element.GetValueAsGEBuildingSceneLayerAttributeStatistics();
					break;
				case Standard.ArcGISElementType.GeographicTransformationStep:
					result = element.GetValueAsGeographicTransformationStep();
					break;
				case Standard.ArcGISElementType.Geometry:
					result = element.GetValueAsGeometry();
					break;
				case Standard.ArcGISElementType.GEVisualizationAttribute:
					result = element.GetValueAsGEVisualizationAttribute();
					break;
				case Standard.ArcGISElementType.GEVisualizationAttributeDescription:
					result = element.GetValueAsGEVisualizationAttributeDescription();
					break;
				case Standard.ArcGISElementType.HorizontalVerticalTransformationStep:
					result = element.GetValueAsHorizontalVerticalTransformationStep();
					break;
				case Standard.ArcGISElementType.OAuthUserCredential:
					result = element.GetValueAsOAuthUserCredential();
					break;
				case Standard.ArcGISElementType.ArcGISCredential:
					result = element.GetValueAsArcGISCredential();
					break;
				case Standard.ArcGISElementType.OAuthUserTokenInfo:
					result = element.GetValueAsOAuthUserTokenInfo();
					break;
				case Standard.ArcGISElementType.ArcGISTokenInfo:
					result = element.GetValueAsArcGISTokenInfo();
					break;
				case Standard.ArcGISElementType.String:
					result = element.GetValueAsString();
					break;
				case Standard.ArcGISElementType.UInt32:
					result = element.GetValueAsUInt32();
					break;
				default:
					throw new InvalidCastException();
			}

			return result;
		}

		internal static T FromArcGISElement<T>(Standard.ArcGISElement element)
		{
			if (typeof(T).IsSubclassOf(typeof(ArcGISDictionaryBase)))
			{
				var dictionaryBase = FromArcGISElement<ArcGISDictionaryBase>(element);

				var dictionary = (T)Activator.CreateInstance(typeof(T), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { dictionaryBase.Handle }, null);

				dictionaryBase.Handle = IntPtr.Zero;

				return dictionary;
			}
			else
			{
				return (T)System.Convert.ChangeType(FromArcGISElement(element), typeof(T));
			}
		}

		internal static Standard.ArcGISElement ToArcGISElement<T>(T value)
		{
			Standard.ArcGISElement result;

			switch (value)
			{
				case ArcGISDictionaryBase converted:
					result = Standard.ArcGISElement.FromDictionary(converted);
					break;
				case GameEngine.Attributes.ArcGISAttribute converted:
					result = Standard.ArcGISElement.FromGEAttribute(converted);
					break;
				case GameEngine.Geometry.ArcGISDatumTransformation converted:
					result = Standard.ArcGISElement.FromDatumTransformation(converted);
					break;
				case double converted:
					result = Standard.ArcGISElement.FromFloat64(converted);
					break;
				case GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics converted:
					result = Standard.ArcGISElement.FromGEBuildingSceneLayerAttributeStatistics(converted);
					break;
				case GameEngine.Geometry.ArcGISGeographicTransformationStep converted:
					result = Standard.ArcGISElement.FromGeographicTransformationStep(converted);
					break;
				case GameEngine.Geometry.ArcGISGeometry converted:
					result = Standard.ArcGISElement.FromGeometry(converted);
					break;
				case GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep converted:
					result = Standard.ArcGISElement.FromHorizontalVerticalTransformationStep(converted);
					break;
				case GameEngine.Authentication.ArcGISOAuthUserCredential converted:
					result = Standard.ArcGISElement.FromOAuthUserCredential(converted);
					break;
				case GameEngine.Authentication.ArcGISOAuthUserTokenInfo converted:
					result = Standard.ArcGISElement.FromOAuthUserTokenInfo(converted);
					break;
				case GameEngine.Authentication.ArcGISTokenInfo converted:
					result = Standard.ArcGISElement.FromArcGISTokenInfo(converted);
					break;
				case GameEngine.Authentication.ArcGISCredential converted:
					result = Standard.ArcGISElement.FromArcGISCredential(converted);
					break;
				case string converted:
					result = Standard.ArcGISElement.FromString(converted);
					break;
				case uint converted:
					result = Standard.ArcGISElement.FromUInt32(converted);
					break;
				case GameEngine.Attributes.ArcGISVisualizationAttribute converted:
					result = Standard.ArcGISElement.FromGEVisualizationAttribute(converted);
					break;
				case GameEngine.Attributes.ArcGISVisualizationAttributeDescription converted:
					result = Standard.ArcGISElement.FromGEVisualizationAttributeDescription(converted);
					break;
				default:
					throw new InvalidCastException();
			}

			return result;
		}

		internal static Standard.ArcGISElementType ToArcGISElementType<T>()
		{
			if (typeof(T) == typeof(ArcGISDictionaryBase) ||
				typeof(T).IsSubclassOf(typeof(ArcGISDictionaryBase)))
			{
				return Standard.ArcGISElementType.Dictionary;
			}
			else if (typeof(T) == typeof(GameEngine.Attributes.ArcGISAttribute))
			{
				return Standard.ArcGISElementType.GEAttribute;
			}
			else if (typeof(T) == typeof(GameEngine.Geometry.ArcGISDatumTransformation))
			{
				return Standard.ArcGISElementType.DatumTransformation;
			}
			else if (typeof(T) == typeof(double))
			{
				return Standard.ArcGISElementType.Float64;
			}
			else if (typeof(T) == typeof(GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics))
			{
				return Standard.ArcGISElementType.GEBuildingSceneLayerAttributeStatistics;
			}
			else if (typeof(T) == typeof(GameEngine.Geometry.ArcGISGeographicTransformationStep))
			{
				return Standard.ArcGISElementType.GeographicTransformationStep;
			}
			else if (typeof(T) == typeof(GameEngine.Geometry.ArcGISGeometry) ||
					 typeof(T).IsSubclassOf(typeof(GameEngine.Geometry.ArcGISGeometry)))
			{
				return Standard.ArcGISElementType.Geometry;
			}
			else if (typeof(T) == typeof(GameEngine.Geometry.ArcGISHorizontalVerticalTransformationStep))
			{
				return Standard.ArcGISElementType.HorizontalVerticalTransformationStep;
			}
			else if (typeof(T) == typeof(string))
			{
				return Standard.ArcGISElementType.String;
			}
			else if (typeof(T) == typeof(uint))
			{
				return Standard.ArcGISElementType.UInt32;
			}
			else if (typeof(T) == typeof(GameEngine.Attributes.ArcGISVisualizationAttribute))
			{
				return Standard.ArcGISElementType.GEVisualizationAttribute;
			}
			else if (typeof(T) == typeof(GameEngine.Attributes.ArcGISVisualizationAttributeDescription))
			{
				return Standard.ArcGISElementType.GEVisualizationAttributeDescription;
			}
			else
			{
				throw new InvalidCastException();
			}
		}
	}
}
