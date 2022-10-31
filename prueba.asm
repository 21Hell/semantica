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
MOV AX,255
PUSH AX
POP AX
MOV i, AX
MOV AX,10
PUSH AX
INC i
RET
END
