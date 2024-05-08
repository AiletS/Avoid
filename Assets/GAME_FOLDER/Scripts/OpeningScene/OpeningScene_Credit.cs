using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScene_Credit : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) load_scene("OpeningScene_Main");
    }

    public void load_scene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
}
