using System;
using System.Collections.Generic;

namespace MarsRover {
  public class Grid {
    public int MaxX { get; set; } = 0;
    public int MaxY { get; set; } = 0;
    public const int GRID_MINIMUM  = 0;
    public List<int[]> ItemsOnGridCoordinates = new List<int[]>();
    public List<string> ItemsOnGridDirection = new List<string>();

    public void AddItemToGrid(int[] Coordinates, string facedDirection) {
      var x = 0;
      var y = 1;
      int[] newItem = new int[]{ Coordinates[x], Coordinates[y] };
      ItemsOnGridDirection.Add(facedDirection);
      ItemsOnGridCoordinates.Add(newItem);
    }

    public bool CheckIfWithinGrid(int XCoordinate, int YCoordinate) {
      var isWithinGrid = (XCoordinate > MaxX || YCoordinate > MaxY || XCoordinate < Grid.GRID_MINIMUM || YCoordinate < Grid.GRID_MINIMUM) ? false : true;
      return isWithinGrid;
    }

    public bool CheckIfItemExsistsAtLocation(int XCoordinate, int YCoordinate) {
      var itemExsists = false;
      var Xvalue = 0;
      var Yvalue = 1;

      for (int i = 0; i < ItemsOnGridCoordinates.Count; i++) {
        if (((ItemsOnGridCoordinates[i][Xvalue] == XCoordinate) && (ItemsOnGridCoordinates[i][Yvalue] == YCoordinate))) {
          itemExsists = true;
          return itemExsists;
        }
      }

      return itemExsists;
    }

    public bool ValidateThatItemCanBeMovedToThisLocation(int XCoordinate, int YCoordinate) {
      var canBeMoved = true;

      if (!CheckIfWithinGrid(XCoordinate, YCoordinate)) {
        canBeMoved = false;
        return canBeMoved;
      }

      if (CheckIfItemExsistsAtLocation(XCoordinate, YCoordinate)) {
        canBeMoved = false;
        return canBeMoved;
      }

      return canBeMoved;
    }

    public void InputGridCoordinates() {  
      var userIO = new UserIO();
      var loopHandler = true;

      while (loopHandler) {
        try {
          MaxX = userIO.InputGridMax("X");
          MaxY = userIO.InputGridMax("Y");
          loopHandler = false;
        }
        catch (Exception e) {
          Console.WriteLine(e.Message);
          loopHandler = true;
        }
      }

    }

  }
}
