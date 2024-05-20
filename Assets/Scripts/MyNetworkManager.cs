using UnityEngine;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.IO;
using Mirror.Examples.AdditiveLevels;

public class MyNetworkManager : NetworkManager
{
    public string firstSceneToLoad;
    public FadeInOutScreen fadeInOut;

    private string[] scenesToLoad;
    private bool subscenesLoaded;

    private readonly List<Scene> subScene = new List<Scene>();

    private bool isInTransition;
    private bool firstSceneLoaded;

    private void Start()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings - 2;
        scenesToLoad = new string[sceneCount];

        for (int i = 0; i < sceneCount; i++)
        {
            scenesToLoad[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i + 2));
        }
    }


    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);

        if (sceneName == onlineScene)
        {
            StartCoroutine(ServerLoadSubScene());
        }
    }

    public override void OnClientSceneChanged()
    {
        if (isInTransition == false)
        {
            base.OnClientSceneChanged();
        }
    }

    IEnumerator ServerLoadSubScene()
    {
        foreach (var additiveScene in scenesToLoad)
        {
            yield return SceneManager.LoadSceneAsync(additiveScene, new LoadSceneParameters
            {
                loadSceneMode = LoadSceneMode.Additive,
                localPhysicsMode = LocalPhysicsMode.Physics2D
            });
        }

        subscenesLoaded = true;
    }

    IEnumerator LoadAdditive(string sceneName)
    {
        isInTransition = true;

        yield return fadeInOut.FadeIn();

        if(mode == NetworkManagerMode.ClientOnly)
        {
            loadingSceneAsync = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (loadingSceneAsync != null && !loadingSceneAsync.isDone)
            {
                yield return null;
            }
        }

        NetworkClient.isLoadingScene = false;
        isInTransition = false;

        OnClientSceneChanged();

        if (firstSceneLoaded == false)
        {
            firstSceneLoaded = true;

            yield return new WaitForSeconds(0.6f);
        }
        else
        {
            firstSceneLoaded = true;

            yield return new WaitForSeconds(0.5f);
        }

        yield return fadeInOut.FadeOut();
    }

    IEnumerator UnloadAdditive(string sceneName)
    {
        isInTransition = true;

        yield return fadeInOut.FadeIn();

        if (mode == NetworkManagerMode.ClientOnly)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
            yield return Resources.UnloadUnusedAssets();
        }

        NetworkClient.isLoadingScene = false;
        isInTransition = false; 
        
        OnClientSceneChanged();
    }
}
