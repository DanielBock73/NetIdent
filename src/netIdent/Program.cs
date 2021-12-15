// Copyright 2005-2021 Daniel Bock. All rights reserved. See License.md in the project root for license information.

using System;
using System.Net;
using System.Linq;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using CommandLine;
using System.Collections.Generic;
using Hilscher.netIdent;


namespace netIdent
{
  class Program
  {
    static public List<(String, IPAddress)> GetNetAdapters()
    {
      var values = new List<(String, IPAddress)>();
      foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
      {
        foreach (UnicastIPAddressInformation ip in nic.GetIPProperties().UnicastAddresses)
        {
          if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
          {

            values.Add((nic.Name, ip.Address));
          }
        }
      }
      return values;
    }

    static int Main(string[] args)
    {
      return CommandLine.Parser.Default.ParseArguments<ScanOptions, UpdateOptions>(args)
      .MapResult(
        (ScanOptions opts) => RunScanAndReturnExitCode(opts),
        (UpdateOptions opts) => RunUpdateAndReturnExitCode(opts),
        errs => 1);
    }

    static int RunScanAndReturnExitCode(ScanOptions options)
    {
      var netAdapters = GetNetAdapters();
      var netAdapter = netAdapters.FirstOrDefault(p => p.Item1 == options.Interface);

      var settings = new JsonSerializerSettings();
      settings.Converters.Add(new IPAddressConverter());
      settings.Converters.Add(new PhysicalAddressConverter());

      var data = new NetIdentProtocolMessage
      {
        MasterIpAddress = netAdapter.Item2,
        PortNumber = NetIdentPorts.MasterPort,
        OpCode = OpCodeEnum.IDENTIFY_REQUEST
      };

      if (options.SerialNumber != 0)
      {
        data.SerialNumber = options.SerialNumber;
      }

      if (options.Verbose)
      {
        Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.None, settings));
      }

      var task = NetIdentSocket.SendAsync(data, (p) =>
      {
        string json = JsonConvert.SerializeObject(p, Formatting.None, settings);
        Console.WriteLine(json);
      }, 2000);

      task.Wait();

      return 0;
    }

    static int RunUpdateAndReturnExitCode(UpdateOptions options)
    {
      var netAdapters = GetNetAdapters();
      var netAdapter = netAdapters.FirstOrDefault(p => p.Item1 == options.Interface);

      var settings = new JsonSerializerSettings();
      settings.Converters.Add(new IPAddressConverter());
      settings.Converters.Add(new PhysicalAddressConverter());

      var data = new NetIdentProtocolMessage
      {
        MasterIpAddress = netAdapter.Item2,
        PortNumber = NetIdentPorts.MasterPort,
        SerialNumber = options.SerialNumber,
        DeviceType = options.DeviceType,
        IpAddress = IPAddress.Parse(options.IpAddress),
        MacAddress = string.IsNullOrWhiteSpace(options.MacAddress) ? NetIdentProtocolMessage.MacAddressZero : PhysicalAddress.Parse(options.MacAddress),
        OpCode = OpCodeEnum.SET_IP_ADDRESS_REQUEST,
      };
      if (options.Verbose)
      {
        Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.None, settings));
      }

      var task = NetIdentSocket.SendAsync(data, (p) =>
      {
        string json = JsonConvert.SerializeObject(p, Formatting.None, settings);
        Console.WriteLine(json);
      });

      task.Wait();

      return 0;
    }

    public static void DoGetHostAddresses(string hostname)
    {
      IPAddress[] addresses = Dns.GetHostAddresses(hostname);

      Console.WriteLine($"GetHostAddresses({hostname}) returns:");

      foreach (IPAddress address in addresses)
      {
        Console.WriteLine($"    {address}");
      }
    }
  }

  class IPAddressConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return (objectType == typeof(IPAddress));
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteValue(value.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      return IPAddress.Parse((string)reader.Value);
    }
  }

  class PhysicalAddressConverter : JsonConverter
  {
    public override bool CanConvert(Type objectType)
    {
      return (objectType == typeof(PhysicalAddress));
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteValue(value.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      return IPAddress.Parse((string)reader.Value);
    }
  }
}


