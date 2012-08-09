namespace System
{
   public class NullableInt
   {
      public bool HasValue { get; set; }
      public int Value { get; set; }

      public NullableInt()
      {
         HasValue = false;
      }

      public NullableInt(int val)
      {
         HasValue = true;
         Value = val;
      }
   }
}
