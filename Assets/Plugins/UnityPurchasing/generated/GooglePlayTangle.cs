#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("brhNE1b4uc5WU8r8lOHEC8ASXJqZvczKOGr9ieL7EqKO8qc5Yn88VCngNY8uctABrtChfwCZMAlVOX0WfqLxkPdHVRIlrB7r/MQWDfr/n8W0SnPAS8KMD+co0PF5/X74HH6NcYT6Em2Dcaju5BR5d0o0DonMGzdBCa9WMUib9F4/E/HGTqZNGizHk8rXi3/60jcULX7Onksi5ZnVtfGLWdZk58TW6+DvzGCuYBHr5+fn4+blHSp9kQHJ0WvfPn7DjzzJiy+p34lk5+nm1mTn7ORk5+fmaM3jfjxi03Ku4Jgwtf9IPjflmb5dn7vHofqlMKcv+5/iEpkkYsc3aeBadwUn2WYjNfuX2KoNEcpe79U60pvKfKkDiBHr+bZPLWjiI+Tl5+bn");
        private static int[] order = new int[] { 2,3,7,12,9,11,10,13,13,10,12,12,13,13,14 };
        private static int key = 230;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
