using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartBtn : MonoBehaviour
{
    public GameObject infoUI;
    // Start is called before the first frame update

    void Start()
    {
        infoUI.SetActive(false);
    }

    [SerializeField] private GameObject canvas;
  
    public void OffLobbyUI()
    {
        canvas.SetActive(false);
        infoUI.SetActive(true);
    }
    public void OnLobbyUI()
    {
        canvas.SetActive(true);
    }
}
