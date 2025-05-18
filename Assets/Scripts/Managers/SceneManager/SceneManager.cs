using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class SceneManager
{
    public Scene_Base CurrentScene { get; set; }

    public void Clear() => CurrentScene.Clear();

    public void LoadSceneAsync<T>() where T : Scene_Base
    {
        Managers.Instance.Clear();

        string key = typeof(T).Name;
        LoadSceneAsync(key).Forget();
    }

    private async UniTaskVoid LoadSceneAsync(string key)
    {
        await Addressables.LoadSceneAsync(nameof(Scene_Loading));
        (CurrentScene as Scene_Loading).LoadSceneAsync(key).Forget();
    }
}