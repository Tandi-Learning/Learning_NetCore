======================================================================

1. from VS solution explorer, right mouse click client-grpc folder and select "Open In Terminal" from the popup menu
2. add node_modules to the env path
   $env:Path = $env:Path + "./node_modules/.bin/;./node_modules/grpc-tools/bin;./node_modules/protoc-gen-grpc-web/bin/;"

3. execute powershell script
   .\generate.ps1

======================================================================
...\client-grpc> npx webpack -o ../wwwroot/js/send/ ./send.js --mode development