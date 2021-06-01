using System;
using System.Collections.Generic;
using MarsRover;
using NUnit.Framework;
using Rhino.Mocks;

namespace MarsRoversTests {
  [TestFixture]
  public class Tests {

    private IConsoleInputWrapper _consoleInputWrapper;
    private UserIO _userIO;
    private Grid grid;

    [SetUp]
    public void Setup() {
      _consoleInputWrapper = MockRepository.GenerateMock<IConsoleInputWrapper>();
      _userIO = new UserIO(_consoleInputWrapper);

      grid = new Grid {
          MaxX = 5,
          MaxY = 5
      };

    }
    
    [Test]
    public void ExitWhenMaxRoversOnGrid() {
      var rovers = new List<RoverPosition>();
      var rover = new RoverPosition {
        XCoordinate = 1,
        YCoordinate = 2,
      };
      for(int i = 0; i < 25; i++) {
        rovers.Add(rover);
      }

      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("Y").Repeat.Once();
      
      var actual =  _userIO.ContinueInputRovers(grid, rovers.Count);
      var expected = false;
      Assert.AreEqual(expected, actual);
    }

    [Test]
    public void ContinueInputInvalidRoversTest() {
      var numberofRovers = 1;

      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("xdfys").Repeat.Once();

      var expected = Assert.Throws<Exception>(() => _userIO.ContinueInputRovers(grid, numberofRovers));
      var actual = "Please input Y or N.";

      Assert.AreEqual(expected.Message, actual);
    }

    [Test]
    public void ContinueInputValidRoversTest() {
      var numberofRovers = 1;

      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("Y").Repeat.Once();
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("N").Repeat.Once();

      var actual1 = _userIO.ContinueInputRovers(grid, numberofRovers);
      var actual2 = _userIO.ContinueInputRovers(grid, numberofRovers);
      var expected1 = true;
      var expected2 = false;

      Assert.AreEqual(expected1, actual1);
      Assert.AreEqual(expected2, actual2);        
    }

    [Test]
    public void InputInitalInvalidDirection() {
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("1").Repeat.Once();
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("2").Repeat.Once();
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("adfdsdfs").Repeat.Once();

      var expected = Assert.Throws<Exception>(() => _userIO.InputInitalRoverPath(grid));
      var actual = "You must input a valid direction that the rover is facing.";

      Assert.AreEqual(expected.Message, actual);
    }

    [Test]
    public void InputInitalValidDirection() {
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("1").Repeat.Once();
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("2").Repeat.Once();
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("N").Repeat.Once();

      var actualRover = _userIO.InputInitalRoverPath(grid);

      var expectedRover = new RoverPosition() {
        XCoordinate = 1,
        YCoordinate = 2,
        FacedDirection = "N"
      };

      Assert.AreEqual(expectedRover.XCoordinate, actualRover.XCoordinate);
      Assert.AreEqual(expectedRover.YCoordinate, actualRover.YCoordinate);
      Assert.AreEqual(expectedRover.FacedDirection, actualRover.FacedDirection);
    }

    [Test]
    public void StartingRoverWhereRoverAlreadyExsistsTest() {
      var rover = new RoverPosition {
        XCoordinate = 1,
        YCoordinate = 2,
        _grid = grid
      };

      int[] Coordinates = new int[] { rover.XCoordinate, rover.YCoordinate };
      grid.AddItemToGrid(Coordinates, rover.FacedDirection);

      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("1").Repeat.Once();
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("2").Repeat.Once();

      var expected = Assert.Throws<Exception>(() => _userIO.GetRoverStartingPosition(grid));
      var actual = "The Rover can not be placed at a location where another rover exists.";

      Assert.AreEqual(expected.Message, actual);
    }

    [Test]
    public void InputInvalidRoverLocationTest() {
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("dfdsdfas").Repeat.Once();
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("-5").Repeat.Once();
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("6").Repeat.Once();

      var exception1 = Assert.Throws<Exception>(() => _userIO.RoverLocation("X", grid));
      var exception2 = Assert.Throws<Exception>(() => _userIO.RoverLocation("Y", grid));
      var exception3 = Assert.Throws<Exception>(() => _userIO.RoverLocation("Y", grid));

      var actual1 = "Please input a valid integer within the grid for the X axis.";
      var actual2 = "Please input a valid integer within the grid for the Y axis.";
      var actual3 = "Please input a valid integer within the grid for the Y axis.";

      Assert.AreEqual(exception1.Message, actual1);
      Assert.AreEqual(exception2.Message, actual2);
      Assert.AreEqual(exception3.Message, actual3);
    }

    [Test]
    public void InputValidRoverLocationTest() {
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("5").Repeat.Once();
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("3").Repeat.Once();

      var actualX = _userIO.RoverLocation("X", grid);
      var actualY = _userIO.RoverLocation("Y", grid);

      var expectedX = 5;
      var expectedY = 3;

      Assert.AreEqual(actualX, expectedX);
      Assert.AreEqual(actualY, expectedY);
    }


    [Test]
    public void InputingInvalidRoverPathTest() { 
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("asdf").Repeat.Once();
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("lm132").Repeat.Once();

      var exception1 = Assert.Throws<Exception>(() => _userIO.InputRoverPath());
      var exception2 = Assert.Throws<Exception>(() => _userIO.InputRoverPath());
      var actual = "An invalid path was entered";

      Assert.AreEqual(exception1.Message, actual);
      Assert.AreEqual(exception2.Message, actual);
    }

