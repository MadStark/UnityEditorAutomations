using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadEnvironment : MonoBehaviour
{
    private void Start()
    {
        AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "main/main"));

        SceneManager.LoadScene("Env1", LoadSceneMode.Additive);
        SceneManager.LoadScene("Env2", LoadSceneMode.Additive);
        SceneManager.LoadScene("Env3", LoadSceneMode.Additive);
        SceneManager.LoadScene("Env4", LoadSceneMode.Additive);
    }
}
