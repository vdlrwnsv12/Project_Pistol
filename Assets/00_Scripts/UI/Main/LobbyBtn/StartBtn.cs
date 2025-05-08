using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartBtn : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject canvas;
  
    public void OffLobbyUI()
    {
        canvas.SetActive(false);
    }
    public void OnLobbyUI()
    {
        canvas.SetActive(true);
    }
}
