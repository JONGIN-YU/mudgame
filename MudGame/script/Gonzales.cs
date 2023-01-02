using System;

public class Gonzales : NPC {
  Random rand = new Random();
  public Gonzales() {
    npcName = "곤잘레스";
  }

  public override void OnNpcAction(Human human) {
    Console.WriteLine("곤잘레스와 만났습니다!");
    Console.WriteLine("");
    int randomAct = rand.Next(2);
    if (randomAct == 0) {
      OnGiveBread(human);
    } else {
      OnGiveHammer(human);
    }
  }
  public void OnGiveBread(Human human) {
    Console.WriteLine("착한 곤잘레스가 당신에게 빵을 주었습니다!");
    Console.WriteLine("체력이 50 증가합니다!");
    human.curHp += 50;
    if (human.curHp > human.fullHp) {
      human.curHp = human.fullHp;
    }
  }
  public void OnGiveHammer(Human human) {
    Console.WriteLine("착한 곤잘레스가 당신의 무기를 손봅니다!");
    int i = rand.Next(2);
    if (i == 0) {
      Console.WriteLine("공격력이 5 증가합니다!");
      human.curAtk += 5;
      human.fullAtk += 5;
    } else {
      Console.WriteLine("공격력이 5 감소합니다!");
      human.curAtk -= 5;
      human.fullAtk -= 5;
    }

  }
}