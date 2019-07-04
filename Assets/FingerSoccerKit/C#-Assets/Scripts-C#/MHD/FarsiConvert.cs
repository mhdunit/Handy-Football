using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarsiConvert : MonoBehaviour
{
    public TextMesh[] ConvertedText;

   void Start()
    {
        for (int i = 0; i < ConvertedText.Length; i++)
        {
            ConvertedText[i].text = ConvertedText[i].text.faConvert();
        }
    }
}
