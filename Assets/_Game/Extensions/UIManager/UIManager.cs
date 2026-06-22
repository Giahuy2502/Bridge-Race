using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    Dictionary<System.Type,UICanvas> canvasActivates = new Dictionary<System.Type, UICanvas>();
    Dictionary<System.Type,UICanvas> canvasPrefabs = new Dictionary<System.Type, UICanvas>();
    [SerializeField] Transform parent;

    private void Awake()
    {
        // load UI prefabs tu resources
        UICanvas[] canvas = Resources.LoadAll<UICanvas>("ui/");
        for (int i = 0; i < canvas.Length; i++)
        {
            canvasPrefabs.Add(canvas[i].GetType(),canvas[i]);
        }
    }

    // mo canvas
    public T Open<T>() where T : UICanvas
    {
        T canvas = GetUI<T>();
        canvas.Setup();
        canvas.Open();
        return canvas;
    }
    
    // dong canvas sau bn giay
    public void CloseUI<T>(float time) where T : UICanvas
    {
        if (IsLoaded<T>())
        {
            canvasActivates[typeof(T)].Close(time);
        }
    }
    
    // dong canvas truc tiep
    public void CloseDirectly<T>() where T : UICanvas
    {
        if (IsLoaded<T>())
        {
            canvasActivates[typeof(T)].CloseDirectly();
        }
    }

    
    // kiem tra canvas duoc tao hay chua
    public bool IsLoaded<T>() where T : UICanvas
    {
        return canvasActivates.ContainsKey(typeof(T)) && canvasActivates[typeof(T)] != null;
    }
    
    // kiem tra canvas dc activate hay chua
    public bool IsOpened<T>() where T : UICanvas
    {
        return IsLoaded<T>() && canvasActivates[typeof(T)].gameObject.activeSelf;
    }

    
    // lay activate canvas
    public T GetUI<T>() where T : UICanvas
    {
        if (!IsLoaded<T>())
        {
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab,parent);
            canvasActivates[typeof(T)] = canvas;
        }
        return canvasActivates[typeof(T)] as T;
    }

    // get prefab 
    private T GetUIPrefab<T>() where T : UICanvas
    {
        return canvasPrefabs[typeof(T)] as T;
    }
    
    // dong tat ca
    public void ClodeAll()
    {
        foreach (var canvas in canvasActivates)
        {
            if (canvas.Value != null && canvas.Value.gameObject.activeSelf)
            {
                canvas.Value.Close(0);
            }
        }
    }
}
