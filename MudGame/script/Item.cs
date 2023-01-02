using System;
using System.Collections.Generic;

public abstract class Item {
  public string name;
  public abstract void OnEffect(Human human);

}

public class Bread : Item {

  public Bread() {
    name = "빵";
  }
  public override void OnEffect(Human human) {
    human.curHp += 50;
    if (human.curHp > human.fullHp) {
      human.curHp = human.fullHp;
    }

    Console.WriteLine("빵을 먹었습니다!");
    Console.WriteLine("체력이" + 50 + "만큼 증가합니다");
    Console.WriteLine(" ");
  }

}

public class Hammer : Item {
  public Hammer() {
    name = "망치";
  }
  Random rand = new Random();
  public override void OnEffect(Human human) {
    int increaseAtk = rand.Next(1, 11);
    human.fullAtk += increaseAtk;
    human.curAtk = human.fullAtk;

    Console.WriteLine("신비로운 망치를 주웠습니다!");
    Console.WriteLine("공격력이 " + increaseAtk + "만큼 증가합니다");
    Console.WriteLine(" ");
  }

}