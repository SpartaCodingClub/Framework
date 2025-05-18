using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class Scene_Loading : Scene_Base
{
    private static bool isInitialized;

    public async UniTaskVoid LoadSceneAsync(string key)
    {
        if (isInitialized == false)
        {
            await PreloadResourcesAsync();
            isInitialized = true;
        }

        await LoadResourcesAsync(GetLabel(key));

        var sceneOperation = Addressables.LoadSceneAsync(key, activateOnLoad: false);
        await sceneOperation;
    }

    private async UniTask PreloadResourcesAsync()
    {
        await LoadResourcesAsync("Preloading");
    }

    private async UniTask LoadResourcesAsync(string label)
    {
        var groupLocation = Addressables.LoadResourceLocationsAsync(label);
        await groupLocation;
        await Managers.Resource.CreateGenericGroupOperation(groupLocation);

        Addressables.Release(groupLocation);
    }
}