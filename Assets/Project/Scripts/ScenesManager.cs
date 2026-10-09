using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public void OpenScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
