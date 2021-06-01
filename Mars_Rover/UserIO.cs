using System;

namespace MarsRover { 
  public class UserIO {

    private readonly IConsoleInputWrapper _consoleInputWrapper;

    public UserIO(IConsoleInputWrapper consoleInputWrapper) {
      _consoleInputWrapper = consoleInputWrapper;
    }

    public UserIO() : this(new ConsoleInputWrapper()) { }

    public bool GetMoreRoversChoice(Grid grid) {
      var loopHandler = true;
      int numberOfRovers = grid.ItemsOnGridCoordinates.Count;

      while (loopHandler) {
        try {
          loopHandler = false;
          return ContinueInputRovers(grid, numberOfRovers);
        }
        catch (Exception e) {
          Console.WriteLine(e.Message);
          loopHandler = true;
        }
      }

      return false;
    }

    public int InputGridMax(string axis) {
      Console.WriteLine("Please input the largest {0}-Coordinate value for the grid", axis);
      int max;

      if (!(int.TryParse(_consoleInputWrapper.GetInputFromConsole(), out max)) || max < 1) {
        throw new Exception("You must input a valid integer greater than 0.");
      }

      return max;
    }

    public string InputRoverPath() {
      Console.WriteLine("Please enter the path you would like the Rover to take using the letters L, R, and M.");
      var roverPathInput = _consoleInputWrapper.GetInputFromConsole().ToUpper();
      var allowableLetters = "LMR";
      var illegalLetters = false;

      foreach (var letter in roverPathInput) {
        illegalLetters = (!allowableLetters.Contains(letter.ToString())) ? true : false;        
      }

      if (illegalLetters) {          
        throw new Exception("An invalid path was entered");          
      }

      return roverPathInput;
    }
    

    public int RoverLocation(string axis, Grid grid) {
      Console.WriteLine("Please input {0} location of the rover", axis);
      int location;
      var Maximum = (axis == "X") ? grid.MaxX : grid.MaxY;

      if (!(int.TryParse(_consoleInputWrapper.GetInputFromConsole(), out location)) || location < Grid.GRID_MINIMUM || location > Maximum) {
        throw new Exception("Please input a valid integer within the grid for the " + axis + " axis.");
      }

      return location;
    }

    public RoverPosition InputInitalRoverPath(Grid grid) {        
      var rover = GetRoverStartingPosition(grid);
      Console.WriteLine("Please input the direction that the rover is facing. [N, S, E, W]");
      var roverDirectionHolder = _consoleInputWrapper.GetInputFromConsole().ToUpper();

      if (!(roverDirectionHolder == "N" || roverDirectionHolder == "W" || roverDirectionHolder == "E" || roverDirectionHolder == "S")) {
        throw new Exception( "You must input a valid direction that the rover is facing.");
      }

      rover.FacedDirection = roverDirectionHolder;
      return rover;
    }

    public void PrintRovers(Grid grid) {
      var Xvalue = 0;
      var Yvalue = 1;

      Console.WriteLine("List of the Rovers on the grid.");

      for (int i = 0; i < grid.ItemsOnGridCoordinates.Count; i++) {
        Console.WriteLine("Rover {0}: {1} {2} {3}", i + 1, grid.ItemsOnGridCoordinates[i][Xvalue], grid.ItemsOnGridCoordinates[i][Yvalue], grid.ItemsOnGridDirection[i]);
      }
    }

    public bool ContinueInputRovers(Grid grid, int numberOfRovers) {
      var userChoice = "N";

      if (grid.MaxX * grid.MaxY > numberOfRovers ) {
        Console.WriteLine("Would you like to enter more Rovers? 'Y' for yes, 'N' for no.");
        userChoice = _consoleInputWrapper.GetInputFromConsole().ToUpper();

        if (!(userChoice == "Y" || userChoice == "N")) {
          throw new Exception("Please input Y or N.");
        }
      }

      return (userChoice == "Y") ? true : false;
    }

    public void DisplayRoverLocation(RoverPosition currentRover, bool collision) {
      if (collision) {
        Console.WriteLine("Rover was stopped before hitting an obstacle.");
      }

      Console.WriteLine("The rover has moved to: {0} {1} {2}", currentRover.XCoordinate, currentRover.YCoordinate, currentRover.FacedDirection);
    }

    public void InputRoverLocationAndDirection(ref RoverPosition roverInstruction, Grid grid) {
      var loopHandler = true;

      while (loopHandler) {
        try {
          loopHandler = false;
          roverInstruction = InputInitalRoverPath(grid);
        }
        catch (Exception e) {
          Console.WriteLine(e.Message);
          loopHandler = true;
        }
      }

    }

    public void InputRoverPathLoop(ref RoverPosition roverInstruction) {
      var loopHandler = true;

      while (loopHandler) {
        try {
          loopHandler = false;
          roverInstruction.Path = InputRoverPath();
        }
        catch (Exception e) {
          Console.WriteLine(e.Message);
          loopHandler = true;

        }
      }

    }

    public RoverPosition GetRoverStartingPosition(Grid grid) { 
      var currentRover = new RoverPosition(grid) {
        XCoordinate = RoverLocation("X", grid),
        YCoordinate = RoverLocation("Y", grid)
      };
      
      var sameStartingPosition = grid.CheckIfItemExsistsAtLocation(currentRover.XCoordinate, currentRover.YCoordinate);

      if (sameStartingPosition) {
        throw new Exception("The Rover can not be placed at a location where another rover exists.");
      }

      return currentRover;
    }

  }
}
