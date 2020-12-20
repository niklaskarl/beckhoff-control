
# build backend service
pushd .\src\ControlServer
& dotnet publish -c Release -f netcoreapp3.1 -r ubuntu.20.04-arm64
popd

# build frontend service
pushd .\src\ClientApp
npm run build
popd
New-Item -ItemType Directory -Force -Path ".\src\ControlServer\bin\Release\netcoreapp3.1\ubuntu.20.04-arm64\publish\wwwroot"
Copy-Item -Path ".\src\ClientApp\dist\*" -Destination ".\src\ControlServer\bin\Release\netcoreapp3.1\ubuntu.20.04-arm64\publish\wwwroot" -Recurse -Force

# stop running service on deployment machine
& ssh beckhoff@192.168.168.12 "systemctl --user stop beckhoff.service"

# copy publish directory to target machine
pushd .\src\ControlServer\bin\Release\netcoreapp3.1\ubuntu.20.04-arm64\publish
& tar -cf ..\publish.tar .
echo "put ..\publish.tar" | sftp beckhoff@192.168.168.12:/home/beckhoff/www
& ssh beckhoff@192.168.168.12 "rm -r /home/beckhoff/www/bin/"
& ssh beckhoff@192.168.168.12 "mkdir -p /home/beckhoff/www/bin/"
& ssh beckhoff@192.168.168.12 "tar -xf /home/beckhoff/www/publish.tar -C /home/beckhoff/www/bin/"
& ssh beckhoff@192.168.168.12 "rm /home/beckhoff/www/publish.tar"
popd

& ssh beckhoff@192.168.168.12 "chmod +x /home/beckhoff/www/bin/ControlServer"
& ssh beckhoff@192.168.168.12 "systemctl --user enable --now beckhoff.service"
