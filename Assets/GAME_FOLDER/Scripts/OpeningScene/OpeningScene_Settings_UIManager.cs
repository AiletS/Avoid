using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class OpeningScene_Settings_UIManager : MonoBehaviour
{
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;
    [SerializeField] TextMeshProUGUI BGMText;
    [SerializeField] TextMeshProUGUI SEText;
    SaveData data;

    private void Start()
    {
        data = GetComponent<SaveLoadManager>().data;
        BGMSlider.value = data.BGMVolume;
        SESlider.value = data.SEVolume;
        BGMText.text = "" + (int)(data.BGMVolume * 100);
        SEText.text = "" + (int)(data.SEVolume * 100);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) load_scene("OpeningScene_Main");
    }

    public void load_scene(string scene_name)
    {
        GetComponent<SaveLoadManager>().Save(data);
        SceneManager.LoadScene(scene_name);
    }

    public void change_BGMVolume()
    {
        data.BGMVolume = BGMSlider.value;
        BGMText.text = "" + (int)(data.BGMVolume * 100);
    }

    public void change_SEVolume()
    {
        data.SEVolume = SESlider.value;
        SEText.text = "" + (int)(data.SEVolume * 100);
    }
}