    [Test]
    public void InputingValidRoverpathTest() {
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("lrmrlrmlr");

      var actual = _userIO.InputRoverPath();
      var expected = "LRMRLRMLR";

      Assert.AreEqual(actual, expected);
    }

    [Test]
    public void InputInvalidGridMaxTest() {
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("txcrtyf").Repeat.Once();
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("-5").Repeat.Once();

      var actual1 = Assert.Throws<Exception>(() => _userIO.InputGridMax("X"));
      var actual2 = Assert.Throws<Exception>(() => _userIO.InputGridMax("Y"));
      var expected = "You must input a valid integer greater than 0.";

      Assert.AreEqual(actual1.Message, expected);
      Assert.AreEqual(actual2.Message, expected);
    }

    [Test]
    public void InputValidGridMaxTest() {
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("5").Repeat.Once();
      _consoleInputWrapper.Expect(x => x.GetInputFromConsole()).Return("6").Repeat.Once();

      var actual1 = _userIO.InputGridMax("X");
      var actual2 = _userIO.InputGridMax("Y");
      var expected1 = 5;
      var expected2 = 6;

      Assert.AreEqual(actual1, expected1);
      Assert.AreEqual(actual2, expected2);
    }

    [Test]
    public void ExecuteRoverPathTest() {
      var rover = new RoverPosition {
        XCoordinate = 1,
        YCoordinate = 2,
        FacedDirection = "E",
        Path = "LMRMRRMMLLLMRRM",
        _grid = grid
      };
      var expectedRover = new RoverPosition {
        XCoordinate = 0,
        YCoordinate = 3,
        FacedDirection = "S",
        _grid = grid
      };

      rover.ExecuteRoverPath();

      Assert.AreEqual(expectedRover.XCoordinate, rover.XCoordinate);
      Assert.AreEqual(expectedRover.YCoordinate, rover.YCoordinate);
      Assert.AreEqual(expectedRover.FacedDirection, rover.FacedDirection);
    }

    [Test]
    public void ExecuteRoverPathTestMultipleRovers() {
      var rover = new RoverPosition {
        XCoordinate = 1,
        YCoordinate = 2,
        FacedDirection = "E",
        Path = "LMRMRRMMLLLMRRM",
        _grid = grid
      };

      var expectedRover = new RoverPosition {
        XCoordinate = 0,
        YCoordinate = 3,
        FacedDirection = "S",
        _grid = grid
      };

      rover.ExecuteRoverPath();

      Assert.AreEqual(expectedRover.XCoordinate, rover.XCoordinate, "First Rover X-Coordinate");
      Assert.AreEqual(expectedRover.YCoordinate, rover.YCoordinate, "First Rover Y-Coordinate");
      Assert.AreEqual(expectedRover.FacedDirection, rover.FacedDirection, "First Rover Direction");

      int[] Coordinates = new int[] { rover.XCoordinate, rover.YCoordinate };
      grid.AddItemToGrid(Coordinates, rover.FacedDirection);

      rover = new RoverPosition {
        XCoordinate = 1,
        YCoordinate = 3,
        FacedDirection = "S",
        Path = "LRMMRM",
        _grid = grid
      };

      rover.ExecuteRoverPath();

      expectedRover = new RoverPosition {
        XCoordinate = 0,
        YCoordinate = 1,
        FacedDirection = "W",
        _grid = grid
      };

      Assert.AreEqual(expectedRover.XCoordinate, rover.XCoordinate, "Second Rover X-Coordinate");
      Assert.AreEqual(expectedRover.YCoordinate, rover.YCoordinate, "Second Rover Y-Coordinate");
      Assert.AreEqual(expectedRover.FacedDirection, rover.FacedDirection, "Second Rover Direction");

      Coordinates = new int[] { rover.XCoordinate, rover.YCoordinate };
      grid.AddItemToGrid(Coordinates, rover.FacedDirection);

      rover = new RoverPosition {
        XCoordinate = 4,
        YCoordinate = 4,
        FacedDirection = "W",
        Path = "MMRMM",
        _grid = grid
      };

      rover.ExecuteRoverPath();

      expectedRover = new RoverPosition {
        XCoordinate = 2,
        YCoordinate = 5,
        FacedDirection = "N",
        _grid = grid
      };

      Assert.AreEqual(expectedRover.XCoordinate, rover.XCoordinate, "Third Rover X-Coordinate");
      Assert.AreEqual(expectedRover.YCoordinate, rover.YCoordinate, "Third Rover Y-Coordinate");
      Assert.AreEqual(expectedRover.FacedDirection, rover.FacedDirection, "Third Rover Direction");

      Coordinates = new int[] { rover.XCoordinate, rover.YCoordinate };
      grid.AddItemToGrid(Coordinates, rover.FacedDirection);

      rover = new RoverPosition(grid) {
        XCoordinate = 2,
        YCoordinate = 3,
        FacedDirection = "S",
        Path = "MRLLLMMM",
      };

      rover.ExecuteRoverPath();

      expectedRover = new RoverPosition {
        XCoordinate = 2,
        YCoordinate = 4,
        FacedDirection = "N",
        _grid = grid
      };

      Assert.AreEqual(expectedRover.XCoordinate, rover.XCoordinate, "Fourth Rover X-Coordinate");
      Assert.AreEqual(expectedRover.YCoordinate, rover.YCoordinate, "Fourth Rover Y-Coordinate");
      Assert.AreEqual(expectedRover.FacedDirection, rover.FacedDirection, "Fourth Rover Direction");
    }
  }
}