using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _startText;      // ������ � �������� "������� ���"
    [SerializeField] private GameObject _finishPanel;    // ������ � ������� "������������� � ���������"

    public void ShowStartText(bool show)
    {
        _startText.SetActive(show);
    }

    public void ShowFinishPanel(bool show)
    {
        _finishPanel.SetActive(show);
    }
}

