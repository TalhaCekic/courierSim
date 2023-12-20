using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class spawnOrderObj : MonoBehaviour
{
    public Image[] pp;
    public Image selectedPP;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void orderSpawn()
    {
        if (OrderManager.instance.isSpawn)
        {
            int randomIndex = Random.Range(0, pp.Length);
            for (int i = 0; i < pp.Length; i++)
            {
                pp[randomIndex].gameObject.SetActive(true);
            }
        }
       
    }
    //randomize aray sistemi
    private T GetRandomElement<T>(T[] array)
    {
        if (array != null && array.Length > 0)
        {
            int randomIndex = Random.Range(0, array.Length);
            return array[randomIndex];
        }
        else
        {
            return default(T);
        }
    }
}
