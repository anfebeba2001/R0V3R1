[System.Serializable]
public class PlayerData 
{
   
   public int defense { get; private set; }

   public int health { get; private set; }
   public int damage { get; private set; }
   public int resistance { get; private set; }
   
   public int tears { get; private set; }
   public int healingOrbs { get; private set; }
   public int arrows { get; private set; }
   public int costPerUpgrade { get; private set; }

   public PlayerData(int defense,int health, int damage, int resistance, int tears, int healingOrbs, int arrows, int costPerUpgrade)
   {
        this.defense = defense;
        this.health = health;
        this.damage = damage;
        this.resistance = resistance;
        this.tears = tears;
        this.healingOrbs = healingOrbs;
        this.arrows = arrows;
        this.costPerUpgrade = costPerUpgrade;
   }
}

