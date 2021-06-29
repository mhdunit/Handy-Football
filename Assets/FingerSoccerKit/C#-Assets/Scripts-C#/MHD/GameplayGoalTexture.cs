using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using I2.Loc;

public class GameplayGoalTexture : MonoBehaviour
{
    public Texture2D Goal_English, Goal_Persian;
    // Start is called before the first frame update
    void Start()
    {
        SetGoalTexture();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGoalTexture()
    {
        if (LocalizationManager.CurrentLanguage == "English")
            GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex",Goal_English);
        else
            GetComponent<MeshRenderer>().materials[0].SetTexture("_MainTex", Goal_Persian);
    }
}
