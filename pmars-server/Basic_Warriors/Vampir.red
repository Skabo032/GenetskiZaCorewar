;name Vampire 2 
const 	EQU 2365 
	SPL 0 		; self splitting 
vamp 	MOV ptr, @ptr 	; throw pointer 
	ADD data, ptr 	; update pointer 
	DJN vamp, <2339 ; loop back + non-lethal attack 
ptr 	JMP trap, ptr 	; pointer to... 
trap 	SPL 1, -100 	; ...here 
	MOV data, <-1 
	JMP -2 
data	DAT #const, #-const
