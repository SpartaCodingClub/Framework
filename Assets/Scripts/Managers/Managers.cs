using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers Instance { get; private set; }

    public static readonly AudioManager Audio = new();
    public static readonly CameraManager Camera = new();
    public static readonly GameManager Game = new();
    public static readonly InputManager Input = new();
    public static readonly PoolManager Pool = new();
    public static readonly ResourceManager Resource = new();
    public static readonly SceneManager Scene = new();
    public static readonly UIManager UI = new();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Audio.Initialize();
        Camera.Initialize();
        Game.Initialize();
        Pool.Initialize();
        UI.Initialize();
    }

    public void Clear()
    {
        Scene.Clear();
        UI.Clear();

        // LateClear
        Pool.Clear();
    }

    private void Update()
    {
        Camera.Update();
    }
}