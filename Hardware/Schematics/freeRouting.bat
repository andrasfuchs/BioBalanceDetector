@ECHO OFF
ECHO if not set already, set the JAVA_HOME environment variable to the shortened Java installation folder (eg. "C:\Progra~1\Java\jdk-11.0.6")

"%JAVA_HOME%\bin\javaw.exe" -jar "c:\Work\BioBalanceDetector\Tools\KiCad_FreeRouting\FreeRouting-miho-master\freerouting-master\build\libs\freerouting-executable.jar"