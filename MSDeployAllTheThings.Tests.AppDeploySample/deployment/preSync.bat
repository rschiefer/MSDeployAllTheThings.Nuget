:: The %configuration%, %server%, %appName%, %destinationFilePath% and your custom variables will be created above this line

:: Stop task if running
::SCHTASKS /s %server% /END /TN "%appName%"
