using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHelper : MonoBehaviour
{
    public string audioName;

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        // button.onClick.AddListener(() => AudioManager.Get().PlayFromPool(audioName));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
