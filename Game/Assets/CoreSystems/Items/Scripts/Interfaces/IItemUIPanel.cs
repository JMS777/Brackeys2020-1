using System;

public interface IItemUIPanel<T>
{
    event Action PanelOpened;
    event Action PanelClosed;

    T Context { get; set; }
    bool IsOpen { get; }
    void Open();
    void Close();
}