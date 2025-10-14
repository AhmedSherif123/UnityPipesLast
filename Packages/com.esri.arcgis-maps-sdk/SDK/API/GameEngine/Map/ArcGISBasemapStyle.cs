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
namespace Esri.GameEngine.Map
{
    /// <summary>
    /// The list of basemap styles.
    /// </summary>
    /// <remarks>
    /// This is used to determine which basemap to use.
    /// These basemaps are secured and require either an APIKey or named user to access them.
    /// </remarks>
    /// <since>1.0.0</since>
    public enum ArcGISBasemapStyle
    {
        /// <summary>
        /// A composite basemap with satellite imagery of the world (raster) as the base layer and labels (vector) as the reference layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISImagery = 0,
        
        /// <summary>
        /// A raster basemap with satellite imagery of the world as the base layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISImageryStandard = 1,
        
        /// <summary>
        /// A vector basemap with labels for the world as the reference layer. Designed to be overlaid on <see cref="GameEngine.Map.ArcGISBasemapStyle.ArcGISImageryStandard">ArcGISBasemapStyle.ArcGISImageryStandard</see>.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISImageryLabels = 2,
        
        /// <summary>
        /// A vector basemap for the world featuring a light neutral background style with minimal colors as the base layer and labels as the reference layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISLightGray = 3,
        
        /// <summary>
        /// A vector basemap for the world featuring a light neutral background style with minimal colors as the base layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISLightGrayBase = 4,
        
        /// <summary>
        /// A vector basemap with labels for the world as the reference layer. Designed to be overlaid on  light neutral backgrounds such as the <see cref="GameEngine.Map.ArcGISBasemapStyle.ArcGISLightGrayBase">ArcGISBasemapStyle.ArcGISLightGrayBase</see> style.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISLightGrayLabels = 5,
        
        /// <summary>
        /// A vector basemap for the world featuring a dark neutral background style with minimal colors as the base layer and labels as the reference layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISDarkGray = 6,
        
        /// <summary>
        /// A vector basemap for the world featuring a dark neutral background style with minimal colors as the base layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISDarkGrayBase = 7,
        
        /// <summary>
        /// A vector basemap with labels for the world as the reference layer. Designed to be overlaid on dark neutral backgrounds such as the <see cref="GameEngine.Map.ArcGISBasemapStyle.ArcGISDarkGrayBase">ArcGISBasemapStyle.ArcGISDarkGrayBase</see> style.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISDarkGrayLabels = 8,
        
        /// <summary>
        /// A vector basemap for the world featuring a custom navigation map style.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISNavigation = 9,
        
        /// <summary>
        /// A vector basemap for the world featuring a 'dark mode' version of the <see cref="GameEngine.Map.ArcGISBasemapStyle.ArcGISNavigation">ArcGISBasemapStyle.ArcGISNavigation</see> style, using the same content.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISNavigationNight = 10,
        
        /// <summary>
        /// A vector basemap for the world featuring a classic Esri street map style.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISStreets = 11,
        
        /// <summary>
        /// A vector basemap for the world featuring a custom 'night time' street map style.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISStreetsNight = 12,
        
        /// <summary>
        /// A composite basemap with elevation hillshade (raster) and a classic Esri street map style (vector) as the base layers.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISStreetsRelief = 13,
        
        /// <summary>
        /// A composite basemap with elevation hillshade (raster) and classic Esri topographic map style including a relief map (vector) as the base layers.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISTopographic = 14,
        
        /// <summary>
        /// A composite basemap with ocean data of the world (raster) as the base layer and labels (vector) as the reference layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISOceans = 15,
        
        /// <summary>
        /// A raster basemap with ocean data of the world as the base layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISOceansBase = 16,
        
        /// <summary>
        /// A vector basemap with labels for the world as the reference layer. Designed to be overlaid on <see cref="GameEngine.Map.ArcGISBasemapStyle.ArcGISOceansBase">ArcGISBasemapStyle.ArcGISOceansBase</see>.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISOceansLabels = 17,
        
        /// <summary>
        /// A composite basemap with elevation hillshade (raster), minimal map content like water and land fill, water lines and roads (vector)
        /// as the base layers and minimal map content like populated place names, admin and water labels with boundary lines (vector) as the reference layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISTerrain = 18,
        
        /// <summary>
        /// A vector basemap with minimal map content like water and land fill, water lines and roads as the base layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISTerrainBase = 19,
        
