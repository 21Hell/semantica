#make_COM
include 'emu8086.inc'
ORG 1000h
MOV AX,0
PUSH AX
POP AX
MOV i, AX
inicioFor1:
MOV AX,i
PUSH AX
MOV AX,3
PUSH AX
POP AX
POP BX
CMP AX, BX
JGE finFor1
MOV AX,i
PUSH AX
POP AX
CALL PRINT_NUM
INC i
JMP inicioFor1
finFor1:

;Variables
	area DW ?
	area DD ?
	radio DW ?
	radio DD ?
	pi DW ?
	pi DD ?
	resultado DW ?
	resultado DD ?
	a DW ?
	a DW ?
	d DW ?
	d DW ?
	altura DW ?
	altura DW ?
	x DW ?
	x DD ?
	y DW ?
	y DW ?
	i DW ?
	i DB ?
	j DW ?
	j DW ?
	k DW ?
	k DW ?
	l DW ?
	l DW ?
RET
DEFINE_SCAN_NUM
DEFINE_PRINT_NUM
DEFINE_PRINT_STR
END
