mkdir bin
cd bin
call ..\packages\Obfuscar.2.2.0\tools\Obfuscar.Console.exe jexus.xml
@IF %ERRORLEVEL% NEQ 0 PAUSE
cd ..