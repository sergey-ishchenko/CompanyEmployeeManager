﻿using System.Security.Cryptography;
using System.Text;

namespace CompanyEmployeeManager.Helpers
{
	public class AuthHelper
	{
		public static string HashPassword(string password)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
				StringBuilder builder = new StringBuilder();
				foreach (var b in bytes)
				{
					builder.Append(b.ToString("x2"));
				}
				return builder.ToString();
			}
		}
	}
}
