using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFiledConvert : MonoBehaviour
{
    public Text TextReal;
    public Text TextFake;


    private void FixedUpdate()
    {
        TextReal.text = TextFake.text.faConvert();

    }
}
