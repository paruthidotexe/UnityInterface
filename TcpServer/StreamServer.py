import socket
import time
from random import randint
import threading

HOST = '127.0.0.1'
PORT = 12345

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((HOST, PORT))
s.listen(1)

while True:
    conn, addr = s.accept()
    print ('Client connection accepted ', addr)
    while True:
        try:
            data = randint(0, 9)
            print ('Server sent:', data)
            conn.send(data.to_bytes(2, byteorder='big'))
            time.sleep(1)
        except socket.error:
            print ('Client connection closed', addr)
            break

conn.close()

