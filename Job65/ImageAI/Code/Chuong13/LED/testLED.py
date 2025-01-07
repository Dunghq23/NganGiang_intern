from gpiozero import LED
from time import sleep, time

led = LED(17)  # Thay 17 bằng chân GPIO tương ứng của bạn

print("Nhập lệnh để điều khiển LED:")
print("   'on'    - Bật LED")
print("   'flash' - Bật LED nháy")
print("   'off'   - Tắt LED")
print("   'exit'  - Thoát chương trình")

try:
    while True:
        command = input("Nhập lệnh: ").strip().lower()  # Nhập lệnh từ người dùng
        if command == "on":
            led.on()
            print("     [THÔNG BÁO] LED đã bật.")
        elif command == "off":
            led.off()
            print("     [THÔNG BÁO] LED đã tắt.")
        elif command == "flash":
            print("     [THÔNG BÁO] LED đang nháy.")
            start_time = time()  # Lưu thời gian bắt đầu
            while time() - start_time < 5:  # Chạy trong 5 giây
                led.on()    # Bật đèn LED
                sleep(0.1)  # Đợi 0.1 giây
                led.off()   # Tắt đèn LED
                sleep(0.1)  # Đợi 0.1 giây
        elif command == "exit":
            print("Thoát chương trình.")
            break
        else:
            print("Lệnh không hợp lệ. Vui lòng nhập 'on', 'flash', 'off', hoặc 'exit'.")
except KeyboardInterrupt:
    print("\nKeyboardInterrupt detected. Stopping the LED.")
finally:
    led.off()  # Đảm bảo LED tắt trước khi thoát
    print("LED đã tắt. Kết thúc chương trình.")
