using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Launch : MonoBehaviour
{
    public List<GameObject> uiObjectsToDisable;

    // Start is called before the first frame update
    void Start()
    {
        DisableAllUIObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisableAllUIObjects()
    {
        foreach(GameObject obj in uiObjectsToDisable)
        {
            obj.SetActive(false);
        }
    }
}
