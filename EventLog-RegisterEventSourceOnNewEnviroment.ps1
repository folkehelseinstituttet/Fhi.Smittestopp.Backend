# Application need to have administrator's rights on the first run to register an Event Source (on local machine you need to open Visual Studio as admin and make first log entry and app will register the EventSource)
# This script can also register Event sources for the apps. It needed to be run with administrator rights.
# Tip: The EventLogMessages.dll path can be taken from the EventLog Viewer and the Windows registry (\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\EventLog\[LogName] ) after creating EventSource on local machine by Visual Studio.
# BE AWERE what version of .net framework is installed on the machine. This  script uses v4.0.30319.
# Without creating the EventSouurce on enviroment logs form the apps will not appear in the Windows EventLog. It is needed for maintenance alerts (MAINTENANCE ALERTS ARE BASED ON THIS ).

$file = "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\EventLogMessages.dll"
New-EventLog -ComputerName . -Source "SS-API-DIGNDB.App.SmitteStop" -LogName Application -MessageResourceFile $file -CategoryResourceFile $file
New-EventLog -ComputerName . -Source "SS-Jobs-DIGNDB.App.SmitteStop" -LogName Application -MessageResourceFile $file -CategoryResourceFile $file