        /// <summary>
        /// A vector basemap with minimal map content like populated place names, admin and water labels with boundary lines as the 
        /// reference layer. Designed to be overlaid on <see cref="GameEngine.Map.ArcGISBasemapStyle.ArcGISTerrainBase">ArcGISBasemapStyle.ArcGISTerrainBase</see> and hillshade.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISTerrainDetail = 20,
        
        /// <summary>
        /// A vector basemap for the world in a style that is optimized to display special areas of 
        /// interest (AOIs) that have been created and edited by Community Maps contributors.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISCommunity = 21,
        
        /// <summary>
        /// A composite basemap with elevation hillshade (raster) and the world featuring a geopolitical style 
        /// reminiscent of a school classroom wall map (vector) as the base layers.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISChartedTerritory = 22,
        
        /// <summary>
        /// A vector basemap presented in the style of hand-drawn, colored pencil cartography.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISColoredPencil = 23,
        
        /// <summary>
        /// A vector basemap for the world featuring a dark background with glowing blue symbology inspired by science-fiction and futuristic themes.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISNova = 24,
        
        /// <summary>
        /// A composite basemap with elevation hillshade (raster) and the look of 18th and 19th century antique maps 
        /// in the modern world of multi-scale mapping (vector) as the base layers.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISModernAntique = 25,
        
        /// <summary>
        /// A vector basemap inspired by the art and advertising of the 1950's that presents a unique design option to the ArcGIS basemaps.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISMidcentury = 26,
        
        /// <summary>
        /// A vector basemap in black & white design with halftone patterns, red highlights, and stylized fonts to depict a unique "newspaper" styled theme.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISNewspaper = 27,
        
        /// <summary>
        /// A raster basemap with elevation hillshade. Designed to be used as a backdrop for topographic, soil, hydro, landcover 
        /// or other outdoor recreational maps.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISHillshadeLight = 28,
        
        /// <summary>
        /// A raster basemap with world hillshade (Dark) is useful in building maps that provide terrain context while highlighting feature layers and labels.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISHillshadeDark = 29,
        
        /// <summary>
        /// A vector basemap in the classic Esri street map style, using a relief map as the base layer. This is a transparent basemap so it is recommended to use it along with a hillshade (raster) layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISStreetsReliefBase = 30,
        
        /// <summary>
        /// A vector basemap in the classic Esri topographic map style, using a relief map as the base layer. This is a transparent basemap  so it is recommended to use it along with a hillshade (raster) layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISTopographicBase = 31,
        
        /// <summary>
        /// A vector basemap in a geopolitical style reminiscent of a school classroom wall map as the base layer. This is a transparent basemap so it is recommended to use it along with a hillshade (raster) layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISChartedTerritoryBase = 32,
        
        /// <summary>
        /// A vector basemap in the style of 18th and 19th century antique maps as the base layer. This is a transparent basemap so it is recommended to use it along with a hillshade (raster) layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISModernAntiqueBase = 33,
        
        /// <summary>
        /// A vector tile basemap for ArcGIS Human Geography, with labels.
        /// </summary>
        /// <remarks>
        /// A vector tile layer basemap containing monochromatic land polygons.
        /// This map is designed for use with Human Geography Label and Detail
        /// layers. The default global place labels are in English.
        /// </remarks>
        /// <since>1.4.0</since>
        ArcGISHumanGeography = 34,
        
        /// <summary>
        /// A vector tile basemap for ArcGIS Human Geography.
        /// </summary>
        /// <remarks>
        /// A vector tile layer basemap containing monochromatic land polygons.
        /// This map is designed for use with Human Geography Label and Detail
        /// layers.
        /// </remarks>
        /// <since>1.4.0</since>
        ArcGISHumanGeographyBase = 35,
        
        /// <summary>
        /// A detailed vector tile basemap for ArcGIS Human Geography.
        /// </summary>
        /// <remarks>
        /// A vector tile layer providing a detailed basemap for the world,
        /// featuring a monochromatic style with content adjusted to support
        /// Human Geography information. This map is designed for use with Human
        /// Geography Label and Base layers. The default global place labels are
        /// in English.
        /// </remarks>
        /// <since>1.4.0</since>
        ArcGISHumanGeographyDetail = 36,
        
