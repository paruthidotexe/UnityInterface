import socket
import time
from datetime import datetime
from random import randint
import threading

HOST = '127.0.0.1'
PORT = 8888

def ClientThread(clientSocket):
    try:
        while True:   
            # data received from client 
            data = clientSocket.recv(1024) 
            #print(data)  
            if not data: 
                print('Bye')                
                # lock released on exit 
                #print_lock.release() 
                break
            print("Client : " + data.decode("utf-8"), end='\n')

    except socket.error as exp:
        print("Exp in ClientThread : " + str(exp))
    print('closed')   
    # connection closed 
    clientSocket.close()


s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((HOST, PORT))
s.listen(10)

while True:
    conn, addr = s.accept()
    conn.send(b'Streaming python server')
    print ('Client connection accepted ', addr)
    newClientThread = threading.Thread(target= ClientThread, args=(conn,))
    #newClientThread.daemon = True
    newClientThread.start()
    while True:
        try:
            data = randint(0, 9)
            print('Sending:{' +  str(data) + '}')

            # now = datetime.now()
            # date_time = now.strftime("%m/%d/%Y, %H:%M:%S")
            # print ('Date, Time :', date_time)

            conn.send(bytes(str(data),"utf-8"))
            #conn.send(bytes(date_time, "utf-8"))
            time.sleep(1)
        except socket.error:
            print ('Client connection closed', addr)
            break

conn.close()

'''

8,9,10 : TCP Server Client

11 - 16 : REST, Chat, Mongo, Flask?

17 - 23 : SQL, VUE / React, OAuth 

24 - 29 : Deployment, Cloud, Automation test, Logging, Debugging 

'''
