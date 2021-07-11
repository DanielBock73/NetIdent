// Copyright 2005-2021 Daniel Bock. All rights reserved. See License.md in the project root for license information.

using CommandLine;

namespace netIdent
{
  [Verb("scan", HelpText = "scan file contents to the index.")]
  class ScanOptions
  {
    [Option('i', "interface", Required = true, HelpText = "Listen on interface.  If unspecified, tcpdump searches the system interface list for the lowest numbered, configured up interface (excluding loopback), which may turn out to be, for example, ``eth0''.")]
    public string Interface { get; set; }

    [Option('s', "serial-number", Required = false, HelpText = "Only if the serial number in the request corresponds with the serial number of the device, the device will acept the request.")]
    public uint SerialNumber { get; set; }

    [Option('j', "Json", Default = true, Required = false, HelpText = "Set output to verbose messages.")]
    public bool Json { get; set; }

    [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
    public bool Verbose { get; set; }
  }
}


