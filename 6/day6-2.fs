\ Day 6, part 2

[DEFINED] EMPTY [IF] EMPTY [THEN] MARKER EMPTY
include ../ttester.fs
CLEARSTACK DECIMAL

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
        OVER I SWAP AT   DUP BL = IF  DROP LEAVE  THEN
        '0' - SWAP 10 * +
    LOOP NIP ;

: +? ( col -- f )  ROWS SWAP AT '+' = ;
: START ( col -- n )      +? IF 0 ELSE 1 THEN ;
: OP ( n1 n2 col -- n3 )  +? IF + ELSE * THEN ;

: CALC ( col -- n )  DUP START SWAP
    DUP DUP #COL + SWAP DO  I NUM  I OP   LOOP ;

S" example.txt" INPUT
