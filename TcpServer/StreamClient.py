import socket

HOST = 'localhost'

#PORT = 12345
PORT = 20480 

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((HOST, PORT))

while True:
    data = s.recv(1024)
    if not data:
        break
    print (data)
    strVal = str(data)
    #print("--<<" + strVal + ">>--")

s.close()

