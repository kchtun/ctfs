from random import choice
from string import lowercase
from hashlib import sha256
from hashlib import sha1
from hashlib import md5
import random
import threading
import sys
import time
import socket


n = 50

CODE = {'A': '.-',     'B': '-...',   'C': '-.-.', 
        'D': '-..',    'E': '.',      'F': '..-.',
        'G': '--.',    'H': '....',   'I': '..',
        'J': '.---',   'K': '-.-',    'L': '.-..',
        'M': '--',     'N': '-.',     'O': '---',
        'P': '.--.',   'Q': '--.-',   'R': '.-.',
     	'S': '...',    'T': '-',      'U': '..-',
        'V': '...-',   'W': '.--',    'X': '-..-',
        'Y': '-.--',   'Z': '--..',
        

        '0': '-----',  '1': '.----',  '2': '..---',
        '3': '...--',  '4': '....-',  '5': '.....',
        '6': '-....',  '7': '--...',  '8': '---..',
        '9': '----.' 
        }
class ThreadedServer(object):
	def __init__(self, host, port):
		self.host = host
		self.port = port
		self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		self.sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
		self.sock.bind((self.host, self.port))
		

	def listen(self):
		self.sock.listen(5)
		while True:
			client, address = self.sock.accept()
            		client.settimeout(3)
            		threading.Thread(target = self.main_server,args = (client,address)).start()
			threading.Thread(target = self.count_down,args = (client,address)).start()

	
 	def count_down(self,client,address):
		i=20
		
		
		while (i > 0):
			
			i = i - 1 
			client.send(str(i))
			time.sleep(1) 
		client.close()
		return False
	
	def generator(self):
		string_val = "".join(choice(lowercase) for j in range(n))
		return string_val
	def main_server(self,client, address):
		client.send(u"Ok let's begin ! you have 3 seconds to find my secret \n")
		i = 20
		key = self.generator()
		print key
		L=["sha256","sha1","md5"]
		r1=random.choice(L)
		r2=random.choice(L)
		r3=random.choice(L)	
		msg = "whatisthe"+r1+"("+r2+"("+r3+"("+ key+")))"
		hx=msg.encode("hex")
		if(r1 == "sha256"):
			hash_key=sha256(key).hexdigest()
		elif(r1 == "sha1"):
			hash_key = sha1(key).hexdigest()
		elif(r1 == "md5"):
			hash_key = md5(key).hexdigest()
				# the r2 hash
		if(r2 == "sha256"):
			hash_key2=sha256(hash_key).hexdigest()
		elif(r2 == "sha1"):
			hash_key2 = sha1(hash_key).hexdigest()
		elif(r2 == "md5"):
			hash_key2 = md5(hash_key).hexdigest()
				#the r3 hash
		if(r3 == "sha256"):
			hash_key3=sha256(hash_key2).hexdigest()
		elif(r3 == "sha1"):
			hash_key3 = sha1(hash_key2).hexdigest()
		elif(r3 == "md5"):
			hash_key3 = md5(hash_key2).hexdigest()
		print hash_key3
		while 1:
		
			
			
				
				
				
		
			for char in hx:
				
				client.send (CODE[char.upper()]+" ",)
			client.send( "\n")
				
				
				
				
				
				
				

			response = client.recv(255).strip()
			print response
			if (response == hash_key3) :
				client.send( u"GOOD JOB , the flag is HZV{DOnt_w0rry_1ts_just_4_w4rm_up}\n")
				return False
					
			elif (response != hash_key):
				client.send( u"Failed\n")
			
			


		
if __name__ == "__main__":
	port_num = input("Port? ")
	ThreadedServer('',port_num).listen()
