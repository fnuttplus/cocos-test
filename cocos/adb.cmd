@echo off
echo su
echo mount -o rw,remount rootfs /
echo chmod 777 /mnt/sdcard
echo exit
"C:\Program Files (x86)\Android\android-sdk\platform-tools\adb.exe" shell
