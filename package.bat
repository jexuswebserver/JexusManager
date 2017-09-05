del JexusManager.zip
mkdir bin
cd bin
del *.dll.config *.xml
..\lib\7z.exe a ..\JexusManager.zip @..\list.txt
cd ..
