using System.Collections.Generic;
using System.Numerics;
using System.Net;
using System;

//아이템 딕셔너리에서 그냥 리스트로 변경 
public class GameManager {
  enum GameState {
    START,
    MOVE,
    FIGHT
  }

  int currentState = (int)GameState.START;
  string userName = "";




  public List<Hero> heros = new List<Hero>();
  public List<Villain> enemies = new List<Villain>();
  public List<NPC> npc = new List<NPC>();
  public List<Item> fieldItems = new List<Item>();


  public Dictionary<Vector2, Villain> saveEnemies = new Dictionary<Vector2, Villain>();
  public int heroClass = 0;
  public Villain currentEnemy = null;


  private MapManager mapManager;

  private Random rand = new Random();

  public void InitializeGame() {
    mapManager = new MapManager();
    mapManager.InitializeMap();

    Warrior warrior = new Warrior();
    heros.Add(warrior);

    Demon demon = new Demon();
    enemies.Add(demon);

    Hammer hammer = new Hammer();
    Bread bread = new Bread();
    fieldItems.Add(hammer);
    fieldItems.Add(bread);

    Gonzales gonzales = new Gonzales();
    npc.Add(gonzales);

    _OnCreatePlayer();

    mapManager.OnDrawMap(heros[heroClass].point);
  }

  public void StartGame() {
    // _OnCreateEnemy();
    while (true) {

      if (currentState == (int)GameState.START) {
        InitializeGame();
      } else if (currentState == (int)GameState.MOVE) {
        _OnChooseAction();
      } else if (currentState == (int)GameState.FIGHT) {
        _OnChooseFightAction();
      }
    }

  }

  private void _OnCheckRoom() {
    int x = (int)heros[heroClass].point.X;
    int y = (int)heros[heroClass].point.Y;
    if (mapManager.map[y, x] == (int)MapManager.MapValue.ENEMY) {

      foreach (Vector2 vec in saveEnemies.Keys) {
        if (heros[heroClass].point == vec) {
          currentEnemy = saveEnemies[vec];
          if (currentEnemy.isDead) {
            return;
          }
        }
      }
      if (currentEnemy == null) {
        int i = rand.Next(enemies.Count);
        Villain newEnemy = enemies[i].OnDeepCopy();
        newEnemy.OnSetStatus();
        Vector2 vec = new Vector2(heros[heroClass].point.X, heros[heroClass].point.Y);
        saveEnemies.Add(vec, newEnemy);
        currentEnemy = newEnemy;
      }
      currentState = (int)GameState.FIGHT;
      Console.WriteLine("------------------------------------------");
      Console.WriteLine("적을 만났다!");
      Console.Write(currentEnemy.name + "  레벨: " + currentEnemy.lev);
      Console.Write("  체력: " + currentEnemy.curHp);
      Console.WriteLine("  공격력: " + currentEnemy.curAtk);
      Console.WriteLine(" ");

    } else if (mapManager.map[y, x] == (int)MapManager.MapValue.ITEM) {

      int i = rand.Next(fieldItems.Count);
      fieldItems[i].OnEffect(heros[heroClass]);
      mapManager.map[y, x] = (int)MapManager.MapValue.EMPTY;

    } else if (mapManager.map[y, x] == (int)MapManager.MapValue.NPC) {
      int i = rand.Next(npc.Count);
      npc[i].OnNpcAction(heros[heroClass]);
      mapManager.map[y, x] = (int)MapManager.MapValue.EMPTY;
    }
  }

  private void _OnChooseAction() {

    bool isCheckAnswer = false;
    while (!isCheckAnswer) {
      Console.WriteLine("무엇을 하시겠어요?");
      Console.WriteLine("이동");
      Console.WriteLine("귀환");
      string todo = Console.ReadLine();
      if (todo == "이동") {

        heros[heroClass].OnMove();
        isCheckAnswer = true;
        mapManager.OnDrawMap(heros[heroClass].point);
        _OnCheckRoom();

      } else if (todo == "귀환") {
        heros[heroClass].OnReturn();
        mapManager.OnDrawMap(heros[heroClass].point);
      }
    }
  }