        /// <summary>
        /// A vector tile basemap for ArcGIS Human Geography labels.
        /// </summary>
        /// <remarks>
        /// A vector tile layer providing a detailed basemap for the world,
        /// featuring a monochromatic style with content consisting of labels to
        /// support Human Geography information. This map is designed for use
        /// with Human Geography Detail and Base layers. The default global
        /// place labels are in English.
        /// </remarks>
        /// <since>1.4.0</since>
        ArcGISHumanGeographyLabels = 37,
        
        /// <summary>
        /// A vector tile basemap for dark ArcGIS Human Geography, with labels.
        /// </summary>
        /// <remarks>
        /// A vector tile layer basemap containing dark monochromatic land
        /// polygons. This map is designed for use with Human Geography Dark
        /// Label and Detail layers. The default global place labels are in
        /// English.
        /// </remarks>
        /// <since>1.4.0</since>
        ArcGISHumanGeographyDark = 38,
        
        /// <summary>
        /// A vector tile basemap for dark ArcGIS Human Geography.
        /// </summary>
        /// <remarks>
        /// A vector tile layer basemap containing dark monochromatic land
        /// polygons. This map is designed for use with Human Geography Dark
        /// Label and Detail layers. The default global place labels are in
        /// English.
        /// </remarks>
        /// <since>1.4.0</since>
        ArcGISHumanGeographyDarkBase = 39,
        
        /// <summary>
        /// A detailed vector tile basemap for dark ArcGIS Human Geography, with labels.
        /// </summary>
        /// <remarks>
        /// A vector tile layer providing a detailed basemap for the world,
        /// featuring a dark monochromatic style with content adjusted to
        /// support Human Geography information. This map is designed for use
        /// with Human Geography Dark Label and Base layers. The default global
        /// place labels are in English.
        /// </remarks>
        /// <since>1.4.0</since>
        ArcGISHumanGeographyDarkDetail = 40,
        
        /// <summary>
        /// A vector tile basemap for dark ArcGIS Human Geography labels.
        /// </summary>
        /// <remarks>
        /// A vector tile layer providing a detailed basemap for the world,
        /// featuring a dark monochromatic style with content adjusted to
        /// support Human Geography information. This map is designed for use
        /// with Human Geography Dark Detail and Base layers. The default global
        /// place labels are in English.
        /// </remarks>
        /// <since>1.4.0</since>
        ArcGISHumanGeographyDarkLabels = 41,
        
        /// <summary>
        /// A detailed vector tile basemap for the natural world.
        /// </summary>
        /// <remarks>
        /// A vector tile layer providing a detailed basemap with an emphasis on the natural world.
        /// It includes rich cartographic styling with vector contours and vector hillshade.
        /// This is a multisource style.
        /// The default global place labels are in English.
        /// </remarks>
        /// <since>1.6.0</since>
        ArcGISOutdoor = 42,
        
        /// <summary>
        /// A vector basemap version of Open Street Map (OSM) data hosted by Esri and rendered using Open Street Map (OSM) cartography.
        /// </summary>
        /// <since>1.0.0</since>
        OSMStandard = 101,
        
        /// <summary>
        /// A composite basemap that uses elevation as an artistic hillshade (raster) and Open Street Map (OSM) data hosted by Esri. 
        /// It is rendered similarly to the Esri Street Map (with Relief) and uses a cartographic style (vector) as the base layer.
        /// </summary>
        /// <since>1.0.0</since>
        OSMStandardRelief = 102,
        
        /// <summary>
        /// A vector basemap is a version of Open Street Map (OSM) data hosted by Esri and rendered using Esri Street Map cartographic style.
        /// </summary>
        /// <since>1.0.0</since>
        OSMStandardReliefBase = 103,
        
        /// <summary>
        /// A vector basemap version of Open Street Map (OSM) data hosted by Esri and rendered using Esri Street Map style.
        /// </summary>
        /// <since>1.0.0</since>
        OSMStreets = 104,
        
        /// <summary>
        /// A composite basemap with elevation as an artistic hillshade (raster) and Open Street Map (OSM) data hosted by Esri and 
        /// rendered similarly to the Esri Street Map (with Relief) cartographic style (vector) as the base layers.
        /// </summary>
        /// <since>1.0.0</since>
        OSMStreetsRelief = 105,
        
        /// <summary>
        /// A vector basemap version of Open Street Map (OSM) data hosted by Esri and rendered using light neutral style with minimal 
        /// colors as the base layer and labels as the reference layer.
        /// </summary>
        /// <since>1.0.0</since>
        OSMLightGray = 106,
        
