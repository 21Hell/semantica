#make_COM#
include emu8086.inc
ORG 100h
PRINTN "Introduce la altura de la piramide: "
CALL SCAN_NUM
MOV altura, CX
MOV AX,altura
PUSH AX
MOV AX,2
PUSH AX
POP BX
POP AX
CMP AX, BX
JLE if1
MOV AX,altura
PUSH AX
POP AX
MOV i, AX
inicioFor1:
MOV AX,i
PUSH AX
MOV AX,0
PUSH AX
POP BX
POP AX
CMP AX, BX
JLE finFor1
MOV AX,1
PUSH AX
MOV AX,0
PUSH AX
POP AX
MOV j, AX
MOV AX,j
PUSH AX
MOV AX,altura
PUSH AX
MOV AX,i
PUSH AX
POP AX
POP BX
SUB AX, BX
PUSH AX
POP BX
POP AX
CMP AX, BX
JGE WHILE0:
MOV AX,j
PUSH AX
MOV AX,2
PUSH AX
POP AX
POP BX
DIV BX
PUSH DX
MOV AX,0
PUSH AX
POP BX
POP AX
CMP AX, BX
JNE if2
JMP Fin2
else2:
PRINTN "-"
JMP Fin2
if2:
JMP else2
Fin2:
MOV AX,1
PUSH AX
POP AX
ADD j, AX
MOV j, AX
JMP WHILE0:
FINWHILE0:
PRINTN "
"
POP AX
ADD j, AX
MOV j, AX
JMP inicioFor1
finFor1:
else2:
else2:
else2:
else2:
else2:
else2:
else2:
else2:
else2:
else2:
else2:
else2:
else2:
else2:
else2:
MOV AX,0
PUSH AX
POP AX
MOV k, AX
DO0:
PRINTN "-"
MOV AX,2
PUSH AX
POP AX
ADD k, AX
MOV k, AX
MOV AX,k
PUSH AX
MOV AX,altura
PUSH AX
MOV AX,2
PUSH AX
POP AX
POP BX
MUL BX
PUSH AX
POP BX
POP AX
CMP AX, BX
JGE DO0:
JMP DO0:
FINDO0:
PRINTN "
"
JMP Fin2
else1:
JMP Fin2
if1:
JMP else1
Fin2:
MOV AX,1
PUSH AX
MOV AX,1
PUSH AX
POP BX
POP AX
CMP AX, BX
JE if3
MOV AX,2
PUSH AX
MOV AX,2
PUSH AX
POP BX
POP AX
CMP AX, BX
JNE if4
JMP Fin4
if4:
JMP else4
Fin4:
JMP Fin4
if3:
JMP else3
Fin4:
MOV AX,258
PUSH AX
POP AX
MOV a, AX
PRINTN "Valor de variable int 'a' antes del casteo: "
MOV AX,a
PUSH AX
POP AX
CALL PRINT_NUM
MOV AX,a
PUSH AX
POP AX
MOV AL, AH
PUSH AX
POP AX
MOV y, AX
PRINTN "
Valor de variable char 'y' despues del casteo de a: "
MOV AX,y
PUSH AX
POP AX
CALL PRINT_NUM
PRINTN "
A continuacion se intenta asignar un int a un char sin usar casteo: 
"

;Variables
	area DW ?
	radio DW ?
	pi DW ?
	resultado DW ?
	a DW ?
	d DW ?
	altura DW ?
	cinco DW ?
	x DW ?
	y DW ?
	i DW ?
	j DW ?
	k DW ?
RET
DEFINE_SCAN_NUM
DEFINE_PRINT_NUM
DEFINE_PRINT_NUM_UNS
END