  private void _OnChooseFightAction() {
    bool isCheckAnswer = false;
    while (!isCheckAnswer) {
      Console.WriteLine("내 정보");
      Console.Write(userName + "  레벨: " + heros[heroClass].lev);
      Console.Write("  체력: " + heros[heroClass].curHp);
      Console.Write("  공격력: " + heros[heroClass].curAtk);
      Console.WriteLine("  경험치: " + heros[heroClass].curExp + "/" + heros[heroClass].maxExp);
      Console.WriteLine("무엇을 하시겠어요?");
      Console.WriteLine("공격");
      Console.WriteLine("도망");
      string todo = Console.ReadLine();
      if (todo == "공격") {
        isCheckAnswer = true;
        heros[heroClass].OnAttack(currentEnemy);

        Console.WriteLine("적을 공격했다!");
        Console.WriteLine("적 남은 체력: " + currentEnemy.curHp);
        if (_OnCheckDead(currentEnemy)) {
          Console.WriteLine("적을 처치했다!");
          heros[heroClass].OnGetExp(currentEnemy);
          currentEnemy.timer.Start();
          mapManager.OnDrawMap(heros[heroClass].point);
          currentState = (int)GameState.MOVE;
          currentEnemy = null;
        } else {
          currentEnemy.OnAttack(heros[heroClass]);
          Console.WriteLine(" ");
          Console.WriteLine("적이 나를 공격했다!");
          Console.WriteLine(" ");
        }

      } else if (todo == "도망") {
        isCheckAnswer = true;
        int runCoin = heros[heroClass].OnRun();
        if (runCoin != 0) {
          //성공
          mapManager.OnDrawMap(heros[heroClass].point);
          currentEnemy = null;
          currentState = (int)GameState.MOVE;
          _OnCheckRoom();
        } else {
          currentEnemy.OnAttack(heros[heroClass]);
          Console.WriteLine(" ");
          Console.WriteLine("적이 나를 공격했다!");
          Console.WriteLine(" ");
        }

      }


      if (_OnCheckDead(heros[heroClass])) {
        Console.WriteLine("적의 공격에 맞아 죽었습니다!");
        Console.WriteLine(" ");
        heros[heroClass].OnReturn();
        currentState = (int)GameState.MOVE;
        mapManager.OnDrawMap(heros[heroClass].point);
        isCheckAnswer = true;
        currentEnemy = null;
      }
    }
  }

  private bool _OnCheckDead(Human human) {
    if (human.curHp <= 0) {
      human.curHp = 0;
      human.isDead = true;
    }
    return human.isDead;
  }

  private void _OnCreatePlayer() {
    bool isCheckPlayer = false;
    while (!isCheckPlayer) {
      Console.WriteLine("안녕하세요!");
      Console.WriteLine("이름이 무엇입니까?");
      userName = Console.ReadLine();

      bool isCheckClass = false;
      while (!isCheckClass) {
        Console.WriteLine("당신의 직업은 무엇입니까? (숫자로 선택)");
        Console.Write("선택지: ");
        for (int i = 0; i < heros.Count; i++) {
          Console.Write(" " + i + $". {heros[i].className} ");
        }
        Console.WriteLine(" ");
        if (Int32.TryParse(Console.ReadLine(), out heroClass)) {
          if (heroClass < heros.Count) {
            isCheckClass = true;
          }
        }
      }
      Console.WriteLine("이름: " + userName);
      Console.WriteLine("직업: " + heros[heroClass].className);
      bool isCheckRightAnswer = false;
      while (!isCheckRightAnswer) {
        Console.WriteLine("맞습니까? y/n");
        string check = Console.ReadLine();
        if (check == "y") {
          isCheckPlayer = true;
          isCheckRightAnswer = true;
          heros[heroClass].name = userName;
          heros[heroClass].point = new Vector2(10, 10);
          currentState = (int)GameState.MOVE;
        } else if (check == "n") {
          isCheckPlayer = false;
          isCheckRightAnswer = true;
        }
      }
    }
  }


}