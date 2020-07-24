#
#-------------------------------------------------------------------------
# Basic Server
# ============
#
# Notes:
# ------
# Simple python server 
# This server replies Hi to new client
#
#-------------------------------------------------------------------------
#

import socket

class BasicClient:
    serverPortNo = 4444
    serverIP = "127.0.0.1"

    def startClient(self):
        print("Client Started")
        return
    
    def stopClient(self):
        print("Client Stopped")
        return

basicClient = BasicClient()

basicClient.startClient()


