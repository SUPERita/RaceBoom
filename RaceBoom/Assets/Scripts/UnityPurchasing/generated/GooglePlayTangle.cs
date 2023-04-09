// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("IN6WY3d8Lyx2vB4bg9YNKXimI99o/2G6of8ou0//Clmz0KYMnQD9rkPAzsHxQ8DLw0PAwMFTPUSqR3VMAEBVXEMxuGCx56v+fPkRdaovj8HSy/6J1Tu+tWo3p7MDNefH9MftaMEvBunIKy7kmnSuRcxR2ChY4zRxBHrEUI3YY5V3BkquSCdqm+mwcVy1C8jtxCn5rjmtinDwnOypLFeJYhFiqVPYCnNVfdTsCBwa5bnPlvqI8UPA4/HMx8jrR4lHNszAwMDEwcIqTn1q6O7XLQ9lyBagl59EcdsYtfrlktGC11Fq1bFzOQwnA0oVQIgK6lVKuF9OejfL2AkXD3LuuRVexNtUShhaOMuXXVyaoZH443vqWiY+0fwHGcgVOPyaMsPCwMHA");
        private static int[] order = new int[] { 9,7,7,7,11,10,7,13,10,9,12,11,12,13,14 };
        private static int key = 193;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
