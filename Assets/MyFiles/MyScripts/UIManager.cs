using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _startText;      // Панель с надписью "Нажмите газ"
    [SerializeField] private GameObject _finishPanel;    // Панель с кнопкой "Соревноваться с призраком"

    public void ShowStartText(bool show)
    {
        _startText.SetActive(show);
    }

    public void ShowFinishPanel(bool show)
    {
        _finishPanel.SetActive(show);
    }
}

