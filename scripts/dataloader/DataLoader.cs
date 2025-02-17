using Godot;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Reflection;

public class DataLoader
{
    private bool m_isInitialised = false;

    private JsonSerializerOptions m_serializerOptions;

    private void Initialise()
    {
        m_serializerOptions = new JsonSerializerOptions{
            Converters = {
                new JsonStringEnumConverter()
            }
        };
    }

    public Task LoadAllResources()
    {
        Reset();
        Initialise();

        //var traitsTask = LoadResources<TraitList>("traitlists");

        return Task.CompletedTask;//Task.WhenAll(traitsTask);
    }

    private async Task LoadResources<LoadableResourceT>(Variant categoryName) where LoadableResourceT : AbstractLoadableDataResource
    {
        string resourceDirectoryPath = $"res://data/resources/{categoryName}";

        List<string> filenames = new List<string>();

        using var dir = DirAccess.Open(resourceDirectoryPath);
        if (dir != null)
        {
            dir.ListDirBegin();
            string fileName = dir.GetNext();
            while (fileName != "")
            {
                GD.Print("Found file: " + fileName);
                filenames.Add(fileName);
                fileName = dir.GetNext();
            }
        }

        foreach (string filename in filenames)
        {
            string filePath = $"{resourceDirectoryPath}/{filename}";
            ResourceLoader.LoadThreadedRequest(filePath, useSubThreads:true, cacheMode: ResourceLoader.CacheMode.Ignore);

            await LoadTask.WaitUntil(() => {
                GD.Print($"DataLoader: awaiting resource load at path {filePath}");
                return ResourceLoader.LoadThreadedGetStatus(filePath) != ResourceLoader.ThreadLoadStatus.InProgress;
            });

            if (ResourceLoader.LoadThreadedGetStatus(filePath) == ResourceLoader.ThreadLoadStatus.Failed)
            {
                GD.PushError($"DataLoader: resource load failed, path {filePath}");
                continue;
            }

            GD.Print($"Resource load finished, getting file of type {typeof(LoadableResourceT).Name} at path {filePath}");
            var loadedResource = (LoadableResourceT)ResourceLoader.LoadThreadedGet(filePath);
            
            loadedResource.PostLoadSetup();
        }
    }

    private void Reset()
    {
        m_isInitialised = false;
        m_serializerOptions = null;
    }
}