        /// <summary>
        /// A vector basemap version of Open Street Map (OSM) data hosted by Esri and rendered using light neutral style with minimal 
        /// colors as the base layer.
        /// </summary>
        /// <since>1.0.0</since>
        OSMLightGrayBase = 107,
        
        /// <summary>
        /// A vector basemap version of Open Street Map (OSM) data hosted by Esri. Contains only labels as the reference layer. Designed to be overlaid
        /// on light neutral styles with minimal colors such as <see cref="GameEngine.Map.ArcGISBasemapStyle.OSMLightGrayBase">ArcGISBasemapStyle.OSMLightGrayBase</see>.
        /// </summary>
        /// <since>1.0.0</since>
        OSMLightGrayLabels = 108,
        
        /// <summary>
        /// A vector basemap version of Open Street Map (OSM) data hosted by Esri and rendered using dark neutral style with minimal colors 
        /// as the base layer and labels as the reference layer.
        /// </summary>
        /// <since>1.0.0</since>
        OSMDarkGray = 109,
        
        /// <summary>
        /// A vector basemap version of Open Street Map (OSM) data hosted by Esri and rendered using dark neutral style with minimal colors as the base layer.
        /// </summary>
        /// <since>1.0.0</since>
        OSMDarkGrayBase = 110,
        
        /// <summary>
        /// A vector basemap version of Open Street Map (OSM) data hosted by Esri. Contains only labels as the reference layer. Designed to be overlaid on dark neutral styles with minimal colors such as <see cref="GameEngine.Map.ArcGISBasemapStyle.OSMDarkGrayBase">ArcGISBasemapStyle.OSMDarkGrayBase</see>.
        /// </summary>
        /// <since>1.0.0</since>
        OSMDarkGrayLabels = 111,
        
        /// <summary>
        /// A vector basemap with Open Street Map (OSM) data hosted by Esri. It is rendered similarly to the Esri Street Map (with Relief) and uses a cartographic style as the base layer. This is a transparent basemap so it is recommended to use it along with a hillshade (raster) layer.
        /// </summary>
        /// <since>1.0.0</since>
        OSMStreetsReliefBase = 112,
        
        /// <summary>
        /// An Open Street Map (OSM) vector basemap presented using a blueprint style.
        /// </summary>
        /// <remarks>
        /// This vector basemap is based on the Daylight map distribution of OSM
        /// data and is hosted by Esri. The map is presented in a creative
        /// cartographic style emulating a blueprint technical drawing. The
        /// default place labels are shown in the local language for where the
        /// label appears. For example, labels for places in France are shown in
        /// French.
        /// </remarks>
        /// <since>1.4.0</since>
        OSMBlueprint = 113,
        
        /// <summary>
        /// An Open Street Map (OSM) basemap for the world.
        /// </summary>
        /// <remarks>
        /// A basemap version of Open Street Map (OSM) data hosted by Esri. This
        /// layer presents low-resolution satellite imagery for the world and
        /// high-resolution satellite and aerial imagery, typically within 3-5
        /// years of currency, for most of the world. The default place labels
        /// are shown in the local language for where the label appears. For
        /// example, labels for places in France are shown in French.
        /// </remarks>
        /// <since>1.4.0</since>
        OSMHybrid = 114,
        
        /// <summary>
        /// An Open Street Map (OSM) detailed vector basemap for the world.
        /// </summary>
        /// <remarks>
        /// A basemap version of Open Street Map (OSM) data hosted by Esri. This
        /// vector tile layer provides a detailed reference layer for the world
        /// designed to be overlaid on imagery. The default place labels are
        /// shown in the local language for where the label appears. For
        /// example, labels for places in France are shown in French.
        /// </remarks>
        /// <since>1.4.0</since>
        OSMHybridDetail = 115,
        
        /// <summary>
        /// An Open Street Map (OSM) vector basemap presented using the Navigation style.
        /// </summary>
        /// <remarks>
        /// This vector basemap is based on the Daylight map distribution of OSM
        /// data and is hosted by Esri. It presents the map in a cartographic
        /// style similar to the Esri Vector Basemap Navigation map. The default
        /// place labels are shown in the local language for where the label
        /// appears. For example, labels for places in France are shown in
        /// French.
        /// </remarks>
        /// <since>1.4.0</since>
        OSMNavigation = 116,
        
