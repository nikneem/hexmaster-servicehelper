# hexmaster-servicehelper
Helper classes to allow simple debugging of Windows Services

If you've created a service in Visual Studio.NET, the program.cs contains the following line:
<code>ServiceBase.Run(ServicesToRun);</code>

After installing the service helper, change this line like so:<br/>
<code>HexMaster.Helper.Run(ServicesToRun);</code>

Now if you run your project with a debugger attached, you will be able to run the
service from a window that pops up, else the service will run as is would without
having the service helper installed.

Note you can install the service helper easy using our NuGet package :<br />
<code>install-package <strong>HexMaster.WindowsServiceHelper</strong></code>

See the NuGet gallery page for more information :
https://www.nuget.org/packages/HexMaster.WindowsServiceHelper/
