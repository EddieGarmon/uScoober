#uScoober
The filament for NETMF.

##Use like a Surgeon
uScoober is a core set of focused libraries, and a set of features and device drivers delivered as source.
You only use what you need, nothing more, no extra bloat on constrained devices. 
Shared projects (*.shproj) and code (c#) are currently compiled and tested with NETMF v4.2 and v4.3.
uScoober consumption via NuGet is the preferred method.

##To Contribute
1. Submit an issue to start the discussion
1. Fork the code
1. Work in a development branch (named: /dev-name/branch-reason)
2. Include tests. (sending a PR with only executing but failing tests is welcomed)
1. Send a pull request
1. Refine implementation due to reviews if necessary
1. Once agreeded upon, the team will merge the PR

##Develop with the source - (for features and drivers)
Directly include the shared project(s) of interest into your external work. (Hint: Create a junction
point from the uScoober folder to a path beneath/beside your external work root, then set that path
to be ignored by version control.)

##Active Plans for version 1:

###Core - delivered as an assembly
- [X] one semantic version for the entire Core API (all dlls)
- [X] NuGet package delivery
- [X] Interface all the things
- [X] Data Structures, and core utilities

###Core - Threading - delivered as an assembly
- [X] TPL like Task API
- [X] TaskSchedulerForUI
- [X] Task extensions for UI

###Core - Test Framework - delivered as an assembly
- [X] [xunit](https://github.com/xunit) inspired testing: facts and theories, constructor setup, IDisposable teardown
- [X] running in the simulator
- [X] decent simulator UI
- [X] running on the hardware, inject feedback mechanism
- [X] hardware IO mocks
- [ ] mock IIC bus
- [ ] mock SPI bus
- [ ] support async testing via Task implementation
- [X] Integrated build automation hook.
- [X] build powershell module for running tests

###Features - delivered as source
- [X] one semantic version per feature
- [X] NuGet package delivery - independent packages per feature
- [ ] Storage and Path API
- [X] Binary Tools
- [X] SPOT based managed IO
- [X] SPOT based managed multiple device IIC bus
- [ ] SPOT based managed multiple device SPI bus
- [ ] BitBang, software multiple device IIC bus
- [ ] BitBang, software multiple device SPI bus
- [ ] Boot/AppContainer API (from block storage)
- [ ] 

###Hardware - delivered as source
- [X] one semantic version per feature
- [X] NuGet package delivery - independent packages per device

####Boards
- [X] Netduino2+
- [X] Netduino2
- [ ] GHI?

####Drivers
- [X] LEDs
- [X] Buttons
- [ ] MCP23008/MCP23017 IIC IO Expander
- [ ] AT24C32 Eeprom
- [ ] DS1307 IIC real time clock
- [ ] TinyRTC (DS1307 and AT24C32)

 
##Future Plans
- [ ] run tests in remote app domain
- [ ] offload results of hardware run tests (aggregate how? network based listener?)
- [ ] support additional boards: GHI? who else?
- [ ] support simple Gadgeteer API?
- [ ] support Harnesses (similar to Gadgeteer)
- [ ] support FTDI FT4232HL (GHI Lynx) - as a new .NET 4.5 class library?
- [ ] UI primitives?

