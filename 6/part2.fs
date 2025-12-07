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

: NUM ( col -- n )  0 ( n )
    ROWS 0 DO
        OVER I SWAP AT  DUP BL - IF  '0' -  SWAP 10 * +  ELSE DROP THEN
    LOOP NIP ;

VARIABLE OP
: OP! ( col -- )  ROWS SWAP AT ( c )
    DUP '+' =  OVER '*' =  OR IF  OP !  ELSE  DROP  THEN ;
: DO-OP ( n1 n2 -- n3 ) OP @ '+' = IF + ELSE * THEN ;

VARIABLE TOTAL
: ACT  ( 0 n1...nX -- 0 )
    BEGIN  OVER WHILE  DO-OP  REPEAT  TOTAL +! ;

: COLUMN ( 0 n1 n2 ... nX col -- 0 n1 n2 ... nX nX+1 | 0 )
    DUP NUM  DUP 0= IF  2DROP  ACT  ELSE  SWAP OP!  THEN ;

: SOLVE ( -- n )  TOTAL OFF
     0  WIDTH 0 DO  I COLUMN  LOOP  ACT DROP  TOTAL @ ;


S" example.txt" INPUT
T{ SOLVE -> 3263827 }T

S" input.txt" INPUT
T{ SOLVE -> 10142723156431 }T
