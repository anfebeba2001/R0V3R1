[System.Serializable]
public class DroppenTearsData 
{
   public int amount;
   public float[] position = new float[3];

   public DroppenTearsData(int amount, float[] position)
   {
        this.amount = amount;
        this.position = position;
   }
}

