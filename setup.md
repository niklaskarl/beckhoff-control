## System Setup

### ssh

##### Local Setup
``` bash
# generate local key pair
ssh-keygen -o
```

``` bash
mkdir -p /home/beckhoff/.ssh
touch /home/beckhoff/.ssh/authorized_keys
chmod 700 /home/user_name/.ssh
chmod 600 /home/user_name/.ssh/authorized_keys
```

### directories
- ``` /home/beckhoff/www/bin ```
- ``` /home/beckhoff/www/dist ```

### systemd
``` bash
cd /home/beckhoff
mkdir -p .config/systemd/user
cd .config/systemd/user
nano beckhoff.service # content below
systemctl --user daemon-reload
systemctl --user enable --now beckhoff.service

# enable automatic startup of user services
loginctl enable-linger beckhoff
```

Content of *beckhoff.service* file:
``` systemd
[Unit]
Description=Beckhoff Service
After=network.target

[Service]
ExecStart=/home/beckhoff/www/bin/ControlServer
WorkingDirectory=/home/beckhoff/www/bin
StandardOutput=inherit
StandardError=inherit
Restart=always
User=beckhoff

[Install]
WantedBy=multi-user.target
```

### nginx
``` nginx
server {
    listen 80;

    # serve static files
    location ~ ^/(images|javascript|js|css|flash|media|static)/ {
        root /var/www/bekhoff/dist;
        expires 30d;
    }

    # forward dynamic content
    location / {
        proxy_pass http://127.0.0.1:5000;
    }
}
```

## Deployment

### Backend
``` bash
dotnet publish -c Release -f netcoreapp3.1 -r ubuntu.20.04-arm64

ssh beckhoff@192.168.168.12 "systemctl --user stop beckhoff.service"

# copy publish directory to target machine
tar -cf - ./bin/Release/netcoreapp3.1/ubuntu.20.04-arm64/publish | ssh target.org " ( cd /home/beckhoff/www/bin ; tar -xf - ) "

ssh beckhoff@192.168.168.12 "chmod +x /home/beckhoff/www/bin/ControlServer"
ssh beckhoff@192.168.168.12 "systemctl --user start beckhoff.service"
```