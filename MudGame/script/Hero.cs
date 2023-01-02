using System.Numerics;
using System;
using System.Collections.Generic;

public abstract class Hero : Human, ISkill {
  public String className = "";
  public int curExp = 0;
  public int maxExp = 20;
  public Vector2 point = new Vector2();
  public Vector2 initialPos = new Vector2(10, 10);
  private Dictionary<string, int> directions = new Dictionary<string, int>();

  private Random rand = new Random();
  public void OnMove() {
    Console.WriteLine("어디로 이동하시겠습니까?");

    //갈 수 있는 방향 확인
    _OnCheckDirection();

    Console.Write("선택지: ");
    string dir = String.Join(" ", directions.Keys.ToArray());
    Console.WriteLine(dir);

    bool isCheckAnswer = false;

    while (!isCheckAnswer) {
      string direction = Console.ReadLine();

      foreach (string key in directions.Keys) {
        if (direction == key) {
          if (key == "동" || key == "서") {
            point.X += directions[key];
          } else {
            point.Y += directions[key];
          }
          isCheckAnswer = true;
        }
      }
    }
  }
  public int OnRun() {
    int runCoin = rand.Next(2);

    if (runCoin == 0) {
      Console.WriteLine("도망치기에 실패했습니다!");
    } else {
      _OnCheckDirection();
      List<string> pickableDir = new List<string>();
      foreach (string key in directions.Keys) {
        pickableDir.Add(key);
      }
      int randDir = rand.Next(directions.Count);

      for (int i = 0; i < pickableDir.Count; i++) {
        if (randDir == i) {
          if (pickableDir[i] == "동" || pickableDir[i] == "서") {
            point.X += directions[pickableDir[i]];
          } else {
            point.Y += directions[pickableDir[i]];
          }
        }
      }
      Console.WriteLine("무사히 도망쳤습니다!");
    }
    return runCoin;
  }
  public void OnReturn() {

    if (isDead) {
      _OnRecovery();
      isDead = false;
      Console.Write("당신은 부활하여 ");
    }
    Console.WriteLine("광장으로 귀환했습니다");
    point = initialPos;
  }
  public void OnLevelUp() {
    lev++;
    fullHp = fullHp * 2;
    fullAtk = fullAtk * 2;
    curHp = fullHp;
    curAtk = fullAtk;
  }
  public void OnGetExp(Villain villain) {
    Console.WriteLine(villain.givingExp + "만큼 경험치를 얻었다!");
    curExp += villain.givingExp;
    if (curExp >= maxExp) {
      curExp -= maxExp;
      maxExp = (int)(maxExp * 1.5);
      OnLevelUp();
    }
  }

  private void _OnRecovery() {
    curHp = fullHp;
    curAtk = fullAtk;
  }

  private void _OnCheckDirection() {
    directions.Clear();
    if (point.X != 20) {
      directions.Add("동", 1);
    }
    if (point.X != 0) {
      directions.Add("서", -1);
    }
    if (point.Y != 20) {
      directions.Add("남", 1);
    }
    if (point.Y != 0) {
      directions.Add("북", -1);
    }
  }

  public abstract void OnUseSkill(Human human);
}

