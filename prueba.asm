#make_COM#
include emu8086.inc
ORG 100h
;Variables: 
	 area DW 0
	 radio DW 0
	 pi DW 0
	 resultado DW 0
	 a DW 0
	 d DW 0
	 altura DW 0
	 cinco DW 0
	 x DW 0
	 y DW 0
	 i DW 0
	 j DW 0
	 k DW 0
PRINT '"Introduce la altura de la piramide: "'
MOV AX, 10
PUSH AX
POP AX
MOV altura, AX
MOV AX, altura
PUSH AX
MOV AX, 2
PUSH AX
POP BX
POP AX
CMP AX, BX
JLE if1
MOV AX, altura
PUSH AX
POP AX
MOV i, AX
inicioFor1:
MOV AX, i
PUSH AX
MOV AX, 0
PUSH AX
POP BX
POP AX
CMP AX, BX
JLE finFor1
MOV AX, 1
PUSH AX
MOV AX, 0
PUSH AX
POP AX
MOV j, AX
WHILEINICO1:
MOV AX, j
PUSH AX
MOV AX, altura
PUSH AX
MOV AX, i
PUSH AX
POP BX
POP AX
SUB AX, BX
PUSH AX
POP BX
POP AX
CMP AX, BX
JGE FINWHILE1:
MOV AX, j
PUSH AX
MOV AX, 2
PUSH AX
POP BX
POP AX
DIV BX
PUSH DX
MOV AX, 0
PUSH AX
POP BX
POP AX
CMP AX, BX
JNE if2
PRINT '"*"'
JMP else2
if2:
PRINT '"-"'
else2:
MOV AX, 1
PUSH AX
POP AX
ADD j, AX
JMP WHILEINICO1:
FINWHILE1:
PRINTN '"'
PRINT '"'
POP AX
SUB i, 1
JMP inicioFor1
finFor1:
MOV AX, 0
PUSH AX
POP AX
MOV k, AX
DO1:
PRINT '"-"'
MOV AX, 2
PUSH AX
POP AX
ADD k, AX
MOV AX, k
PUSH AX
MOV AX, altura
PUSH AX
MOV AX, 2
PUSH AX
POP BX
POP AX
MUL BX
PUSH AX
POP BX
POP AX
CMP AX, BX
JGE FINDO1:
JMP DO1:
FINDO1:
PRINTN '"'
PRINT '"'
JMP else1
if1:
PRINTN '"'
PRINTN 'Error: la altura debe de ser mayor que 2'
PRINT '"'
else1:
MOV AX, 1
PUSH AX
MOV AX, 1
PUSH AX
POP BX
POP AX
CMP AX, BX
JE if3
PRINT '"Esto no se debe imprimir"'
MOV AX, 2
PUSH AX
MOV AX, 2
PUSH AX
POP BX
POP AX
CMP AX, BX
JNE if4
PRINT '"Esto tampoco"'
JMP else4
if4:
else4:
JMP else3
if3:
else3:
MOV AX, 258
PUSH AX
POP AX
MOV a, AX
PRINT '"Valor de variable int a antes del casteo: "'
MOV AX, a
PUSH AX
POP AX
CALL PRINT_NUM
MOV AX, a
PUSH AX
POP AX
MOV AL, AH
PUSH AX
POP AX
MOV y, AX
PRINTN '"'
PRINT 'Valor de variable char y despues del casteo de a: "'
MOV AX, y
PUSH AX
POP AX
CALL PRINT_NUM
PRINTN '"'
PRINTN 'A continuacion se intenta asignar un int a un char sin usar casteo: '
PRINT '"'

RET
DEFINE_SCAN_NUM
DEFINE_PRINT_NUM
DEFINE_PRINT_NUM_UNS
END