        /// <summary>
        /// An Open Street Map (OSM) vector basemap presented using the dark Navigation style.
        /// </summary>
        /// <remarks>
        /// This vector basemap is based on the Daylight map distribution of OSM
        /// data and is hosted by Esri. It presents the map in a cartographic
        /// style similar to the Esri Vector Basemap Navigation Dark Mode map.
        /// The default place labels are shown in the local language for where
        /// the label appears. For example, labels for places in France are
        /// shown in French.
        /// </remarks>
        /// <since>1.4.0</since>
        OSMNavigationDark = 117,
        
        /// <summary>
        /// A vector basemap version of Open Basemap data using a blueprint style.
        /// </summary>
        /// <since>2.1.0</since>
        OpenBlueprint = 200,
        
        /// <summary>
        /// A vector basemap version of Open Basemap data hosted by Esri, and rendered using dark neutral style
        /// with minimal colors as the base layer, and labels as the reference layer.
        /// </summary>
        /// <remarks>
        /// This global basemap is based on the <see cref="Overture Maps">https://overturemaps.org/</see> data, which features OpenStreetMap and other open data sources.
        /// It provides a global basemap with cartography symbolized with a dark gray, neutral background style with
        /// minimal colors, content, and labels that is designed to draw attention to your thematic content.
        /// </remarks>
        /// <since>2.1.0</since>
        OpenDarkGray = 201,
        
        /// <summary>
        /// A vector basemap version of Open Basemap data hosted by Esri and rendered using dark neutral style
        /// with minimal colors as the base layer.
        /// </summary>
        /// <remarks>
        /// This vector tile layer is based on the <see cref="Overture Maps">https://overturemaps.org/</see> data, which features OpenStreetMap and other open data
        /// sources. It provides a global base layer featuring a dark gray, neutral style designed to draw attention
        /// to your thematic content. This layer is designed to be used with the
        /// Open Basemap Dark Gray Canvas Reference layer.
        /// </remarks>
        /// <since>2.1.0</since>
        OpenDarkGrayBase = 202,
        
        /// <summary>
        /// A vector basemap version of Open Basemap data hosted by Esri. Contains only labels as the reference layer.
        /// Designed to be overlaid on dark neutral styles with minimal colors such as <see cref="GameEngine.Map.ArcGISBasemapStyle.OpenDarkGrayBase">ArcGISBasemapStyle.OpenDarkGrayBase</see>.
        /// </summary>
        /// <since>2.1.0</since>
        OpenDarkGrayLabels = 203,
        
        /// <summary>
        /// An Open Basemap for the world.
        /// </summary>
        /// <remarks>
        /// This vector tile layer is based on the <see cref="Overture Maps">https://overturemaps.org/</see> data, which features OpenStreetMap and other open data sources.
        /// It provides a global reference overlay featuring highways, major and minor roads, railways, water features,
        /// cities, parks, landmarks, administrative boundaries and map labels, hosted by Esri.
        /// </remarks>
        /// <since>2.1.0</since>
        OpenHybrid = 204,
        
        /// <summary>
        /// An Open Basemap detailed vector basemap for the world.
        /// </summary>
        /// <since>2.1.0</since>
        OpenHybridDetail = 205,
        
        /// <summary>
        /// A vector basemap version of Open Basemap data hosted by Esri and rendered using light neutral style with minimal 
        /// colors as the base layer and labels as the reference layer.
        /// </summary>
        /// <since>2.1.0</since>
        OpenLightGray = 206,
        
        /// <summary>
        /// A vector basemap version of Open Basemap data hosted by Esri and rendered using light neutral style with minimal 
        /// colors as the base layer.
        /// </summary>
        /// <remarks>
        /// This vector tile layer is based on the <see cref="Overture Maps">https://overturemaps.org/</see> data, which features OpenStreetMap and other open data sources.
        /// It provides a global base layer featuring a light gray, neutral style designed to draw attention to your
        /// thematic content. This layer is designed to be used with the <see cref="GameEngine.Map.ArcGISBasemapStyle.OpenLightGray">ArcGISBasemapStyle.OpenLightGray</see> layer.
        /// </remarks>
        /// <since>2.1.0</since>
        OpenLightGrayBase = 207,
        
