using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour
{
    void Update()
    {
        if (dayManager.instance.isdayFinished || interact.instance.isMechanic|| phoneMenu.instance.isPhoneActive&&!phoneMenu.instance.isMapActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            interact.instance.isLook = false;
        }
        else if(!dayManager.instance.isdayFinished || !interact.instance.isMechanic || !phoneMenu.instance.isPhoneActive&& phoneMenu.instance.isMapActive)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            interact.instance.isLook = true;
        }
        
    }
}
