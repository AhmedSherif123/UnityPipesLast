// COPYRIGHT 1995-2025 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Attn: Contracts and Legal Department
// Environmental Systems Research Institute, Inc.
// 380 New York Street
// Redlands, California 92373
// USA
//
// email: legal@esri.com
namespace Esri.GameEngine.RCQ
{
    /// <summary>
    /// Material parameter scalars.
    /// </summary>
    public enum ArcGISMaterialScalarProperty
    {
        /// <summary>
        /// Clipping mode.
        /// </summary>
        ClippingMode = 0,
        
        /// <summary>
        /// Use uv region lut.
        /// </summary>
        UseUvRegionLut = 1,
        
        /// <summary>
        /// Metallic factor for PBR.
        /// </summary>
        Metallic = 2,
        
        /// <summary>
        /// Roughness factor for PBR.
        /// </summary>
        Roughness = 3,
        
        /// <summary>
        /// Opacity factor.
        /// </summary>
        Opacity = 4,
        
        /// <summary>
        /// Flag indicating whether the Renderable has transparency.
        /// </summary>
        HasTransparency = 5,
        
        /// <summary>
        /// Face culling and lighting.
        /// </summary>
        FaceProperties = 6,
        
        /// <summary>
        /// Draw order on screen space
        /// </summary>
        DrawOrder = 7,
        
        /// <summary>
        /// Screen space placement for ScreenOverlay.
        /// </summary>
        Placement = 8
    };
}