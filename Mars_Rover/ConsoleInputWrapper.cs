using System;

namespace MarsRover {
  public class ConsoleInputWrapper : IConsoleInputWrapper {
    public string GetInputFromConsole() {
      return Console.ReadLine();
    }
  }
}
