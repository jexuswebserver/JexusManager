mkdir bin
cd bin
del JexusManager.zip
del *.pdb *.xml
..\lib\7z.exe a JexusManager.zip @..\list.txt
cd ..
