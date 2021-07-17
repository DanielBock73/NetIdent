# Hilscher netIdent Cli

The Ethernet Device Configuration Tool is used for the following purposes:

Set IP-adress via Hilscher protocol "NetIdent", to be able to configure Hilscher devices via TCP (e.g. NL 50 / 51, NT 50, NetHOST)

## Install

### Step 1

Clone the folowing repos
```bash
git clone https://github.com/DanielBock73/Hilscher.NetIdent.git
git clone https://github.com/DanielBock73/NetIdent.git
```

ist schut looks like this

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

run 

```bash
./src/netIdent/bin/Debug/net5.0/netIdent scan -i enp0s25 
```

you resive:

```json
{
  "MagicCookie": "HINI",
  "OpCode": 33554432,
  "ErrorCode": 0,
  "MasterIpAddress": "192.168.1.10",
  "PortNumber": 25384,
  "IpAddress": "192.168.0.1",
  "MacAddress": "XXXXXXXXXX",
  "DeviceType": DDDDDDD,
  "SerialNumber": SSSSSS,
  "DeviceName": "netHOST",
  "AddrSwitch": 0,
  "Version": 3892510720,
  "TransactionID": 2330697283,
  "HiniFlags": 2113929216
}
```

### Set the ip address for a particular device


```bash
./src/netIdent/bin/Debug/net5.0/netIdent update -i enp0s25 -s SSSSSS -d DDDDDDD 192.168.1.222
```

the has to be set like the resived one:

+ SerialNumber ==> -s, --serial-number 
+ DeviceType ==> -d, --devive-type 

when the ip address was successfully set you resive similare out like the scan output but with the corect "IpAddress": "192.168.0.222":

```json
{
  "MagicCookie": "HINI",
  "OpCode": :67108864,
  "ErrorCode": 0,
  "MasterIpAddress": "192.168.1.10",
  "PortNumber": 25384,
  "IpAddress": "192.168.0.222",
  "MacAddress": "XXXXXXXXXX",
  "DeviceType": DDDDDDD,
  "SerialNumber": SSSSSS,
  "DeviceName": "netHOST",
  "AddrSwitch": 0,
  "Version": 3892510720,
  "TransactionID": 1133640155,
  "HiniFlags": 2113929216
}
```
