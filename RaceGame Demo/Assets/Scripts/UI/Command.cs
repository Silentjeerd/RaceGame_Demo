using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class that commands inherit.
/// </summary>
public abstract class Command
{
    public abstract void Execute();
    public abstract void UnExecute();
}

/// <summary>
/// 
/// </summary>
public class MenuSwapCommand : Command
{
    GameObject previousMenu;
    GameObject menu;

    /// <summary>
    /// The constructor for the command.
    /// </summary>
    /// <param name="previousMenu"></param> The menu that has to be closed.
    /// <param name="menu"></param> The menu that has to be opened.
    public MenuSwapCommand(GameObject previousMenu, GameObject menu)
    {
        this.previousMenu = previousMenu;
        this.menu = menu;
    }

    /// <summary>
    /// Executes the command making you go to the next menu.
    /// </summary>
    public override void Execute()
    {
        previousMenu.SetActive(false);
        menu.SetActive(true);
    }

    /// <summary>
    /// Unexecutes the command making you go to the previous menu.
    /// </summary>
    public override void UnExecute()
    {
        previousMenu.SetActive(true);
        menu.SetActive(false);
    }
}