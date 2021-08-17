// Copyright 2005-2021 Daniel Bock. All rights reserved. See License.md in the project root for license information.

using CommandLine;

namespace netIdent
{
  [Verb("update", HelpText = "Record changes to the repository.")]
  class UpdateOptions
  {
    [Option('i', "interface", Required = true, HelpText = "Listen on interface. If unspecified, tcpdump searches the system interface list for the lowest numbered, configured up interface (excluding loopback), which may turn out to be, for example, ``eth0''.")]
    public string Interface { get; set; }

    [Option('d', "device-type", Required = true, HelpText = "Only if the device type in the request corresponds with the device type of the device, the device will acept the request.")]
    public uint DeviceType { get; set; }

    [Option('s', "serial-number", Required = false, HelpText = "Only if the serial number in the request corresponds with the serial number of the device, the device will acept the request.")]
    public uint SerialNumber { get; set; }

    [Option('m', "mac-address", Required = false, HelpText = "Only if the MAC address in the request corresponds with the MAC address of the device, the device will acept the request.")]
    public string MacAddress { get; set; }

    // omitting long name, default --verbose
    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { get; set; }
  
    [Value(0)]
    public string IpAddress { get; set; }
  }
}
