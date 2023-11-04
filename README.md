# IvyTcp2Serial



<br />

##  About
IvyTcp2Serial is a bridge tool that allows a TCP stream to flow to a serial port. <br />
This tool allows applications dedicated to serial ports to be used through TCP/IP.<br />


<br />

## ⚡ Featurees

* TCP/IP ata stream to serial port.

<br />

## ️️ Requirements
   Windows 7 or later<br/>
   .NET 7.0 runtime library<br/>

<br />

##  Install
1. [Download](https://github.com/Invary/IvyTcp2Serial/releases) latest version of IvyTcp2Serial
2. Extract it to installation folder
3. Create shortcut of **`IvyTcp2Serial.exe`** .
4. Modify shortcut file, adding arguments like **`/server=192.168.100.101 /port=80 /comport=COM19 /baudrate=115200 /parity=none /stopbits=1`**

<br />

##  Supported arguments
* **`/comport=[name]`**  Serial port name. ex:**`/comport=COM1`**
* **`/baudrate=[speed]`**  Serial port baud rate value. ex:**`/baudrate=115200`**
* **`/parity=[none|odd|even|space|mark]`**  Serial port parity value. ex:**`/parity=none`**
* **`/stopbits=[none|1|1.5|2]`**  Serial port stop bits value. ex:**`/stopbits=1`**
* **`/server=[name]`**  Server IP address. ex:**`/server=192.168.100.101`**
* **`/port=[number]`**  Server port. ex:**`/port=80`**
* **`/?`**  Show supported command line options.

<br />

##  Examples of Usage
GRBL CNC control software named [Candle](https://github.com/Denvi/Candle) for windows supports serial ports only.<br />
[FluidNC](https://github.com/bdring/FluidNC) , a CNC controller, can be connected via serial port, but also via wifi/telnet. <br />
The following is an example of a setup in which CNC for wifi connection by [FluidNC](https://github.com/bdring/FluidNC) is controlled by a [Candle](https://github.com/Denvi/Candle). <br />
<br />
1. Install virtual serial port [com0com](https://sourceforge.net/projects/com0com/files/com0com/3.0.0.0/). <br />
2. In setup of com0com, check **`enumulate baudrate`** and add one pair of virtual serial port, COM19 & COM20. <br />
3. [Download](https://github.com/Invary/IvyTcp2Serial/releases) latest version of IvyTcp2Serial and extract. <br />
4. Create shortcut file of **`IvyTcp2Serial.exe`** .<br />
5. Right click chortcut file and open property. <br />
6. Edit shortcut link **`IvyTcp2Serial.exe`** to **`IvyTcp2Serial.exe /server=192.168.100.101 /port=23 /comport=COM19 /baudrate=115200 /parity=none /stopbits=1`**. IP address and comport should be changed according to your environment. <br />
7. Execute IvyTcp2Serial with serial, and connected to CNC/FluidNC via wifi/telnet. <br />
8. Execute Candle with serial port like **`COM20`**, and you can control CNC/FluidNC via wifi. <br />

<br />

##  Privacy policy
- Do not send privacy information.
- Do not display online ads.
- Do not collect telemetry information.
- If online information is needed, get it from github repositories whenever possible.

<br />


##  Changelog

- [Ver100](https://github.com/Invary/IvyTcp2Serial/releases/tag/Ver100)
Initial release

<br />

##  Suggestions and feedback
If you have any idea or suggestion, please add a github issue.

<br />


## ⭐ Price

FREE of charge. <br />
If you would like to donate, please send me a ko-fi or crypto. It's a great encouragement!

[![ko-fi](https://raw.githubusercontent.com/Invary/IvyMediaDownloader/main/img/donation_kofi.png)](https://ko-fi.com/E1E7AC6QH)

- Address: 0xCbd4355d13CEA25D87F324E9f35A075adce6507c<br/>
 -- Binance Smart Chain (BNB, USDT, BUSD etc.)<br/>
 -- Polygon network (MATIC, USDC etc.)<br/>
 -- Ethereum network (ETH)<br/>

- Address: 1FvzxYriyNDdeA12eaUGXTGSJxkzpQdxPd<br/>
 -- Bitcoin (BTC)<br/>

<br />


<br />
<br />
<br />

#### Copyright (c) Invary
