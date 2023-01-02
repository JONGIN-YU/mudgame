using System;
using System.Numerics;

public class MapManager {
  public enum MapValue {
    EMPTY,
    PLAYER,
    ENEMY,
    NPC,
    ITEM
  }
  public int mapColumn = 21;
  public int mapRow = 21;
  public int[,] map;

  public void InitializeMap() {
    Random rand = new Random();

    map = new int[mapRow, mapColumn];

    for (int i = 0; i < mapColumn; i++) {
      for (int j = 0; j < mapRow; j++) {
        map[i, j] = rand.Next(2, 5);
      }
    }
    map[10, 10] = (int)MapValue.EMPTY;
  }

  public void OnDrawMap(Vector2 point) {
    for (int i = 0; i < mapColumn; i++) {
      for (int j = 0; j < mapRow; j++) {
        if (map[i, j] == (int)MapValue.ENEMY) {
          Console.ForegroundColor = ConsoleColor.Red;
        } else if (map[i, j] == (int)MapValue.NPC) {
          Console.ForegroundColor = ConsoleColor.DarkBlue;
        } else if (map[i, j] == (int)MapValue.ITEM) {
          Console.ForegroundColor = ConsoleColor.DarkYellow;
        } else {
          Console.ForegroundColor = ConsoleColor.White;
        }

        if (i == (int)point.Y && j == (int)point.X) {
          Console.ForegroundColor = ConsoleColor.Green;
          Console.Write(" " + (int)MapValue.PLAYER);
        } else {
          //맵그리기;
          if (j == mapRow - 1) {
            Console.WriteLine(" " + map[i, j]);
          } else {
            Console.Write(" " + map[i, j]);
          }
        }
      }
    }
    Console.ForegroundColor = ConsoleColor.White;
  }
}

