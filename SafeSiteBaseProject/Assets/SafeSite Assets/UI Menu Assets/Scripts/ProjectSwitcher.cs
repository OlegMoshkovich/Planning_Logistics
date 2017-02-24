using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ProjectSwitcher : MonoBehaviour {
    public void ToggleProjectSwitcher()
    {
        if (gameObject.activeInHierarchy) gameObject.SetActive(false);
        else gameObject.SetActive(true);
    }
	public void loadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
