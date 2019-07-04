using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertTextMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMesh>().text = GetComponent<TextMesh>().text.faConvert();
    }

    
}
