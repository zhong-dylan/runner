using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class D3AddressablesManager : MonoBehaviour
{
    public static D3AddressablesManager Instance { get; private set; }

    private static readonly Dictionary<string, string> SceneAddresses = new Dictionary<string, string>
    {
        { "GameSceneinfintyRunner", "SceneGame/DemoInfinityRunner/GameSceneinfintyRunner" },
        { "GameSceneNoEnemy", "SceneGame/DemoNoEnemy/GameSceneNoEnemy" },
        { "TitleNoEnemy", "SceneGame/DemoNoEnemy/TitleNoEnemy" },
        { "GamePlayCurvedVariant1", "SceneGame/DemoRunnerLevelSystem/GamePlayCurvedVariant1" },
        { "GamePlayCurvedVariant2", "SceneGame/DemoRunnerLevelSystem/GamePlayCurvedVariant2" },
        { "GamePlayCurvedVariant3", "SceneGame/DemoRunnerLevelSystem/GamePlayCurvedVariant3" },
        { "TitleInfintyRunner", "Main" },
        { "TitleInfinityRunner", "Main" },
        { "TitleScene", "Main" }
    };

    private readonly Dictionary<object, AsyncOperationHandle> handles = new Dictionary<object, AsyncOperationHandle>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public AsyncOperationHandle<T> LoadAsset<T>(string address, Action<T> completed = null)
    {
        var handle = Addressables.LoadAssetAsync<T>(address);
        handles[address] = handle;

        if (completed != null)
        {
            handle.Completed += operation =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    completed(operation.Result);
                }
            };
        }

        return handle;
    }

    public AsyncOperationHandle<GameObject> Instantiate(string address, Transform parent = null, Action<GameObject> completed = null)
    {
        var handle = Addressables.InstantiateAsync(address, parent);
        handles[handle] = handle;

        if (completed != null)
        {
            handle.Completed += operation =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    completed(operation.Result);
                }
            };
        }

        return handle;
    }

    public AsyncOperationHandle<SceneInstance> LoadScene(string address, LoadSceneMode loadMode = LoadSceneMode.Single, Action<SceneInstance> completed = null)
    {
        var handle = Addressables.LoadSceneAsync(address, loadMode);
        handles[address] = handle;

        if (completed != null)
        {
            handle.Completed += operation =>
            {
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    completed(operation.Result);
                }
            };
        }

        return handle;
    }

    public static AsyncOperationHandle<SceneInstance> LoadSceneByName(string scene, LoadSceneMode loadMode = LoadSceneMode.Single, Action<SceneInstance> completed = null)
    {
        var address = GetSceneAddress(scene);
        if (string.Equals(address, "Main", StringComparison.OrdinalIgnoreCase))
        {
            SceneManager.LoadScene("Main", loadMode);
            return default;
        }

        return GetOrCreate().LoadScene(address, loadMode, completed);
    }

    private static D3AddressablesManager GetOrCreate()
    {
        if (Instance != null)
        {
            return Instance;
        }

        var gameObject = new GameObject(nameof(D3AddressablesManager));
        return gameObject.AddComponent<D3AddressablesManager>();
    }

    private static string GetSceneAddress(string scene)
    {
        if (string.IsNullOrEmpty(scene))
        {
            return scene;
        }

        var address = scene.Replace("\\", "/");
        if (address.StartsWith("Assets/Game/"))
        {
            address = address.Substring("Assets/Game/".Length);
        }

        if (address.EndsWith(".unity", StringComparison.OrdinalIgnoreCase))
        {
            address = address.Substring(0, address.Length - ".unity".Length);
        }

        if (address.Contains("/"))
        {
            return address;
        }

        return SceneAddresses.TryGetValue(address, out var mappedAddress) ? mappedAddress : address;
    }

    public AsyncOperationHandle<SceneInstance> UnloadScene(SceneInstance scene, bool autoReleaseHandle = true)
    {
        return Addressables.UnloadSceneAsync(scene, autoReleaseHandle);
    }

    public void Release(string address)
    {
        if (!handles.TryGetValue(address, out var handle))
        {
            return;
        }

        Addressables.Release(handle);
        handles.Remove(address);
    }

    public void ReleaseInstance(GameObject instance)
    {
        if (instance != null)
        {
            Addressables.ReleaseInstance(instance);
        }
    }
}
