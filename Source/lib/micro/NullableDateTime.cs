namespace System
{
   public class NullableDateTime
   {
      public bool HasValue { get; set; }
      public DateTime Value { get; set; }

      public NullableDateTime()
      {
         HasValue = false;
      }

      public NullableDateTime(DateTime val)
      {
         HasValue = true;
         Value = val;
      }
   }
}
