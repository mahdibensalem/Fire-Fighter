using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class NextLevelButton : MonoBehaviour
{
    Button m_button;
    void Start()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;
       if ((activeScene) >= (PlayerPrefs.GetInt("LVL")))
                            PlayerPrefs.SetInt("LVL", SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.LoadScene(activeScene + 1);
    }
}