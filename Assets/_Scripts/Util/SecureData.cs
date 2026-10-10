using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System;
using System.IO;

public class SecureData
{

	static int Iterations = 1000;
	// Obfuscation only: keeps casual players from editing PlayerPrefs, not a real secret.
	static string hash = "1olI6KZ3WHIeLPa92YPAfXB6";

	public static string DoEncrypt(string plainText, string hash)
	{
		var des = new DESCryptoServiceProvider();
		des.GenerateIV();

		var rfc2898DeriveBytes = new Rfc2898DeriveBytes(hash, des.IV, Iterations);
		

		byte[] key = rfc2898DeriveBytes.GetBytes(8);	
		using (var memoryStream = new MemoryStream())
			using (var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(key, des.IV), CryptoStreamMode.Write))
		{
			memoryStream.Write(des.IV, 0, des.IV.Length);		
			byte[] bytes = Encoding.UTF8.GetBytes(plainText);
			cryptoStream.Write(bytes, 0, bytes.Length);
			cryptoStream.FlushFinalBlock();
			
			return Convert.ToBase64String(memoryStream.ToArray());
		}
	}
	
	public static string DoDecrypt(string cipherText, string hash)
	{		
		var result = "";

		try
		{   
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			
			using (var memoryStream = new MemoryStream(cipherBytes))
			{			
				var des = new DESCryptoServiceProvider();
			
			
				byte[] iv = new byte[8];
				memoryStream.Read(iv, 0, iv.Length);
						
				var rfc2898DeriveBytes = new Rfc2898DeriveBytes(hash, iv, Iterations);
				
				byte[] key = rfc2898DeriveBytes.GetBytes(8);
				
				using (var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(key, iv), CryptoStreamMode.Read))
				    using (var streamReader = new StreamReader(cryptoStream))
				    {
					    result = streamReader.ReadToEnd();
				    }
			}
		}
		catch (Exception e)
        {
			result = "";
            Debug.Log(e);
		}

		return result;
	}

    public static void Save(string key, bool value)
    {
        Save(key, value?"true":"false");
    }
        
	public static void Save(string key, string value)
	{
        string hashedKey = GenerateMD5(key);
        var encryptedValue = DoEncrypt(value, hash);

		PlayerPrefs.SetString(hashedKey, encryptedValue);
	}

    public static bool Get(string key, bool defaultValue)
    {
        return Get(key, defaultValue.ToString().ToLower()).Equals("true");
    }

	public static string Get(string key, string defaultValue)
	{
        string hashedKey = GenerateMD5(key);

        var value = defaultValue;
        if (PlayerPrefs.HasKey(hashedKey))
        {
            value = PlayerPrefs.GetString(hashedKey);
            value = DoDecrypt(value, hash);
            
        }

		return value;
	}



    static string GenerateMD5(string text)
    {
        var md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = md5.ComputeHash(inputBytes);
        
        var sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }
}


