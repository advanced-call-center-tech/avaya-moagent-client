//Copyright (c) 2010 - 2012, Matthew J Little and contributors.
//All rights reserved.
//
//Redistribution and use in source and binary forms, with or without modification, are permitted
//provided that the following conditions are met:
//
//  Redistributions of source code must retain the above copyright notice, this list of conditions
//  and the following disclaimer.
//
//  Redistributions in binary form must reproduce the above copyright notice, this list of
//  conditions and the following disclaimer in the documentation and/or other materials provided
//  with the distribution.
//
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR 
//IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR
//CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
//DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
//WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN
//ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace AvayaMoagentClient
{
  /// <summary>
  /// Utilities
  /// </summary>
  public static class Utilities
  {
    /// <summary>
    /// Converts a regular string to a SecureString.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static SecureString ToSecureString(this string str)
    {
      SecureString ret = null;

      if (str != null)
      {
        unsafe
        {
          fixed (char* chars = str)
          {
            ret = new SecureString(chars, str.Length);
            ret.MakeReadOnly();
          }
        }
      }

      return ret;
    }

    /// <summary>
    /// Converts a SecureString to a regular string.
    /// </summary>
    /// <param name="secure"></param>
    /// <returns></returns>
    public static string ToUnsecureString(this SecureString secure)
    {
      string ret = null;

      if (secure != null)
      {
        var unmanagedString = IntPtr.Zero;

        try
        {
          unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secure);
          ret = Marshal.PtrToStringUni(unmanagedString);
        }
        finally
        {
          Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
        }
      }

      return ret;
    }

    /// <summary>
    /// DigitsOnly
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string DigitsOnly(string value)
    {
      const string REGEX_PATTERN = "\\D*";
      var ret = value;

      if (!string.IsNullOrEmpty(value))
      {
        ret = Regex.Replace(value, REGEX_PATTERN, string.Empty);
      }

      return ret;
    }
  }
}
