;Papir
cnt	EQU lst-src
src	MOV #cnt, 0
	MOV <src, <dst
	JMP -1, src
dst 	SPL @0, 1222
	SUB #23, dst
	JMZ src, src
lst 	END src
