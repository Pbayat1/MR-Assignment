namespace MarsRover {
  public class RoverPosition {
    public int XCoordinate { get; set; }
    public int YCoordinate { get; set; }
    public string FacedDirection { get; set; }
    public string Path { get; set; } = string.Empty;
    public Grid _grid { get; set; }

    public RoverPosition(Grid grid) {
      _grid = grid;
    }

    public RoverPosition() { }

    public bool ExecuteRoverPath() {
      for (int i = 0; i < Path.Length; i++) {
        switch (Path[i]) {
          case 'L':
            TurnLeft();
            break;

          case 'R':
            TurnRight();
            break;

          case 'M':
            if (!FindDirectionThenMoveRover()) { //FindDirectionThenMoveRover() will return false if the rover runs into an obstacle
              return true;
            }
            break;

          default:
            break;
        }
      }
      return false;
    }

    private bool FindDirectionThenMoveRover() { 
      switch (FacedDirection) {
        case "N":
          return MoveRover(0, 1);

        case "E":
          return MoveRover(1, 0);

        case "S":
          return MoveRover(0, -1);

        case "W":
          return MoveRover(-1, 0);

        default:
          return false;
      }
    }

    private bool MoveRover(int XChange, int YChange) {  //check if rover move is valid. if valid, then move
      var newPositionY = YCoordinate + YChange;
      var newPositionX = XCoordinate + XChange;

      if (_grid.ValidateThatItemCanBeMovedToThisLocation(newPositionX, newPositionY)) {
        XCoordinate = newPositionX;
        YCoordinate = newPositionY;
        return true;
      }

      return false;
    }

    private void TurnLeft() {
      switch (FacedDirection) {
        case "N":
          FacedDirection = "W";
          return;

        case "E":
          FacedDirection = "N";
          return;

        case "S":
          FacedDirection = "E";
          return;

        case "W":
          FacedDirection = "S";
          return;
      }
      return;
    }

    private void TurnRight() {
      switch (FacedDirection) {
        case "N":
          FacedDirection = "E";
          return;

        case "E":
          FacedDirection = "S";
          return;

        case "S":
          FacedDirection = "W";
          return;

        case "W":
          FacedDirection = "N";
          return;
      }
      return;
    }
  }

}
