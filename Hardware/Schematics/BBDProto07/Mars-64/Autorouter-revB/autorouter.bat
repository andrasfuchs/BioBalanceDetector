@echo off
for /r %%i in (*.dsn) do (call :execute "%%i")
GOTO :eof

:execute
echo Starting autorouter for '%~n1'
start cmd.exe /K "c:\Progra~1\Java\jdk-11.0.6\bin\java.exe -jar freerouting-executable.jar -de %~n1.dsn -do %~n1.ses -mp 100"
timeout 3
GOTO :eof