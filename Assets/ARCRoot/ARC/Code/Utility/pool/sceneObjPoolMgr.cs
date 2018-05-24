using UnityEngine;
using System.Collections;

public class sceneObjPoolMgr : arcGamePoolMgr<sceneObjPoolMgr> 
{
    public override void Awake()
    {
        Instance = this;             
    }
}
