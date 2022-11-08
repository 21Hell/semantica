#make_COM#
include emu8086.inc
ORG 100h
PRINTN "Introduce la altura de la piramide: "
MOV AX,10
PUSH AX
POP AX
MOV altura, AX
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
POP AXADD j, AXMOV j, AX
JMP WHILE0:
FINWHILE0:
PRINTN "
"
POP AXADD j, AXMOV j, AX
JMP inicioFor1
finFor1:
JMP Fin3
else3:
JMP Fin3
if3:
JMP else3
Fin3:
JMP Fin4
else4:
JMP Fin4
if4:
JMP else4
Fin4:
JMP Fin5
else5:
JMP Fin5
if5:
JMP else5
Fin5:
JMP Fin6
else6:
JMP Fin6
if6:
JMP else6
Fin6:
JMP Fin7
else7:
JMP Fin7
if7:
JMP else7
Fin7:
JMP Fin8
else8:
JMP Fin8
if8:
JMP else8
Fin8:
JMP Fin9
else9:
JMP Fin9
if9:
JMP else9
Fin9:
JMP Fin10
else10:
JMP Fin10
if10:
JMP else10
Fin10:
JMP Fin11
else11:
JMP Fin11
if11:
JMP else11
Fin11:
JMP Fin12
else12:
JMP Fin12
if12:
JMP else12
Fin12:
JMP Fin13
else13:
JMP Fin13
if13:
JMP else13
Fin13:
JMP Fin14
else14:
JMP Fin14
if14:
JMP else14
Fin14:
JMP Fin15
else15:
JMP Fin15
if15:
JMP else15
Fin15:
JMP Fin16
else16:
JMP Fin16
if16:
JMP else16
Fin16:
JMP Fin17
else17:
JMP Fin17
if17:
JMP else17
Fin17:
JMP Fin18
else18:
JMP Fin18
if18:
JMP else18
Fin18:
JMP Fin19
else19:
JMP Fin19
if19:
JMP else19
Fin19:
JMP Fin20
else20:
JMP Fin20
if20:
JMP else20
Fin20:
JMP Fin21
else21:
JMP Fin21
if21:
JMP else21
Fin21:
JMP Fin22
else22:
JMP Fin22
if22:
JMP else22
Fin22:
JMP Fin23
else23:
JMP Fin23
if23:
JMP else23
Fin23:
JMP Fin24
else24:
JMP Fin24
if24:
JMP else24
Fin24:
JMP Fin25
else25:
JMP Fin25
if25:
JMP else25
Fin25:
JMP Fin26
else26:
JMP Fin26
if26:
JMP else26
Fin26:
JMP Fin27
else27:
JMP Fin27
if27:
JMP else27
Fin27:
JMP Fin28
else28:
JMP Fin28
if28:
JMP else28
Fin28:
JMP Fin29
else29:
JMP Fin29
if29:
JMP else29
Fin29:
JMP Fin30
else30:
JMP Fin30
if30:
JMP else30
Fin30:
JMP Fin31
else31:
JMP Fin31
if31:
JMP else31
Fin31:
JMP Fin32
else32:
JMP Fin32
if32:
JMP else32
Fin32:
JMP Fin33
else33:
JMP Fin33
if33:
JMP else33
Fin33:
JMP Fin34
else34:
JMP Fin34
if34:
JMP else34
Fin34:
JMP Fin35
else35:
JMP Fin35
if35:
JMP else35
Fin35:
JMP Fin36
else36:
JMP Fin36
if36:
JMP else36
Fin36:
JMP Fin37
else37:
JMP Fin37
if37:
JMP else37
Fin37:
JMP Fin38
else38:
JMP Fin38
if38:
JMP else38
Fin38:
JMP Fin39
else39:
JMP Fin39
if39:
JMP else39
Fin39:
JMP Fin40
else40:
JMP Fin40
if40:
JMP else40
Fin40:
JMP Fin41
else41:
JMP Fin41
if41:
JMP else41
Fin41:
JMP Fin42
else42:
JMP Fin42
if42:
JMP else42
Fin42:
JMP Fin43
else43:
JMP Fin43
if43:
JMP else43
Fin43:
JMP Fin44
else44:
JMP Fin44
if44:
JMP else44
Fin44:
JMP Fin45
else45:
JMP Fin45
if45:
JMP else45
Fin45:
JMP Fin46
else46:
JMP Fin46
if46:
JMP else46
Fin46:
JMP Fin47
else47:
JMP Fin47
if47:
JMP else47
Fin47:
JMP Fin48
else48:
JMP Fin48
if48:
JMP else48
Fin48:
JMP Fin49
else49:
JMP Fin49
if49:
JMP else49
Fin49:
JMP Fin50
else50:
JMP Fin50
if50:
JMP else50
Fin50:
JMP Fin51
else51:
JMP Fin51
if51:
JMP else51
Fin51:
JMP Fin52
else52:
JMP Fin52
if52:
JMP else52
Fin52:
JMP Fin53
else53:
JMP Fin53
if53:
JMP else53
Fin53:
JMP Fin54
else54:
JMP Fin54
if54:
JMP else54
Fin54:
JMP Fin55
else55:
JMP Fin55
if55:
JMP else55
Fin55:
JMP Fin56
else56:
JMP Fin56
if56:
JMP else56
Fin56:
JMP Fin57
else57:
JMP Fin57
if57:
JMP else57
Fin57:
MOV AX,0
PUSH AX
POP AX
MOV k, AX
DO0:
PRINTN "-"
MOV AX,2
PUSH AX
POP AXADD k, AXMOV k, AX
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
JMP Fin57
else1:
JMP Fin57
if1:
JMP else1
Fin57:
MOV AX,1
PUSH AX
MOV AX,1
PUSH AX
POP BX
POP AX
CMP AX, BX
JE if58
MOV AX,2
PUSH AX
MOV AX,2
PUSH AX
POP BX
POP AX
CMP AX, BX
JNE if59
JMP Fin59
if59:
JMP else59
Fin59:
JMP Fin59
if58:
JMP else58
Fin59:
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
