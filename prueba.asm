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
POP AX
MOV y, AX
POP AX
POP BX
CMP AX, BX
JNE if1
POP AX
MOV y, AX
if1:
RET
END
