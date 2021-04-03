#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("jYjeloWPnY+foFvizB/9gnV/4AaPjZiJ3ti6mLuajYjej4GYgcr7+6ENww18hoqKjo6Lu+m6gLuCjYjepLsKSI2DoI2Kjo6MiYm7Cj2RCjjZ7ufi6uXo7qvk5av/4+L4q+ju+YPVuwmKmo2I3parjwmKg7sJio+7/Pyl6vv75+6l6OTmpOr7++fu6Oo8kDYYya+ZoUyElj3GF9XoQ8ALnK27r42I3o+AmJbK+/vn7qvI7vn/g6CNio6OjImKnZXj///7+LGkpPzi7eLo6v/i5OWryv7/4+T54v/yugmKi42CoQ3DDXzo746Kuwp5u6GN7ASDP6t8QCenq+T7PbSKuwc8yESnq+ju+f/i7eLo6v/uq/vk5+Lo8ufuq8Ll6KW6rbuvjYjej4CYlsr7HhXxhy/MANBfnby4QE+ExkWf4lq2reyrAbjhfIYJRFVgKKRy2OHQ770Sx6bzPGYHEFd4/BB5/Vn8u8RKq+Ttq//j7qv/4+7lq+r7++fi6Or0yiMTclpB7Rev4JpbKDBvkKFIlP/i7eLo6v/uq+nyq+rl8qv76vn/lBpQlczbYI5m1fIPpmC9KdzH3mf75+6ryO75/+Lt4ujq/+Lk5avK/lK99EoM3lIsEjK5yXBTXvoV9SrZho2CoQ3DDXyGioqOjouICYqKi9fp5+6r+P/q5e/q+e+r/+755vir6kvouPx8sYyn3WBRhKqFUTH4ksQ+q+rl76vo7vn/4u3i6Or/4uTlq/vl76vo5OXv4v/i5OX4q+Ttq/747q9pYFo8+1SEzmqsQXrm82ZsPpycuL3Ru+m6gLuCjYjej42Yid7Yupg1f/gQZVnvhEDyxL9TKbVy83TgQz6xJn+EhYsZgDqqnaX/XreGUOmd776onsCe0pY4H3x9FxVE2zFK09sjV/WpvkGuXlKEXeBfKa+omnwqJ8JT/RS4n+4q/B9CpomIiouKKAmKjouICYqEi7sJioGJCYqKi28aIoLSLI6C95zL3ZqV/1g8AKiwzChe5ACSAlVywOd+jCCpu4ljk7Vz24JYOrvTZ9GPuQfjOASWVe74dOzV7jeEFrZ4oMKjkUN1RT4yhVLVl11Ato27hI2I3paYiop0j467iIqKdLuWvrm6v7u4vdGchri+u7m7srm6v7uMZ/ayCADYq1izTzo0EcSB4HSgd/Kr6vj4/ubu+Kvq6Oju+//q5ejuC5+gW+LMH/2CdX/gBqXLLXzMxvSlyy18zMb0g9W7lI2I3paoj5O7nbsJjzC7CYgoK4iJiomJiom7ho2CQpL5ftaFXvTUEHmuiDHeBMbWhnq7mo2I3o+BmIHK+/vn7qvC5eiluvnq6P/i6O6r+P/q/+7m7uX/+KW7lA4IDpAStsy8eSIQywWnXzobmVME+ArrTZDQgqQZOXPPw3vrsxWefqvIyrsJiqm7ho2CoQ3DDXyGioqKICj6GczY3kokpMo4c3Bo+0ZtKMfO9ZTH4NsdygJP/+mAmwjKDLgBCvG7CYr9u4WNiN6WhIqKdI+PiImK/+Pk+eL/8rqdu5+NiN6PiJiGyvv75+6r2eTk/6vIyruVnIa7vbu/uZ27n42I3o+ImIbK+/vn7qvZ5OT/2yEBXlFvd1uCjLw7/v6q");
        private static int[] order = new int[] { 30,2,2,37,38,33,36,59,10,35,11,34,21,44,37,18,56,48,41,48,39,41,38,58,47,42,57,45,43,53,34,50,58,39,54,45,37,52,56,57,56,53,59,51,56,58,50,54,48,58,50,58,55,56,54,59,58,57,59,59,60 };
        private static int key = 139;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
