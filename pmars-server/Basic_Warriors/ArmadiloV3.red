;name Armadilo v3
bomb	spl 0
loop	add #3039, ptr
ptr	mov bomb, 83
	spl loop
	mov 2, <-5
	djn loop, <3800
