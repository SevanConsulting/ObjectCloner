using System.Drawing;

namespace SC.ObjectCLoner.Tests
{
    public enum SimpleEnum
    {
        Unknown,
        Red,
        Black
    }
    
    public class SimpleTestData
    {
        public string StringProp { get; set; }
        public int IntProp { get; set; }
        public double FloatProp { get; set; }
        public bool BoolProp { get; set; }
        public SimpleEnum EnumProp { get; set; }
        public Point StructProp { get; set; }
    }
}