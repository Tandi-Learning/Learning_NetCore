protoc.exe -I ..\Protos `
		   --js_out=import_style=commonjs:.\ `
		   --grpc-web_out=import_style=commonjs,mode=grpcwebtext:.\ `
		   meterreader.proto enums.proto