# Hilscher netIdent cli

The netIdent ethernet device configuration tool is used for the following purposes:

Set IP-address via Hilscher protocol "NetIdent", to be able to configure Hilscher devices via TCP (e.g. NL 50 / 51, NT 50, NetHOST)

## Install

### Step 1

Clone the following repos:

```bash
git clone https://github.com/DanielBock73/Hilscher.NetIdent.git
git clone https://github.com/DanielBock73/NetIdent.git
```

it should looks like this

![image](https://user-images.githubusercontent.com/8033405/126030024-4d507b0d-8a8b-4e47-aeb0-0e37c65bf2c9.png)


### Step 2

Change the directory

```bash
cd NetIdent
```

### Step 3

Build the project 

```bash
dotnet build
```

## Execute the cli app

### Scan for devices

```bash
./src/netIdent/bin/Debug/net5.0/netIdent scan -i enp0s25 
```

you receive:

```json
{
  "MagicCookie": "HINI",
  "OpCode": 33554432,
  "ErrorCode": 0,
  "MasterIpAddress": "192.168.1.10",
  "PortNumber": 25384,
  "IpAddress": "192.168.0.1",
  "MacAddress": "XXXXXX",
  "DeviceType": 999999,
  "SerialNumber": 888888,
  "DeviceName": "netHOST",
  "AddrSwitch": 0,
  "Version": 3892510720,
  "TransactionID": 2330697283,
  "HiniFlags": 2113929216
}
```

### Set the ip address for a particular device


```bash
netIdent update -i enp0s25 -s 888888 -d 999999 192.168.1.222
```

the has to be set like the receive one:

+ SerialNumber ==> -s, --serial-number 
+ DeviceType ==> -d, --device-type 

when the ip address was successfully set you receive similar out like the scan output but with the correct "IpAddress": "192.168.0.222":

```json
{
  "MagicCookie": "HINI",
  "OpCode": 67108864,
  "ErrorCode": 0,
  "MasterIpAddress": "192.168.1.10",
  "PortNumber": 25384,
  "IpAddress": "192.168.0.222",
  "MacAddress": "XXXXXXXXXX",
  "DeviceType": 999999,
  "SerialNumber": 888888,
  "DeviceName": "netHOST",
  "AddrSwitch": 0,
  "Version": 3892510720,
  "TransactionID": 1133640155,
  "HiniFlags": 2113929216
}
```

How to get `--interface` on Windows

```pws
Get-NetIPAddress | Select-Object -Property InterfaceAlias,IPAddress
```
Output:
```
InterfaceAlias   IPAddress
--------------   ---------
vEthernet (nat)  192.168.0.100
```

Run on Powershell and get as `PSCustomObject`.

```pwsh
netIdent scan -i 'vEthernet (nat)' | ConvertFrom-Json 
```

## Disclaimer

Hopefully this is obvious, but:

> This is an open source project (under the [MIT license]), and all contributors are volunteers. All commands are executed at your own risk.