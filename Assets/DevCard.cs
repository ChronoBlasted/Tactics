using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevCard : MonoBehaviour
{
    [SerializeField] string Url;
    public void HandleOnClick()
    {
        Application.OpenURL(Url);
    }
}
