using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public List<GameObject> dontDestroy;
    public static Scene_Manager instance;

    private bool ChangedScene;

    private void Awake()
    {
        Scene_Manager _instance = FindObjectOfType<Scene_Manager>().GetComponent<Scene_Manager>();
        if (instance != null || _instance!=this)
        {
            Destroy(this.gameObject);
            foreach (GameObject obj in dontDestroy)
                Destroy(obj);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            foreach (GameObject obj in dontDestroy)
                DontDestroyOnLoad(obj);
        }
        instance = this;
    }

    private void Update()
    {
        if(ChangedScene) ChangedScene = false;
    }

    public void RemoveDontDestroy(GameObject gameObject)
    {
        if (dontDestroy.Contains(gameObject))
        {
            dontDestroy.Remove(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddDontDestroy(GameObject gameObject)
    {
        if (!dontDestroy.Contains(gameObject))
        {
            dontDestroy.Add(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
        ChangedScene = true;
    }

    public void LoadSceneName(string name)
    {
        SceneManager.LoadScene(name);
        ChangedScene = true;
    }

    public int GetActiveIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

}
