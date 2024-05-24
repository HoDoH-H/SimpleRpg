using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject cameraBrain;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(cameraBrain);
    }
}
