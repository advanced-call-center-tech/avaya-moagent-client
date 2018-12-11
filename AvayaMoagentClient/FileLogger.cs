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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace AvayaMoagentClient
{
  /// <summary>
  /// FileLogger
  /// </summary>
  public class FileLogger
  {
    private const int _DEFAULT_SLEEP_TIME = 5000;
    private readonly List<string> _events;
    private readonly StringBuilder _writeBuffer = new StringBuilder();
    private readonly Thread _writer;

    /// <summary>
    /// Default constructor
    /// </summary>
    public FileLogger()
    {
      _events = new List<string>();
      SleepTimeMs = _DEFAULT_SLEEP_TIME;

      LogFile = Path.Combine(Directory.GetCurrentDirectory(),
        string.Format("{0}.{1:yyyy.MM.dd.HHmm}.{2}.log", "Moagent32", DateTime.Now, Process.GetCurrentProcess().Id));

      _writer = new Thread(_WriteLogFile);
      _writer.Name = "_Moagent32LogFileWriter" + _writer.ManagedThreadId;
      _writer.IsBackground = true;
      _writer.Start();
    }

    ~FileLogger()
    {
      _WriteLogData();
    }

    /// <summary>
    /// LogFile
    /// </summary>
    public string LogFile { get; set; }

    /// <summary>
    /// Sleep time between log refreshes, in milliseconds.
    /// </summary>
    public int SleepTimeMs { get; set; }

    /// <summary>
    /// Write
    /// </summary>
    /// <param name="msg"></param>
    public void Write(string msg)
    {
      _Write(msg);
    }

    /// <summary>
    /// FlushBuffer
    /// </summary>
    public void FlushBuffer()
    {
      _WriteLogData();
    }

    private void _Write(string msg)
    {
      Debug.WriteLine(msg);

      lock (_writeBuffer)
      {
        _writeBuffer.AppendLine(msg);
      }

      lock (_events)
      {
        _events.Add(msg);
      }
    }

    private void _WriteLogFile()
    {
      do
      {
        Thread.Sleep(SleepTimeMs);

        _WriteLogData();
      } while (true);
    }

    private void _WriteLogData()
    {
      if (!string.IsNullOrEmpty(LogFile) && _writeBuffer.Length > 0)
      {
        try
        {
          string buffer;

          lock (_writeBuffer)
          {
            buffer = _writeBuffer.ToString();
            _writeBuffer.Clear();
          }

          using (var stream = new FileStream(LogFile, FileMode.Append))
          {
            using (var writer = new StreamWriter(stream))
            {
              writer.Write(buffer);
              writer.Flush();
              writer.Close();
            }

            stream.Close();
          }
        }
        catch
        {
          //Well...we tried...
        }
      }
    }
  }
}
