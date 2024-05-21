using UnityEngine;
using Mirror;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class TransitionToScene : MonoBehaviour
{
    private MyNetworkManager myNetworkManagerScript;
    private FadeInOutScreen myFadeInOutScreenScript;

    [Scene]
    public string transitionToSceneName;
    public string scenePosToSpawnOn;

    private void Awake()
    {
        if (myNetworkManagerScript == null)
        {
            myNetworkManagerScript = FindAnyObjectByType<MyNetworkManager>();
            myFadeInOutScreenScript = FindAnyObjectByType<FadeInOutScreen>();
        }
    }
}
