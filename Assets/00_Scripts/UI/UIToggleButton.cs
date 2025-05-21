using UnityEngine;
using UnityEngine.UI;

public class UIToggleButton : MonoBehaviour
{
        private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClickToggleUI);
    }

    private void OnClickToggleUI()
    {

        GameManager.Instance.TogglePopup();
    }
}

