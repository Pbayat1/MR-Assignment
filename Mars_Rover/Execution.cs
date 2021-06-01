using System;

namespace MarsRover {
  public class Execution {
    public void ExecuteRover() {
      var grid = new Grid();
      grid.InputGridCoordinates();
      var moreRovers = true;
      var rover = new RoverPosition(grid);
      var userIO = new UserIO();

      while (moreRovers) {
        userIO.InputRoverLocationAndDirection(ref rover, grid);
        userIO.InputRoverPathLoop(ref rover);
        var collision = rover.ExecuteRoverPath();
        userIO.DisplayRoverLocation(rover, collision);
        int[] Coordinates = new int[] { rover.XCoordinate, rover.YCoordinate };
        grid.AddItemToGrid(Coordinates, rover.FacedDirection);
        moreRovers = userIO.GetMoreRoversChoice(grid);        
      }

      Console.WriteLine("======================================================================");
      userIO.PrintRovers(grid);
      Console.WriteLine("Press any key to exit.");
      Console.ReadKey();

    }
  }
}
