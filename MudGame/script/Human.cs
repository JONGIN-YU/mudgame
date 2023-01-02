using System;
using System.Numerics;
public class Human {
  public string name;
  public int lev;
  public int curHp;
  public int fullHp;
  public int curAtk;
  public int fullAtk;
  public bool isDead;

  public bool OnDead() {
    isDead = true;
    return isDead;
  }

  public void OnAttack(Human human) {
    human.curHp -= curAtk;
  }
}
