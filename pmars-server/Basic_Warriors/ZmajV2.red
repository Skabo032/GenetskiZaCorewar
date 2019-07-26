;redcode-94nop
;name Zmaj v1.1
;author Bosko Ristovic

bomb	spl 2
loop	mov bomb, 81
	add #3039, -1
	djn loop, <4800
	mov 1, <-1
	end bomb
