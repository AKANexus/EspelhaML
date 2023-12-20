namespace MlSuite.Api.Statics
{
    public static class StaticFuncs
    {
        public static class Extensions
        {
            public static bool IsNumericType(Type? type)
            {
                return type == typeof(int) ||
                       type == typeof(uint) ||
                       type == typeof(long) ||
                       type == typeof(ulong) ||
                       type == typeof(short) ||
                       type == typeof(ushort) ||
                       type == typeof(byte) ||
                       type == typeof(sbyte) ||
                       type == typeof(float) ||
                       type == typeof(double) ||
                       type == typeof(decimal);
            }

        }
    }
}