        /// <summary>
        /// A vector basemap version of Open Basemap data hosted by Esri. Contains only labels as the reference layer.
        /// Designed to be overlaid on light neutral styles with minimal colors such as <see cref="GameEngine.Map.ArcGISBasemapStyle.OpenLightGrayBase">ArcGISBasemapStyle.OpenLightGrayBase</see>.
        /// </summary>
        /// <remarks>
        /// This vector tile layer is based on the <see cref="Overture Maps">https://overturemaps.org/</see> data, which features OpenStreetMap and other open data sources.
        /// It provides a global reference layer featuring labels.
        /// This layer is designed to be used with the <see cref="GameEngine.Map.ArcGISBasemapStyle.OpenLightGrayBase">ArcGISBasemapStyle.OpenLightGrayBase</see> layer.
        /// </remarks>
        /// <since>2.1.0</since>
        OpenLightGrayLabels = 208,
        
        /// <summary>
        /// An Open Basemap vector basemap presented using the Navigation style.
        /// </summary>
        /// <since>2.1.0</since>
        OpenNavigation = 209,
        
        /// <summary>
        /// An Open Basemap vector basemap presented using the dark Navigation style.
        /// </summary>
        /// <since>2.1.0</since>
        OpenNavigationDark = 210,
        
        /// <summary>
        /// An Open Basemap vector basemap presented using the OSM style.
        /// </summary>
        /// <remarks>
        /// The OpenStreetMap Style map style provides a global basemap with cartography representative of the
        /// OpenStreetMap style for Open Basemaps. The style provides unique capabilities for customization
        /// and high-resolution display. This comprehensive map includes highways, major and minor roads, railways,
        /// water features, cities, parks, landmarks, building footprints, administrative boundaries, and map labels.
        /// </remarks>
        /// <since>2.1.0</since>
        OpenOSMStyle = 211,
        
        /// <summary>
        /// An Open Basemap vector basemap presented using the OSM style.
        /// </summary>
        /// <remarks>
        /// The OpenStreetMap Style with Relief map style provides a global basemap with cartography representative of
        /// the OpenStreetMap style, designed to be used with shaded relief, for Open Basemaps. The style provides unique
        /// capabilities for customization and high-resolution display. This comprehensive map includes highways,
        /// major and minor roads, railways, water features, cities, parks, landmarks, building footprints,
        /// administrative boundaries, and map labels.
        /// </remarks>
        /// <since>2.1.0</since>
        OpenOSMStyleRelief = 212,
        
        /// <summary>
        /// An Open Basemap vector basemap presented using the OSM style.
        /// </summary>
        /// <since>2.1.0</since>
        OpenOSMStyleReliefBase = 213,
        
        /// <summary>
        /// A vector basemap version of Open Basemap data hosted by Esri and rendered using Esri Street Map style.
        /// </summary>
        /// <remarks>
        /// This vector tile layer is based on the <see cref="Overture Maps">https://overturemaps.org/</see> data, which features OpenStreetMap and other open data
        /// sources. It provides a global basemap symbolized in a street map style, emphasizing the road network
        /// and urban landscape.
        /// </remarks>
        /// <since>2.1.0</since>
        OpenStreets = 214,
        
        /// <summary>
        /// A vector basemap version of Open Basemap data hosted by Esri and rendered using Esri Street Map style.
        /// </summary>
        /// <remarks>
        /// This vector tile layer is based on the <see cref="Overture Maps">https://overturemaps.org/</see> data, which features OpenStreetMap and other open data
        /// sources. It provides a global basemap symbolized in a street map style, emphasizing the road network
        /// and urban landscape, designed for use at night or other low-light environments.
        /// </remarks>
        /// <since>2.1.0</since>
        OpenStreetsNight = 215,
        
        /// <summary>
        /// A composite basemap with elevation as an artistic hillshade (raster) and Open Street Map (OSM) data hosted by
        /// Esri and rendered similarly to the Esri Street Map (with Relief) cartographic style (vector) as the base layers.
        /// </summary>
        /// <remarks>
        /// This vector tile layer is based on the <see cref="Overture Maps">https://overturemaps.org/</see> data, which features OpenStreetMap and other open data 
        /// sources. It provides a global basemap symbolized in a street map style, emphasizing the road network
        /// and urban landscape and designed to be used with shaded relief, hosted by Esri.
        /// </remarks>
        /// <since>2.1.0</since>
        OpenStreetsRelief = 216,
        
        /// <summary>
        /// A vector basemap version of Open Basemap data hosted by Esri and rendered similarly to the Esri Street Map
        /// (with Relief) cartographic style (vector) as the base layers.
        /// </summary>
        /// <since>2.1.0</since>
        OpenStreetsReliefBase = 217
    };
}