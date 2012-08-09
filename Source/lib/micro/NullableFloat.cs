namespace System
{
   public class NullableFloat
   {
      public bool HasValue { get; set; }
      public float Value { get; set; }

      public NullableFloat()
      {
         HasValue = false;
      }

      public NullableFloat(float val)
      {
         HasValue = true;
         Value = val;
      }
   }
}
