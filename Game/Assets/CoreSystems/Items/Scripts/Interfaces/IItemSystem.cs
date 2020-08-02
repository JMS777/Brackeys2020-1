using System;

public interface IItemSystem
{
    event Action ItemsChanged;

    string Name { get; }
}