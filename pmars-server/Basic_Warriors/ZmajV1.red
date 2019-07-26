;redcode-94nop
;name Zmaj v1.0
;author Bosko Ristovic

bomb	spl 0
loop	add #3039, ptr
ptr	mov bomb, 81
	djn loop, <4800
	mov 1, <-1
	end bomb
