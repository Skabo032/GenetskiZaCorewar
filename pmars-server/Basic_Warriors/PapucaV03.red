;name Papuca v0.3
bomb	SPL 0
loop	ADD #3039, ptr
ptr 	MOV bomb, 81
		SPL 0
		ADD #3039, 1
		MOV bomb-1, 80
		JMP loop
		MOV 1, <-1