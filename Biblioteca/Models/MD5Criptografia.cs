using System.Security.Cryptography;
using System.Text;

public class MD5Criptografia
{
    public static string EncodeMD5(string s)
    {
        MD5 md5 = new MD5CryptoServiceProvider(); 
        md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(s));  
    
        byte[] result = md5.Hash;

        StringBuilder strBuilder = new StringBuilder();  
        for (int i = 0; i < result.Length; i++)  
            strBuilder.Append(result[i].ToString("x2"));  

        return strBuilder.ToString();
    }

    public static bool compareMD5(string s, string encoded){
        return encoded.Equals(EncodeMD5(s));
    }

}