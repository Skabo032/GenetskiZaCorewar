;name Papuca v0.5
bomb	SPL 0
loop	MOV bomb-1, <-3
	ADD #3039, ptr
ptr	MOV bomb, 81
	JMP loop
	MOV 1, <-1
