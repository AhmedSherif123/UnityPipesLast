// Copyright 2022 Esri.
// Licensed under the Apache License, Version 2.0

using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class ArcGISDataFetcher : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    public async void FetchFeatureData(int objectID, string position)
    {
        Debug.LogWarning("Start fetching . . .  .");
        string url = $"https://services3.arcgis.com/YhesYCzrdc1fSZZx/arcgis/rest/services/Pipes/FeatureServer/0/query?where=OBJECTID_1={objectID}&outFields=FamilyType&returnGeometry=false&f=pjson";

        Debug.Log($"[ArcGISDataFetcher] Fetching data from: {url}");

        string response = await FetchWebDataAsync(url);

        if (string.IsNullOrEmpty(response))
        {
            Debug.LogError("[ArcGISDataFetcher] Empty or failed web response.");
            if (resultText != null)
                resultText.text = "Failed to fetch data.";
            return;
        }

        SaveFullJson(response, objectID);
        DisplayFullJson(response, objectID, position);
    }

    private async Task<string> FetchWebDataAsync(string url)
    {
        using UnityWebRequest request = UnityWebRequest.Get(url);
        var operation = request.SendWebRequest();

        while (!operation.isDone)
        {
         
        await Task.Yield();
}
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"[ArcGISDataFetcher] Request failed: {request.error}");
            return null;
        }

        return request.downloadHandler.text;
    }

    private void SaveFullJson(string json, int featureId)
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, $"ArcGIS_Feature_{featureId}.json");
            File.WriteAllText(path, json);
            Debug.Log($"[ArcGISDataFetcher] JSON saved at: {path}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[ArcGISDataFetcher] Failed to save JSON: {ex.Message}");
        }
    }

    private void DisplayFullJson(string jsonResponse, int featureId, string position)
    {
        if (resultText == null)
            return;

        resultText.text = $"\nFeatureID: {featureId}\n\n";

        try
        {
            var jObject = JObject.Parse(jsonResponse);
            var features = jObject["features"] as JArray;

            if (features == null || features.Count == 0)
            {
                resultText.text += "No features found.\n";
                return;
            }

            foreach (var feature in features)
            {
                
                var attributes = feature["attributes"] as JObject;

                if (attributes != null)
                {
                    foreach (var prop in attributes.Properties())
                    {
                        resultText.text += $"{prop.Name} : {prop.Value}\n";
                    }
                }

            }

          
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[ArcGISDataFetcher] JSON parse error: {ex.Message}");
            resultText.text += "\nError parsing JSON.\n";
        }
    }
}
