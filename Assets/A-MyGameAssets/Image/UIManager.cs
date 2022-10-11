using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public RectTransform img;

    private void Awake()
    {
        
    }
    private void Start()
    {
        img.localScale = Vector2.zero;
    }
    public void ExpandX()
    {

        img.localScale = Vector2.one;
    }
    public void ShrinkX()
    {
        img.localScale = Vector2.zero;
    }

    public void ExpandXShrink()
    {

        if (img.localScale.x == 0)
        {
            img.localScale = Vector3.one;
            Time.timeScale = 0;
            
        }
        else
        {
            img.localScale = Vector3.zero;

            Time.timeScale = 1;
        }

    }
    public void replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void GoHome()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

}
