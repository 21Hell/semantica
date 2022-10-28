#make_COM
include 'emu8086.inc'
ORG 1000h
;Variables
	area DW ?
	radio DW ?
	pi DW ?
	resultado DW ?
	a DW ?
	d DW ?
	altura DW ?
	x DW ?
	y DW ?
	i DW ?
	j DW ?
	k DW ?
	l DW ?
MOV AX,1
PUSH AX
POP AX
MOV i, AX
POP AX
MOV AX,1
PUSH AX
MOV AX,3
PUSH AX
POP AX
POP BX
ADD AX, BX
PUSH AX
INC i
POP AX
RET
END
