\ Day 6, part 2

include ../init.fs

VARIABLE IN
10000 CONSTANT MAX-LINE
: ?ERR   ABORT" File I/O error" ;
: OPEN   ( a n -- )  R/O OPEN-FILE ?ERR  IN ! ;
: CLOSE  IN @ CLOSE-FILE ?ERR ;
: READ   ( a -- n flag )  MAX-LINE IN @ READ-LINE ?ERR ;

\ read as text
0 VALUE DATA
0 VALUE ROWS ( just the numbers )
0 VALUE WIDTH

: 'AT ( row col -- a )  SWAP WIDTH * +  DATA + ;
: AT  ( row col -- c )  'AT C@ ;

: INPUT  ( a n -- )
    ALIGN  HERE TO DATA  -1 TO ROWS  OPEN
    BEGIN  HERE READ WHILE  DUP ALLOT  TO WIDTH  ROWS 1+ TO ROWS
    REPEAT DROP CLOSE ;

: .ROW ( n -- )  WIDTH * DATA + WIDTH TYPE SPACE ;
: .DATA  ROWS 1+ 0 DO CR I .ROW LOOP ;

\ find the width of a column from the operations row
: #COL ( col -- n )
    ROWS SWAP 'AT 1+  DUP BEGIN COUNT BL > UNTIL  1-  SWAP - ;

: NUM ( col -- n )  0 ( n )
    ROWS 0 DO
        OVER I SWAP AT  DUP BL - IF  '0' -  SWAP 10 * +  ELSE DROP THEN
    LOOP NIP ;

: START ( col -- 0/1 )  ROWS SWAP AT '+' = 1+ ;

DEFER OP ( n1 n2 -- n3 )
variable calcs
: CALC ( col -- n )   1 calcs +!
    DUP START ( col n )
    DUP IF ['] * ELSE ['] + THEN  IS OP ( set operation )
    SWAP DUP #COL BOUNDS DO  I NUM  OP  LOOP ;

: TOTAL ( -- n )  0 0
    BEGIN ( n col )
        DUP CALC  ROT + SWAP
        DUP #COL 1+ + ( next col)
        DUP WIDTH >=
    UNTIL DROP ;

S" example.txt" INPUT

T{ 0 CALC -> 8544 }T
T{ 4 CALC -> 625 }T
T{ 8 CALC -> 3253600 }T
T{ 12 CALC -> 1058 }T
T{ TOTAL -> 3263827 }T

S" input.txt" INPUT
\  9716259487083 too low
\ 10142723154972 too low

