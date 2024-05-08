using System.Collections;
using System.Collections.Generic;
using TbsFramework.Grid;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            FindObjectOfType<CellGrid>().EndTurn();
        }
    }
}
