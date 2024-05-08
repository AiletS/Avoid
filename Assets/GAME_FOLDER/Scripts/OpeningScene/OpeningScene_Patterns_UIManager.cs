using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class OpeningScene_Patterns_UIManager : MonoBehaviour
{
    [SerializeField] GameObject explmove;


    private void Update()
    {
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            Vector3 pos = explmove.transform.position;
            Vector3 npos = new Vector3(pos.x, Mathf.Min(27f, pos.y + 25 * Time.deltaTime), pos.z);
            explmove.transform.position = npos;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            Vector3 pos = explmove.transform.position;
            Vector3 npos = new Vector3(pos.x, Mathf.Max(-1.5f, pos.y - 25 * Time.deltaTime), pos.z);
            explmove.transform.position = npos;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) load_scene("OpeningScene_Main");
    }

    public void load_scene(string scene_name)
    {
        SceneManager.LoadScene(scene_name);
    }
}
