import socket

HOST = 'localhost'
PORT = 12345

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((HOST, PORT))

while True:
    data = s.recv(1024)
    print (data)

s.close()

