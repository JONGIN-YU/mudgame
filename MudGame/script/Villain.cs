using System;
using System.Timers;
public abstract class Villain : Human {
  public int maxLevel;
  public int minLevel;

  public int multiHp;
  public int multiAtk;
  public int multiExp;
  public int givingExp;

  public float respawnTime;
  public System.Timers.Timer timer;


  Random rand = new Random();
  public void OnTimerSetting() {
    timer = new System.Timers.Timer(respawnTime * 1000);
    timer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
  }

  private void OnTimedEvent(Object source, ElapsedEventArgs e) {
    OnRespawn();
    timer.Stop();
  }

  public void OnRespawn() {
    if (isDead) {
      Console.WriteLine("적이 부활하였습니다!");
      lev = rand.Next(minLevel, maxLevel);
      OnSetStatus();
      isDead = false;
    }
  }

  public void OnSetStatus() {
    lev = rand.Next(minLevel, maxLevel);
    fullHp = lev * multiHp;
    curHp = fullHp;
    fullAtk = lev * multiAtk;
    curAtk = fullAtk;
    givingExp = lev * multiExp;
  }
  public abstract Villain OnDeepCopy();
}



