using System;

public class Demon : Villain {
  public Demon() {
    name = "악마";
    maxLevel = 10;
    minLevel = 1;
    multiAtk = 10;
    multiHp = 30;
    multiExp = 10;

    respawnTime = 30;
    OnTimerSetting();
  }

  public override Villain OnDeepCopy() {
    Demon newCopy = new Demon();
    return newCopy;
  }
}