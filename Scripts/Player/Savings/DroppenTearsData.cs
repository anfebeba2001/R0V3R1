[System.Serializable]
public class DroppenTearsData 
{

   public int amount { get; private set; }
   public float[] position  { get; private set; } = new float[3];

   public DroppenTearsData(int amount, float[] position)
   {
        this.amount = amount;
        this.position = position;
   }
}

