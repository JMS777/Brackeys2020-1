
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public abstract class ItemUIPanel<T> : MonoBehaviour, IItemUIPanel<T> where T : IItemSystem
{
    public event Action PanelOpened;
    public event Action PanelClosed;

    public TMP_Text title;

    private T context;
    public T Context
    {
        get { return context; }
        set
        {
            if (context != null)
            {
                context.ItemsChanged -= UpdateUI;
            }

            context = value;
            
            context.ItemsChanged += UpdateUI;

            title.text = context.Name;
        }
    }

    public bool IsOpen
    {
        get
        {
            return gameObject.activeSelf;
        }
    }

    void Start()
    {
        UpdateUI();
    }

    protected abstract void UpdateUI();

    public void Open()
    {
        gameObject.SetActive(true);
        PanelOpened?.Invoke();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        PanelClosed?.Invoke();
    }
}
