using System;

public class Warrior : Hero {
  public Warrior() {
    this.className = "전사";
    this.lev = 1;
    this.fullHp = 100;
    this.fullAtk = 10;
    this.curHp = 100;
    this.curAtk = 10;
  }
  public override void OnUseSkill(Human human) {
    Console.WriteLine("전사스킬사용!");
  }
}
